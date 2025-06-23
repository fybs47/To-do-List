using System.Globalization;
using CsvHelper;

namespace Application.Helpers;

public static class CsvExportHelper
{
    public static byte[] ExportToCsv<T>(IEnumerable<T> data)
    {
        using var memoryStream = new MemoryStream();
        using var streamWriter = new StreamWriter(memoryStream);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
        
        csvWriter.WriteRecords(data);
        streamWriter.Flush();
        
        return memoryStream.ToArray();
    }
}