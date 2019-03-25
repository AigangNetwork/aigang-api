namespace Aigang.Platform.Contracts.Insurance.Android
{
    public class PairDeviceRequest : BaseRequest
    {
        public string ProductAddress { get; set; }
        
        public int ProductTypeId { get; set; }
        
        public string DeviceId { get; set; }
    }
}