namespace Application.DataClasses.Report;

public abstract class ReportData<T>
{
    public List<T> Data { get; set; }
}
