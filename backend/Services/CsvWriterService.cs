using CsvHelper;
using System.Globalization;
using DataETL.Models;

namespace DataETL.Services;

public static class CsvWriterService
{
    public static void WriteRecords(string path, IEnumerable<Trip> records)
    {
        using var streamWriter = new StreamWriter(path);
        using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

        csvWriter.WriteRecords(records);
    }
}