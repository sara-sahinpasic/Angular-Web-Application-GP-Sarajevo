using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Tickets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PdfSharp.Pdf;
using System.Text;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace Infrastructure.Services.File;

public sealed class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IConfiguration _configuration;

    public FileService(IWebHostEnvironment hostingEnvironment, IIssuedTicketRepository issuedTicketRepository, IConfiguration configuration)
    {
        _hostingEnvironment = hostingEnvironment;
        _issuedTicketRepository = issuedTicketRepository;
        _configuration = configuration;
    }

    public async Task<string?> UploadFileAsync(string[] acceptedExtensions, IFormFile file, string folderName = "", CancellationToken cancellationToken = default)
    {
        if (!IsValidExtension(acceptedExtensions, file))
        {
            return null;
        }

        string uploadsDir = Path.Combine(_hostingEnvironment.ContentRootPath, folderName); 

        if (!Directory.Exists(uploadsDir))
            Directory.CreateDirectory(uploadsDir);

        string fileName = GetRandomFileName(file);
        string fullPath = Path.Combine(uploadsDir, fileName);

        await using FileStream stream = new(fullPath, FileMode.Create, FileAccess.Write, FileShare.None);
        await file.CopyToAsync(stream, cancellationToken);
        await stream.FlushAsync(cancellationToken);

        return fullPath;
    }    


    private static string GetRandomFileName(IFormFile file)
    {
        string fileName = Path.GetRandomFileName() + "." + file.ContentType.Split("/")[1];
        return fileName;
    }

    private static bool IsValidExtension(string[] validExtensions, IFormFile file)
    {
        string extension = file.ContentType.Split("/")[1];
        return validExtensions.Contains(extension);
    }

    public async Task<byte[]> GeneratePurchaseHistoryPDFAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        ICollection<IssuedTicket> issuedTickets = await _issuedTicketRepository.GetAll()
            .Include(issuedTicket => issuedTicket.Ticket)
            .Where(issuedTicket => issuedTicket.UserId == userId)
            .Select(issuedTicket => new IssuedTicket
            {
                Ticket = issuedTicket.Ticket,
                IssuedDate = issuedTicket.IssuedDate
            })
            .ToArrayAsync();

        string purchaseHistoryHtml = GetPurchaseHistoryTableHtml(issuedTickets);

        PdfDocument pdfDocument = PdfGenerator.GeneratePdf(purchaseHistoryHtml, PdfSharp.PageSize.A4);
        using MemoryStream memoryStream = new();
        
        pdfDocument.Save(memoryStream, true);
        byte[] pdfBytes = memoryStream.ToArray();

        return pdfBytes;
    }

    private string GetPurchaseHistoryTableHtml(ICollection<IssuedTicket> issuedTickets)
    {
        StringBuilder stringBuilder = new();

        AppendStyles(stringBuilder);

        stringBuilder.AppendLine("<h1>Historija kupovine</h1>");
        stringBuilder.AppendLine("<div class=\"table-container\">");
        stringBuilder.AppendLine("<table>");
        stringBuilder.AppendLine(@"<thead>
            <tr>
              <th scope=""col"">Tip karte</th>
              <th scope=""col"">Relacija </th>
              <th scope=""col"">Cijena</th>
              <th scope=""col"">Datum</th>
            </tr>
          </thead>");

        foreach (IssuedTicket issuedTicket in issuedTickets)
        {
            stringBuilder.AppendLine($@"<tbody>
            <tr>
              <td>{issuedTicket.Ticket.Name}</td>
              <td>relationId</td>
              <td>{issuedTicket.Ticket.Price}</td>
              <td>{issuedTicket.IssuedDate.ToString("d")}</td>
            </tr>
          </tbody>");
        }

        stringBuilder.AppendLine("</table>");
        stringBuilder.AppendLine("</div>");

        return stringBuilder.ToString();
    }

    private void AppendStyles(StringBuilder stringBuilder)
    {
        string pdfStylesPath = _configuration.GetSection("PDFStylesPath").Get<string>();
        string contents = System.IO.File.ReadAllText(Path.GetFullPath("..\\" + pdfStylesPath + "\\PDF.css"));

        stringBuilder.AppendLine($"<style>{contents}</style>");
    }
}