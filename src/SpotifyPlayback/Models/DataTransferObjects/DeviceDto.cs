namespace SpotifyPlayback.Models.DataTransferObjects
{
    public class DeviceDto
    {
        public string Id { get; set; } = null!;
        public bool IsActive { get; set; }
        public bool IsRestricted { get; set; }
        public string Name { get; set; } = null!;
        public string Type { get; set; } = null!;
        public int VolumePercent { get; set; }
    }
}