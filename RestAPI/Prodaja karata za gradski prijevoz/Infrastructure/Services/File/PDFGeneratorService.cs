using Application.Services.Abstractions.Interfaces.File;
using Domain.Entities.Invoices;
using Domain.Entities.Tickets;
using Domain.Enums.PaymentOption;
using Infrastructure.Classes;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using System.Globalization;

namespace Infrastructure.Services.File;

public sealed class PDFGeneratorService : IPDFGeneratorService
{
    private readonly XPdfFontOptions _options = new(PdfFontEncoding.Unicode);
    private readonly XFont _fontHeader;
    private readonly XFont _fontRegular;
    private readonly XFont _fontBold;
    private readonly XPen _dashedPen = new(XColors.Black, 1)
    {
        DashPattern = new double[] { 5, 4 }
    };
    private readonly string[] _invoiceHeaderColumns = new[] { "Karta", "Količina", "Cijena" };
    
    public PDFGeneratorService()
    {
        if (Capabilities.Build.IsCoreBuild && GlobalFontSettings.FontResolver is null)
        {
            GlobalFontSettings.FontResolver = new FailsafeFontResolver();
        }

        _fontHeader = new("Times New Roman", 30, XFontStyleEx.Bold, _options);
        _fontRegular = new("Times New Roman", 12, XFontStyleEx.Regular, _options);
        _fontBold = new("Times New Roman", 12, XFontStyleEx.Bold, _options);

    }

    public PdfDocument CreateInvoicePDFDocument(Invoice invoice)
    {
        PdfDocument pdfDocument = new();

        pdfDocument.Info.Title = $"{invoice.Id}";
        pdfDocument.Info.Subject = "Invoice";

        PdfPage page = pdfDocument.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        PdfGraphicsData pdfGraphicsData = new()
        {
            CanvasHeight = 540,
            CanvasWidth = 540,
            XOffset = 27.5,
            YOffset = 15,
            CanvasPaddingX = 25,
            CanvasPaddingY = 50
        };

        pdfGraphicsData.RowPositionY = pdfGraphicsData.CanvasContentStartingPointY;

        DrawCanvas(gfx, pdfGraphicsData);
        DrawInvoiceHeader(invoice, gfx, pdfGraphicsData);
        DrawHorizontalDashedLine(gfx, pdfGraphicsData);
        DrawInvoiceTableHeader(gfx, pdfGraphicsData);
        DrawHorizontalLine(gfx, pdfGraphicsData);
        DrawInvoiceTableBody(invoice, gfx, pdfGraphicsData);
        DrawHorizontalDashedLine(gfx, pdfGraphicsData);
        DrawInvoiceFooter(invoice, gfx, pdfGraphicsData);

        return pdfDocument;
    }

    private void DrawInvoiceFooter(Invoice invoice, XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        string invoiceTotalWithoutTaxes = invoice.TotalWithoutTax.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + "KM";
        double textWidth = gfx.MeasureString(invoiceTotalWithoutTaxes, _fontBold).Width;

        gfx.DrawString("Bez PDV:", _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentStartingPointX, pdfGraphicsData.RowPositionY);
        gfx.DrawString(invoiceTotalWithoutTaxes, _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentEndingPointX - textWidth, pdfGraphicsData.RowPositionY);

        pdfGraphicsData.RowPositionY += 20;

        string invoiceTotalWithTaxes = invoice.Total.ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + "KM";
        textWidth = gfx.MeasureString(invoiceTotalWithTaxes, _fontBold).Width;

        gfx.DrawString("Total:", _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentStartingPointX, pdfGraphicsData.RowPositionY);
        gfx.DrawString(invoiceTotalWithTaxes, _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentEndingPointX - textWidth, pdfGraphicsData.RowPositionY);

        pdfGraphicsData.RowPositionY += 30;

        DrawHorizontalDashedLine(gfx, pdfGraphicsData);

        textWidth = gfx.MeasureString(PaymentOptions.GetPaymentOptionName(invoice.PaymentOption), _fontBold).Width;
        gfx.DrawString("Način plaćanja:", _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentStartingPointX, pdfGraphicsData.RowPositionY);
        gfx.DrawString(PaymentOptions.GetPaymentOptionName(invoice.PaymentOption), _fontBold, XBrushes.Black, pdfGraphicsData.CanvasContentEndingPointX - textWidth, pdfGraphicsData.RowPositionY);
    }

