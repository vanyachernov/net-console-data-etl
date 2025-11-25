namespace DefaultNamespace;

public class Trip
{
    public DateTime PickUp { get; set; }
    public DateTime DropOff { get; set; }
    public int PassengerCount { get; set; }
    public double Distance { get; set; }
    public string StoreAndFwdFlag { get; set; }
    public int PULocationID { get; set; }
    public int DOLocationID { get; set; }
    public decimal FareAmount { get; set; }
    public decimal TipAmount { get; set; }
}