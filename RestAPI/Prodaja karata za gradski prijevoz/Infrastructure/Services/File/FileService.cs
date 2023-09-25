using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PdfSharp.Pdf;
using System.Text;
using IOFile = System.IO.File;

namespace Infrastructure.Services.File;

public sealed class FileService : IFileService
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IPDFGeneratorService _pdfGeneratorService;

    public FileService(IWebHostEnvironment hostingEnvironment, IIssuedTicketRepository issuedTicketRepository, IPDFGeneratorService pdfGeneratorService)
    {
        _hostingEnvironment = hostingEnvironment;
        _issuedTicketRepository = issuedTicketRepository;
        _pdfGeneratorService = pdfGeneratorService;

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
    }

    public async Task<string?> SaveFileAsync(string[] acceptedExtensions, IFormFile file, string folderName = "", CancellationToken cancellationToken = default)
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

    public async Task<byte[]> GeneratePurchaseHistoryPDFAsync(Guid userId, bool shouldGenerateLocalFile = false, CancellationToken cancellationToken = default)
    {
        ICollection<IssuedTicket> issuedTickets = await _issuedTicketRepository.GetUserIssuedTicketsForPurchaseHistoryReportAsync(userId, cancellationToken);

        PdfDocument document = _pdfGeneratorService.CreatePurchaseHistoryPDFDocument(issuedTickets);

        using MemoryStream memoryStream = new();

        document.Save(memoryStream, false);
        byte[] pdfBytes = memoryStream.ToArray();

        return pdfBytes;
    }


    public byte[] GenerateInvoicePDF(Invoice invoice, bool shouldGenerateLocalFile = false)
    {
        PdfDocument pdfDocument = _pdfGeneratorService.CreateInvoicePDFDocument(invoice);

        using MemoryStream memoryStream = new();

        pdfDocument.Save(memoryStream, true);
        byte[] pdfBytes = memoryStream.ToArray();

        if (shouldGenerateLocalFile)
        {
            CreatePDF($"{invoice.Id}.pdf", pdfBytes);
        }

        return pdfBytes;
    }

    private static void CreatePDF(string pdfName, byte[] pdfBytes)
    {
        if (!Directory.Exists("PDFs"))
        {
            Directory.CreateDirectory("PDFs");
        }

        IOFile.WriteAllBytes($"PDFs/{pdfName}", pdfBytes);
    }

    public byte[] GenerateIssuedTicketPDF(Invoice invoice, bool shouldGenerateLocalFile = false)
    {
        PdfDocument pdf = _pdfGeneratorService.CreateIssuedTicketsPDFDocument(invoice);
        
        using MemoryStream memoryStream = new();

        pdf.Save(memoryStream, true);

        byte[] pdfData = memoryStream.ToArray();

        if (shouldGenerateLocalFile)
        {
            CreatePDF($"{invoice.Id}_Tickets.pdf", pdfData);
        }

        return pdfData;
    }
}