    private void DrawInvoiceTableBody(Invoice invoice, XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        IEnumerable<IssuedTicket> issuedTickets = invoice.IssuedTickets.DistinctBy(issuedTicket => issuedTicket.TicketId)
            .Select(issuedTicket => new IssuedTicket()
            {
                Id = issuedTicket.Id,
                TicketId = issuedTicket.TicketId,
                ValidFrom = issuedTicket.ValidFrom,
                ValidTo = issuedTicket.ValidTo,
                Ticket = issuedTicket.Ticket,
                Amount = invoice.IssuedTickets.Where(x => x.TicketId == issuedTicket.TicketId).Sum(x => x.Amount),
                User = issuedTicket.User, 
                UserId = issuedTicket.UserId,
                IssuedDate = issuedTicket.IssuedDate
            })
            .ToArray();

        foreach (IssuedTicket issuedTicket in issuedTickets)
        {
            string[] columnsContent = new[] 
            { 
                issuedTicket.Ticket.Name, 
                issuedTicket.Amount.ToString(), 
                Math.Round(issuedTicket.Ticket.Price * issuedTicket.Amount, 2).ToString("F2", CultureInfo.GetCultureInfo("de-DE")) + "KM" 
            };

            double headerColumnXPosition = pdfGraphicsData.CanvasContentWidth / (columnsContent.Length - 1);

            for (int i = 0; i < columnsContent.Length; ++i)
            {
                string columnContent = columnsContent[i];
                double centerOffset = i == 1 ? gfx.MeasureString(columnsContent[1], _fontRegular).Width / 2 : 0;
                double endOffset = i == 2 ? gfx.MeasureString(columnsContent[2], _fontRegular).Width : 0;
                double columnContentPositionX = pdfGraphicsData.CanvasContentStartingPointX + (i * headerColumnXPosition) - endOffset - centerOffset;

                gfx.DrawString(columnContent, _fontRegular, XBrushes.Black, columnContentPositionX, pdfGraphicsData.RowPositionY);
            }

            pdfGraphicsData.RowPositionY += 40;
        }
    }

    private static void DrawHorizontalLine(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        gfx.DrawLine(XPens.Black, pdfGraphicsData.CanvasContentStartingPointX, pdfGraphicsData.RowPositionY, pdfGraphicsData.CanvasContentEndingPointX, pdfGraphicsData.RowPositionY);
        pdfGraphicsData.RowPositionY += 40;
    }

    private void DrawInvoiceTableHeader(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        double headerColumnXPosition = pdfGraphicsData.CanvasContentWidth / (_invoiceHeaderColumns.Length - 1);

        for (int i = 0; i < _invoiceHeaderColumns.Length; ++i)
        {
            string headerColumnTitle = _invoiceHeaderColumns[i];
            double centerOffset = i == 1 ? gfx.MeasureString(_invoiceHeaderColumns[1], _fontBold).Width / 2 : 0;
            double endOffset = i == 2 ? gfx.MeasureString(_invoiceHeaderColumns[2], _fontBold).Width: 0;
            double columnPositionX = pdfGraphicsData.CanvasContentStartingPointX + (i * headerColumnXPosition) - centerOffset - endOffset;

            gfx.DrawString(headerColumnTitle, _fontBold, XBrushes.Black, columnPositionX, pdfGraphicsData.RowPositionY);
            
            if (i == 1)
            {
                XSize textSize = gfx.MeasureString(headerColumnTitle, _fontBold);
                gfx.DrawLine(XPens.Black, columnPositionX - 50, pdfGraphicsData.RowPositionY + 5, columnPositionX - 50, pdfGraphicsData.RowPositionY - Math.Ceiling(textSize.Height / 2) - 5);
                gfx.DrawLine(XPens.Black, columnPositionX + 50 + textSize.Width, pdfGraphicsData.RowPositionY + 5, columnPositionX + 50 + textSize.Width, pdfGraphicsData.RowPositionY - Math.Ceiling(textSize.Height / 2) - 5);
            }
        }

        pdfGraphicsData.RowPositionY += 30;
    }

    private void DrawHorizontalDashedLine(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        gfx.DrawLine(_dashedPen, pdfGraphicsData.CanvasContentStartingPointX, pdfGraphicsData.RowPositionY, pdfGraphicsData.CanvasContentEndingPointX, pdfGraphicsData.RowPositionY);
        pdfGraphicsData.RowPositionY += 40;
    }

