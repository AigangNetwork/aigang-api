namespace Aigang.Platform.Domain.DeviceData
{
    public class MobileData
    {
        public string DeviceId { get; set; }
        
        public WearLevel WearLevel { get; set; }
        
        public int BatteryDesignCapacity { get; set; }
        
        public int ChargeLevel { get; set; }
        
        public int AgeInMonths { get; set; }
        
        public string Region { get; set; }
        
        public string Brand { get; set; }
    }
}