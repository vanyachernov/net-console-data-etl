using System.Data;
using DataETL.Models;

namespace DataETL.Services;

public static class TripConverterService
{
    public static DataTable ToDataTable(IEnumerable<Trip> records)
    {
        var dt = new DataTable();
        
        dt.Columns.Add("tpep_pickup_datetime", typeof(DateTime));
        dt.Columns.Add("tpep_dropoff_datetime", typeof(DateTime));
        dt.Columns.Add("passenger_count", typeof(int));
        dt.Columns.Add("trip_distance", typeof(double));
        dt.Columns.Add("store_and_fwd_flag", typeof(string));
        dt.Columns.Add("PULocationID", typeof(int));
        dt.Columns.Add("DOLocationID", typeof(int));
        dt.Columns.Add("fare_amount", typeof(decimal));
        dt.Columns.Add("tip_amount", typeof(decimal));

        foreach (var r in records)
        {
            dt.Rows.Add(
                r.PickUp,
                r.DropOff,
                r.PassengerCount,
                r.Distance,
                r.StoreAndFwdFlag,
                r.PULocationID,
                r.DOLocationID,
                r.FareAmount,
                r.TipAmount
            );
        }

        return dt;
    }
}