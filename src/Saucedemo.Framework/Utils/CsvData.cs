using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;

namespace Saucedemo.Framework.Utils;

public sealed class UserLoginCase
{
    public string Username { get; set; } = "";
    public string Execute { get; set; } = "";
}

public static class CsvData
{
    public static IEnumerable<UserLoginCase> LoadUsers(string path)
    {
        using var reader = new StreamReader(path);
        var cfg = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            DetectDelimiter = true,
            TrimOptions = TrimOptions.Trim
        };
        using var csv = new CsvReader(reader, cfg);
        foreach (var row in csv.GetRecords<UserLoginCase>())
        {
            yield return row;
        }
    }
}
