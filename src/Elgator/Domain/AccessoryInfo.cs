using System.Text.Json.Serialization;

namespace Elgator
{
    public class AccessoryInfo
    {
        [JsonPropertyName("productName")]
        public string Product { get; set; }

        [JsonPropertyName("hardwareBoardType")]
        public int BoardType { get; set; }

        [JsonPropertyName("firmwareBuildNumber")]
        public int FirmwareBuild { get; set; }

        [JsonPropertyName("firmwareVersion")]
        public string FirmwareVersion { get; set; }

        [JsonPropertyName("serialNumber")]
        public string SerialNumber { get; set; }

        [JsonPropertyName("displayName")]
        public string Name { get; set; }

        [JsonPropertyName("features")]
        public string[] Features { get; set; }

        public AccessoryInfo()
        {
            Product = "n/a";
            FirmwareVersion = "n/a";
            SerialNumber = "n/a";
            Name = "n/a";
            Features = new string[0];
        }
    }
}