    private void DrawInvoiceHeader(Invoice invoice, XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        string headerTitle = "Račun";
        double headerXPosition = pdfGraphicsData.CanvasContentEndingPointX - gfx.MeasureString(headerTitle, _fontHeader).Width;

        gfx.DrawString(headerTitle, _fontHeader, XBrushes.Black, headerXPosition, pdfGraphicsData.RowPositionY);

        XSize invoiceIdHeaderSize = gfx.MeasureString(invoice.Id.ToString(), _fontRegular);

        pdfGraphicsData.RowPositionY += invoiceIdHeaderSize.Height;

        double invoiceNumberXPosition = pdfGraphicsData.CanvasContentEndingPointX - gfx.MeasureString(invoice.Id.ToString(), _fontRegular).Width;

        gfx.DrawString(invoice.Id.ToString(), _fontRegular, XBrushes.Black, invoiceNumberXPosition, pdfGraphicsData.RowPositionY);

        pdfGraphicsData.RowPositionY += 40;
    }

    private static void DrawCanvas(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        gfx.DrawRectangle(XPens.LightGray, pdfGraphicsData.XOffset, pdfGraphicsData.YOffset, pdfGraphicsData.CanvasWidth, pdfGraphicsData.CanvasHeight);
    }

    public PdfDocument CreateIssuedTicketsPDFDocument(Invoice invoice)
    {
        PdfDocument pdfDocument = new();

        pdfDocument.Info.Title = $"Issued tickets_{invoice.User.FirstName + " " + invoice.User.LastName}";
        pdfDocument.Info.Subject = "Issued tickets";
        
        foreach (IssuedTicket issuedTicket in invoice.IssuedTickets)
        {
            PdfGraphicsData pdfGraphicsData = new()
            {
                CanvasHeight = 350,
                CanvasWidth = 540,
                XOffset = 27.5,
                YOffset = 15,
                CanvasPaddingX = 25,
                CanvasPaddingY = 50
            };
            PdfPage page = pdfDocument.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);

            DrawCanvas(gfx, pdfGraphicsData);
            DrawIssuedTicketHeader(gfx, pdfGraphicsData);
            DrawSidebar(gfx, pdfGraphicsData, issuedTicket);
            DrawFirstColumn(gfx, issuedTicket, pdfGraphicsData);
            DrawSecondColumn(gfx, issuedTicket, pdfGraphicsData);
            DrawFooter(gfx, issuedTicket, pdfGraphicsData, invoice);
        }

