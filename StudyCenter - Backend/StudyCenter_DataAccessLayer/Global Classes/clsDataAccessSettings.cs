using Microsoft.Extensions.Configuration;


namespace StudyCenter_DataAccessLayer.Global_Classes;

public class clsDataAccessSettings
{
    private static IConfigurationRoot? _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    public static string? ConnectionString = _configuration.GetSection("ConnectionString").Value;

}