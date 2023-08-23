using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using PdfSharp.Pdf;

namespace Application.Services.Abstractions.Interfaces.File;

public interface IPDFGeneratorService
{
    public PdfDocument CreateInvoicePDFDocument(Invoice invoice);
    public PdfDocument CreateIssuedTicketsPDFDocument(Invoice invoice);
    public PdfDocument CreatePurchaseHistoryPDFDocument(IEnumerable<IssuedTicket> issuedTickets);
}
