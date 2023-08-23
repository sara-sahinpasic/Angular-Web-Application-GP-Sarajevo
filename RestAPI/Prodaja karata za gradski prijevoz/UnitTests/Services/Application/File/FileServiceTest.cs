using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Domain.Enums.PaymentOption;
using FakeItEasy;
using FluentAssertions;
using Infrastructure.Services.File;
using Microsoft.AspNetCore.Hosting;
using PdfSharp.Pdf;

namespace UnitTests.Services.Application.File;

public sealed class FileServiceTest
{
	private readonly IFileService _fileService;
    private readonly Guid userId = Guid.NewGuid();
    private readonly IEnumerable<IssuedTicket> _issuedTickets;
    private readonly Invoice _invoice;

    public FileServiceTest()
    {
        // setup
        IWebHostEnvironment hostingEnvironment = A.Dummy<IWebHostEnvironment>();
        IPDFGeneratorService pdfGeneratorService = A.Dummy<IPDFGeneratorService>();
        IIssuedTicketRepository issuedTicketRepository = A.Fake<IIssuedTicketRepository>();

        Guid ticketId = new();
        
        _issuedTickets = new List<IssuedTicket>()
        {
            new()
            {
                Id = Guid.NewGuid(),
                Amount = 1,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 1,
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
                Amount = 1,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 1,
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
                Amount = 1,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 1,
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
                Amount = 1,
                UserId = userId,
                IssuedDate = DateTime.UtcNow,
                Ticket = new()
                {
                    Active = true,
                    Id = ticketId,
                    Name = "Test",
                    Price = 1,
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
            Total = 4,
            User = new()
            {
                Active = true,
                DateOfBirth = DateTime.UtcNow,
                Email = "someEmail@amor.com",
                FirstName = "Test",
                LastName = "Test",
            }
        };

        IssuedTicket[] issuedTicketsArray = _issuedTickets.ToArray();

        PdfDocument pdfDocument = new();
        pdfDocument.AddPage();
        
        A.CallTo(() => issuedTicketRepository.GetUserIssuedTicketsForPurchaseHistoryReportAsync(userId, default)).Returns(Task.FromResult(issuedTicketsArray));
        A.CallTo(() => pdfGeneratorService.CreatePurchaseHistoryPDFDocument(issuedTicketsArray)).Returns(pdfDocument);
        A.CallTo(() => pdfGeneratorService.CreateInvoicePDFDocument(_invoice)).Returns(pdfDocument);
        A.CallTo(() => pdfGeneratorService.CreateIssuedTicketsPDFDocument(_invoice)).Returns(pdfDocument);

        _fileService = new FileService(hostingEnvironment, issuedTicketRepository, pdfGeneratorService);
    }

    [Fact]
    public void GenerateIssuedTicketPDFAsync_ShouldReturnByteData_WhenIssuedTicketPDFIsGenerated()
    {
        byte[] data = _fileService.GenerateIssuedTicketPDF(_invoice);
        data.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GeneratePurchaseHistoryPDFAsync_ShouldReturnByteData_WhenPurchaseHistoryHasBeenBuilt()
    {
        byte[] data = await _fileService.GeneratePurchaseHistoryPDFAsync(userId);

        data.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void GenerateInvoicePDF_ShouldReturnInvoicePdfByteData_WhenProvidedValidInvoice()
    {
        byte[] data = _fileService.GenerateInvoicePDF(_invoice);

        data.Should().NotBeNullOrEmpty();
    }
}