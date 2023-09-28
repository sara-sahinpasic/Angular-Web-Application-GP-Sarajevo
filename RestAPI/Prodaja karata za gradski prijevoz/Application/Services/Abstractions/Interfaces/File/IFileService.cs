using Domain.Entities.Invoices;
using Microsoft.AspNetCore.Http;

namespace Application.Services.Abstractions.Interfaces.File;

public interface IFileService
{
    Task<string?> SaveFileAsync(string[] acceptedExtensions, IFormFile file, string folderName = "", CancellationToken cancellationToken = default);
    Task<byte[]> GeneratePurchaseHistoryPDFAsync(Guid userId, bool shouldGenerateLocalFile = false, CancellationToken cancellationToken = default);
    byte[] GenerateInvoicePDF(Invoice invoice, bool shouldGenerateLocalFile = false);
    byte[] GenerateIssuedTicketPDF(Invoice invoice, bool shouldGenerateLocalFile = false);
}                                                                    

