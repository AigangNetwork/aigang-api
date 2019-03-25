using Aigang.Platform.Domain.DeviceData;

namespace Aigang.AndroidDataCollector.Client.Responses
{
    public class MobileDataResponse : BaseDataCollectorResponse
    {
        public string DeviceId { get; set; }
        public WearLevel WearLevel { get; set; }
        public string BatteryDesignCapacity { get; set; }
        public int ChargeLevel { get; set; }
        public int AgeInMonths { get; set; }
        public string Region { get; set; }
        public string Brand { get; set; }
    }
}