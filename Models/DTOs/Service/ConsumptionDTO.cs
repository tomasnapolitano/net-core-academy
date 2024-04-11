namespace Models.DTOs.Service
{
    public class ConsumptionDTO
    {
        public int DaysOfConsumption { get; set; }
        public float UnitsConsumed {  get; set; }
        public float TotalCost { get; set; }
        public ServiceSubscriptionWithUserDTO ServiceSubscription { get; set; }
    }
}
