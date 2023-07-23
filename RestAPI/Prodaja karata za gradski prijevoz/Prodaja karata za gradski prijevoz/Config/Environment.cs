namespace Prodaja_karata_za_gradski_prijevoz.Config;

public static partial class Environment
{
    public static void ConfigureEnvironment(this WebApplicationBuilder builder)
    {
        builder.Environment.ContentRootPath = "Uploads";
    }
}