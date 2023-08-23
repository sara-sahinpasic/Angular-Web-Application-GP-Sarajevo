using Application.Services.Abstractions.Interfaces.File;
using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Domain.Enums.PaymentOption;
using FluentAssertions;
using Infrastructure.Services.File;
using PdfSharp.Pdf;

namespace UnitTests.Services.Application.File;

public sealed class PDFGeneratorServiceTests
{
    private readonly IPDFGeneratorService _pdfGeneratorService = new PDFGeneratorService();
    private readonly IEnumerable<IssuedTicket> _issuedTickets;
    private readonly Invoice _invoice;
    private readonly bool shouldGeneratePDFs = false;

    public PDFGeneratorServiceTests()
    {
        // setup
        Guid ticketId = new();
        Guid userId = Guid.NewGuid();
        _issuedTickets = new List<IssuedTicket>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 100,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 3.2,
                },
                User = new()
                {
                    Active = true,
                    DateOfBirth = DateTime.UtcNow,
                    Email = "someEmail@amor.com",
                    FirstName = "Test",
                    LastName = "Test",
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 25,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 3.2,
                },
                User = new()
                {
                    Active = true,
                    DateOfBirth = DateTime.UtcNow,
                    Email = "someEmail@amor.com",
                    FirstName = "Test",
                    LastName = "Test",
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 50,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 3.2,
                },
                User = new()
                {
                    Active = true,
                    DateOfBirth = DateTime.UtcNow,
                    Email = "someEmail@amor.com",
                    FirstName = "Test",
                    LastName = "Test",
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 40,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 103.2,
                },
                User = new()
                {
                    Active = true,
                    DateOfBirth = DateTime.UtcNow,
                    Email = "someEmail@amor.com",
                    FirstName = "Test",
                    LastName = "Test",
                }
            }
        };
        _invoice = new()
        {
            InvoicingDate = DateTime.UtcNow,
            IssuedTickets = _issuedTickets,
            PaymentOption = PaymentOptions.Card,
            Total = _issuedTickets.Sum(issuedTicket => issuedTicket.Ticket.Price),
            User = new()
            {
                Active = true,
                DateOfBirth = DateTime.UtcNow,
                Email = "someEmail@amor.com",
                FirstName = "Test",
                LastName = "Test",
            },
            Tax = new()
            {
                Name = "TestTax",
                Percentage = 0.17,
                Active = true
            }
        };

        _invoice.TotalWithoutTax = _invoice.Total - (_invoice.Total * _invoice.Tax.Percentage);
    }

    [Fact]
    public void CreateInvoicePDFDocument_ShouldReturnInvoicePDF_WhenInvoiceExists()
    {
        PdfDocument pdfDocument = _pdfGeneratorService.CreateInvoicePDFDocument(_invoice);

        pdfDocument.Should().NotBeNull();

        // for testing the pdf contents
        if (shouldGeneratePDFs)
        {
            CreatePDF(_invoice.Id + ".pdf", pdfDocument);
        }
    }

    [Fact]
    public void CreateInvoicePDFDocument_ShoulThrowException_WhenInvalidInvoiceProvided()
    {
        Invoice faultyInvoice = new()
        {
            IssuedTickets = _issuedTickets,
            User = null
        };

        Action act = () => _pdfGeneratorService.CreateInvoicePDFDocument(faultyInvoice);

        act.Should().Throw<Exception>();
    }

    private static void CreatePDF(string pdfName, PdfDocument pdfDocument)
    {
        if (!Directory.Exists("TestPDFs"))
        {
            Directory.CreateDirectory("TestPDFs");
        }

        pdfDocument.Save($"TestPDFs/{pdfName}");
        pdfDocument.Close();
    }

    [Fact]
    public void CreateIssuedTicketsPDFDocument_ShouldReturnIssuedTicketsPDF_WhenIssuedTicketsExist()
    {
        PdfDocument pdfDocument = _pdfGeneratorService.CreateIssuedTicketsPDFDocument(_invoice);

        pdfDocument.Should().NotBeNull();

        // for testing the pdf contents
        if (shouldGeneratePDFs)
        {
            CreatePDF(_invoice.Id + "_tickets.pdf", pdfDocument);
        }
    }   

    [Fact]
    public void CreatePurchaseHistoryPDFDocument_ShouldReturnPurchaseHistoryPDF()
    {
        ICollection<IssuedTicket> moreTicketsToTest = _issuedTickets.Concat(_issuedTickets)
            .Concat(_issuedTickets)
            .Concat(_issuedTickets)
            .ToList();

        PdfDocument pdfDocument = _pdfGeneratorService.CreatePurchaseHistoryPDFDocument(moreTicketsToTest);

        // for testing the pdf contents
        if (shouldGeneratePDFs)
        {
            CreatePDF(_invoice.User.Id + "_history.pdf", pdfDocument);
        }
    }
}
