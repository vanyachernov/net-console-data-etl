using DataETL.Models;

namespace DataETL.Services;

public static class TripDataProcessorService
{
    public static (List<Trip> Clean, List<Trip> Duplicates) Deduplicate(IEnumerable<Trip> data)
    {
        var seen = new HashSet<(DateTime PickUp, DateTime DropOff, int PassengerCount)>();
        var clean = new List<Trip>();
        var duplicates = new List<Trip>();

        foreach (var r in data)
        {
            var key = (r.PickUp, r.DropOff, r.PassengerCount); // I use ValueType for efficient code. 
            
            if (!seen.Add(key))
            {
                duplicates.Add(r);
            }
            else
            {
                clean.Add(r);
            }
        }
        return (clean, duplicates);
    }
}