using System.Data;
using DataETL.Models;
using DataETL.Services;
using Microsoft.Data.SqlClient;

public static class Program
{
    private const string CsvPath = "./Data/sample-cab-data.csv";
    private const string CsvDublicatesPath = "./Data/duplicates.csv";
    private const string ConnectionString = "Server=localhost,1433;Database=TestAssignmentDB;User Id=sa;Password=StrongPassw0rd1!;TrustServerCertificate=True;";
    
    public static void Main()
    {
        var raw = ReadCsv(CsvPath);
        var (clean, duplicates) = DeduplicateTrips(raw);
        var dataTable = ConvertToDataTable(clean);

        InsertToDatabase(dataTable, "dbo.Trips");
        WriteDuplicates(duplicates, CsvDublicatesPath);
    }

    private static IEnumerable<Trip> ReadCsv(string path)
    {
        return CsvReaderService.Read(path);
    }

    private static (IEnumerable<Trip> Clean, IEnumerable<Trip> Duplicates) DeduplicateTrips(IEnumerable<Trip> trips)
    {
        return TripDataProcessorService.Deduplicate(trips);
    }

    private static DataTable ConvertToDataTable(IEnumerable<Trip> trips)
    {
        return TripConverterService.ToDataTable(trips);
    }

    private static void InsertToDatabase(DataTable table, string tableName)
    {
        using var connection = new SqlConnection(ConnectionString);
        connection.Open();

        using var bulk = new SqlBulkCopy(connection);
        bulk.DestinationTableName = tableName;
        bulk.BulkCopyTimeout = 0;
        
        bulk.ColumnMappings.Add("tpep_pickup_datetime", "tpep_pickup_datetime");
        bulk.ColumnMappings.Add("tpep_dropoff_datetime", "tpep_dropoff_datetime");
        bulk.ColumnMappings.Add("passenger_count", "passenger_count");
        bulk.ColumnMappings.Add("trip_distance", "trip_distance");
        bulk.ColumnMappings.Add("store_and_fwd_flag", "store_and_fwd_flag");
        bulk.ColumnMappings.Add("PULocationID", "PULocationID");
        bulk.ColumnMappings.Add("DOLocationID", "DOLocationID");
        bulk.ColumnMappings.Add("fare_amount", "fare_amount");
        bulk.ColumnMappings.Add("tip_amount", "tip_amount");
        
        bulk.WriteToServer(table);
    }

    private static void WriteDuplicates(IEnumerable<Trip> duplicates, string path)
    {
        CsvWriterService.WriteRecords(path, duplicates);
    }
}