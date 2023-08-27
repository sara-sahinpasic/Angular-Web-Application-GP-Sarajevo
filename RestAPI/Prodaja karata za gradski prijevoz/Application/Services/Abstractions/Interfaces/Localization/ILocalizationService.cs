namespace Application.Services.Abstractions.Interfaces.Localization;

public interface ILocalizationService
{
    public string Localize(string? locale, string message);
}
