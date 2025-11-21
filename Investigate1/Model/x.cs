namespace Investigate1.Model;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Color
{
    Green,
    Yellow
}

public class ColorRequest
{
    public Color? Here { get; set; }
}

public class ColorResponse
{
    public required Color There { get; init; }
}