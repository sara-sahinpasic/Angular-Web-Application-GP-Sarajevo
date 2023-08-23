namespace Infrastructure.Classes;

internal sealed class PdfGraphicsData
{
    public double RowPositionY { get; set; }
    public double XOffset { get; set; }
    public double YOffset { get; set; }
    public double CanvasPaddingX { get; set; }
    public double CanvasPaddingY { get; set; }
    public double CanvasWidth { get; set; }
    public double CanvasHeight { get; set; }
    public double CanvasContentWidth 
    {
        get => CanvasWidth - (CanvasPaddingX * 2); 
    }
    public double CanvasContentHeight
    {
        get => CanvasHeight - (CanvasPaddingY * 2);
    }
    public double CanvasContentStartingPointX 
    {
        get => XOffset + CanvasPaddingX;
    }
    public double CanvasContentEndingPointX
    {
        get => CanvasContentStartingPointX + CanvasContentWidth;
    }
    public double CanvasContentStartingPointY
    {
        get => YOffset + CanvasPaddingY;
    }
    public double CanvasContentEndingPointY
    {
        get => CanvasContentStartingPointY + CanvasContentHeight;
    }
}