        return pdfDocument;
    }

    private void DrawFooter(XGraphics gfx, IssuedTicket issuedTicket, PdfGraphicsData pdfGraphicsData, Invoice invoice)
    {
        string buyerText = "Kupac:";
        string invoiceNumberText = "Broj računa:";
        string dateOfPurchaseText = "Datum kupovine:";

        double footerYPosition = pdfGraphicsData.CanvasContentEndingPointY - 25;
        double footerXPosition = pdfGraphicsData.CanvasContentStartingPointX + 40;
        double xOffset = 200;

        gfx.DrawLine(_dashedPen, footerXPosition, footerYPosition, pdfGraphicsData.CanvasContentEndingPointX, footerYPosition);

        footerYPosition += 15;

        gfx.DrawString(buyerText, _fontBold, XBrushes.Black, footerXPosition + 5, footerYPosition);
        gfx.DrawString(invoiceNumberText, _fontBold, XBrushes.Black, footerXPosition + xOffset, footerYPosition);

        footerYPosition += 15;

        gfx.DrawString(issuedTicket.User.FirstName + " " + issuedTicket.User.LastName, _fontRegular, XBrushes.Black, footerXPosition + 5, footerYPosition);
        gfx.DrawString(invoice.Id.ToString(), _fontRegular, XBrushes.Black, footerXPosition + xOffset, footerYPosition);

        footerYPosition += 20;

        gfx.DrawString(dateOfPurchaseText, _fontBold, XBrushes.Black, footerXPosition + 5, footerYPosition);
        footerYPosition += 15;

        gfx.DrawString(invoice.InvoicingDate.ToString("dd.MM.yyyy"), _fontRegular, XBrushes.Black, footerXPosition + 5, footerYPosition);
    }

    private void DrawSecondColumn(XGraphics gfx, IssuedTicket issuedTicket, PdfGraphicsData pdfGraphicsData)
    {
        string timeOfArivalText = "Vrijeme dolaska:";
        string destinationText = "Cilj:";
        string dateText = "Datum:";

        double columnXPosition = pdfGraphicsData.CanvasContentStartingPointX + 300;
        double rowIncrement = 30;
        double headerRowIncrement = 15;
        double initalSecondColumnYPosition = pdfGraphicsData.RowPositionY + pdfGraphicsData.CanvasContentStartingPointY + rowIncrement + headerRowIncrement;

        gfx.DrawString(destinationText, _fontBold, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
        initalSecondColumnYPosition += headerRowIncrement;
        gfx.DrawString("Otoka", _fontRegular, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
        initalSecondColumnYPosition += rowIncrement;

        gfx.DrawString(dateText, _fontBold, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
        initalSecondColumnYPosition += headerRowIncrement;
        gfx.DrawString(issuedTicket.ValidFrom.ToString("dd.MM.yyyy"), _fontRegular, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
        initalSecondColumnYPosition += rowIncrement;

        gfx.DrawString(timeOfArivalText, _fontBold, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
        initalSecondColumnYPosition += headerRowIncrement;
        gfx.DrawString("15:30", _fontRegular, XBrushes.Black, columnXPosition, initalSecondColumnYPosition);
    }

    private void DrawFirstColumn(XGraphics gfx, IssuedTicket issuedTicket, PdfGraphicsData pdfGraphicsData)
    {
        string cardTypeText = "Tip karte:";
        string startText = "Start:";
        string dateText = "Datum:";
        string timeOfDepartureText = "Vrijeme polaska:";

        double columnXPosition = pdfGraphicsData.CanvasContentStartingPointX + 100;
        double initalColumnYPosition = pdfGraphicsData.RowPositionY + pdfGraphicsData.CanvasContentStartingPointY;
        double rowIncrement = 30;
        double headerRowIncrement = 15;

        gfx.DrawString(cardTypeText, _fontBold, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += headerRowIncrement;
        gfx.DrawString(issuedTicket.Ticket.Name, _fontRegular, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += rowIncrement;

        gfx.DrawString(startText, _fontBold, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += headerRowIncrement;
        gfx.DrawString("Otoka", _fontRegular, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += rowIncrement;

        gfx.DrawString(dateText, _fontBold, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += headerRowIncrement;
        gfx.DrawString(issuedTicket.ValidFrom.ToString("dd.MM.yyyy"), _fontRegular, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += rowIncrement;

        gfx.DrawString(timeOfDepartureText, _fontBold, XBrushes.Black, columnXPosition, initalColumnYPosition);
        initalColumnYPosition += headerRowIncrement;
        gfx.DrawString("15:00", _fontRegular, XBrushes.Black, columnXPosition, initalColumnYPosition);
    }

    private static void DrawSidebar(XGraphics gfx, PdfGraphicsData pdfGraphicsData, IssuedTicket issuedTicket)
    {
        double centerOffset = 20;

        XFont sidebarFont = new("Times New Roman", 9, XFontStyleEx.Bold);

        gfx.RotateTransform(90);
        gfx.DrawString(issuedTicket.Id.ToString(), sidebarFont, XBrushes.Black, 
            pdfGraphicsData.CanvasContentStartingPointY + pdfGraphicsData.RowPositionY + centerOffset, 
            -pdfGraphicsData.CanvasContentStartingPointX - 10);
        gfx.RotateTransform(-90);

        gfx.DrawLine(XPens.Black, pdfGraphicsData.CanvasContentStartingPointX + 30, 
            pdfGraphicsData.RowPositionY + pdfGraphicsData.CanvasContentStartingPointY, 
            pdfGraphicsData.CanvasContentStartingPointX + 30, pdfGraphicsData.CanvasContentEndingPointY - 30);

        pdfGraphicsData.RowPositionY += 40;
    }

    private void DrawIssuedTicketHeader(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        string title = "Vozna karta";
        double titleXPosition = pdfGraphicsData.CanvasContentStartingPointX + 100;
        gfx.DrawString(title, _fontHeader, XBrushes.Black, titleXPosition, pdfGraphicsData.CanvasContentStartingPointY);

        pdfGraphicsData.RowPositionY += 5;
    }

    public PdfDocument CreatePurchaseHistoryPDFDocument(IEnumerable<IssuedTicket> issuedTickets)
    {
        PdfDocument pdfDocument = new();

        pdfDocument.Info.Title = $"Historija";
        pdfDocument.Info.Subject = "Purchase history";

        PdfPage page = pdfDocument.AddPage();
        XGraphics gfx = XGraphics.FromPdfPage(page);
        XSize pageSize = PageSizeConverter.ToSize(PageSize.A4);

        PdfGraphicsData pdfGraphicsData = new()
        {
            CanvasHeight = pageSize.Height,
            CanvasWidth = pageSize.Width,
            XOffset = 27.5,
            YOffset = 15,
            CanvasPaddingX = 0,
            CanvasPaddingY = 50
        };

        DrawPurchaseHistoryHeader(gfx, pdfGraphicsData);

        string[] contentArray = new[] { "Tip karte", "Relacija", "Cijena", "Datum" };
        DrawTableEntry(gfx, pdfGraphicsData, contentArray, true);

        int counter = 1;
        foreach (IssuedTicket issuedTicket in issuedTickets)
        {
            string[] contents = new[] { issuedTicket.Ticket.Name, "neka relacija", issuedTicket.Ticket.Price.ToString("F2", CultureInfo.GetCultureInfo("de-DE")), issuedTicket.IssuedDate.ToString("dd.MM.yyyy") };
            DrawTableEntry(gfx, pdfGraphicsData, contents);
            
            ++counter;

            if (counter >= 14)
            {
                counter = 0;
                AddNewPage(pdfDocument, out page, out gfx, out pageSize, out pdfGraphicsData);
            }
        }

        return pdfDocument;
    }

    private static void AddNewPage(PdfDocument pdfDocument, out PdfPage page, out XGraphics gfx, out XSize pageSize, out PdfGraphicsData pdfGraphicsData)
    {
        page = pdfDocument.AddPage();
        gfx = XGraphics.FromPdfPage(page);
        pageSize = PageSizeConverter.ToSize(PageSize.A4);

        pdfGraphicsData = new()
        {
            CanvasHeight = pageSize.Height,
            CanvasWidth = pageSize.Width,
            XOffset = 27.5,
            YOffset = 15,
            CanvasPaddingX = 0,
            CanvasPaddingY = 50
        };
    }

    private static void DrawTableEntry(XGraphics gfx, PdfGraphicsData pdfGraphicsData, string[] contents, bool isHeader = false)
    {
        double rectangleHeight = 50;
        double rectangleWidth = pdfGraphicsData.CanvasContentWidth - pdfGraphicsData.XOffset * 2;
        double contentRowY = pdfGraphicsData.CanvasContentStartingPointY + pdfGraphicsData.RowPositionY + 30;

        XFont font = new("Times New Roman", 15, XFontStyleEx.Regular);

        if (isHeader)
        {
            font = new("Times New Roman", 15, XFontStyleEx.Bold);
        }

        gfx.DrawRectangle(XPens.Black,
            pdfGraphicsData.CanvasContentStartingPointX,
            pdfGraphicsData.CanvasContentStartingPointY + pdfGraphicsData.RowPositionY,
            rectangleWidth,
            rectangleHeight);

        for (int i = 0; i < contents.Length; i++)
        {
            int xOffset = i > 0 ? -10 : 10;
            double contentX = pdfGraphicsData.CanvasContentStartingPointX + (rectangleWidth / (contents.Length - 1)) * i + xOffset;

            if (i == contents.Length - 1)
            {
                double lastElementWidth = gfx.MeasureString(contents[3], font).Width;
                gfx.DrawString(contents[i], font, XBrushes.Black, contentX - lastElementWidth, contentRowY);

                break;
            }

            gfx.DrawString(contents[i], font, XBrushes.Black, contentX, contentRowY);
        }

        pdfGraphicsData.RowPositionY += 50;
    }

    private void DrawPurchaseHistoryHeader(XGraphics gfx, PdfGraphicsData pdfGraphicsData)
    {
        string title = "Historija kupovina";
        XSize titleSize = gfx.MeasureString(title, _fontHeader);

        gfx.DrawString(title, _fontHeader, XBrushes.Black, pdfGraphicsData.CanvasContentStartingPointX + titleSize.Width / 2, pdfGraphicsData.CanvasContentStartingPointY);

        pdfGraphicsData.RowPositionY += 15;
    }
}