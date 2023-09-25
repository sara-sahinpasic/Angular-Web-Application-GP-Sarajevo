using Application.Localization;
using Application.Services.Abstractions.Interfaces.Localization;

namespace Application.Services.Implementations.Localization;

public sealed class LocalizationService : ILocalizationService
{
    private readonly Messages _messages;

    public LocalizationService(Messages messages)
    {
        _messages = messages;
    }

    public string Localize(string? locale, string message)
    {
        locale = locale?.ToLower();

        if ((locale is null || locale == "null" || locale == "bs") && _messages.Bs.TryGetValue(message, out string? bsTranslation))
        {
            return bsTranslation;
        }

        if (locale == "en" && _messages.En.TryGetValue(message, out string? enTranslation))
        {
            return enTranslation;
        }

        return message;
    }
}
