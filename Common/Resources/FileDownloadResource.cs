using Newtonsoft.Json;

namespace Common.Resources;

public class FileDownloadResource
{
    [JsonProperty("version")]
    public Version Version { get; set; } = new Version();

    [JsonProperty("fileBytes")]
    public byte[] FileContent { get; set; } = Array.Empty<byte>();
}
