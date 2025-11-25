using CsvHelper;
using TimeZoneConverter;
using System.Globalization;
using CsvHelper.Configuration;
using DataETL.Models;

namespace DataETL.Services;

public static class CsvReaderService
{
    public static List<Trip> Read(string path)
    {
        var trips = new List<Trip>();

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            TrimOptions = TrimOptions.Trim,
            MissingFieldFound = null,
            HeaderValidated = null,
            BadDataFound = null
        };

        var est = TZConvert.GetTimeZoneInfo("Eastern Standard Time");
        using var reader = new StreamReader(path);
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();

        while (csv.Read())
        {
            try
            {
                var pickUp = csv.GetField<DateTime>("tpep_pickup_datetime");
                var dropOff = csv.GetField<DateTime>("tpep_dropoff_datetime");

                trips.Add(new Trip
                {
                    PickUp = TimeZoneInfo.ConvertTimeToUtc(pickUp, est),
                    DropOff = TimeZoneInfo.ConvertTimeToUtc(dropOff, est),
                    PassengerCount = csv.GetField<int>("passenger_count"),
                    Distance = csv.GetField<double>("trip_distance"),
                    StoreAndFwdFlag = csv.GetField<string>("store_and_fwd_flag") == "Y" ? "Yes" : "No",
                    PULocationID = csv.GetField<int>("PULocationID"),
                    DOLocationID = csv.GetField<int>("DOLocationID"),
                    FareAmount = csv.GetField<decimal>("fare_amount"),
                    TipAmount = csv.GetField<decimal>("tip_amount")
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in row {csv.Context}: {ex.Message}");
            }
        }

        return trips;
    }
}