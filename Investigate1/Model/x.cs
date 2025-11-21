namespace Investigate1.Model;

public enum Katzenum
{
    green,
    yellow
}

public class OptinalKatze
{
    public  Katzenum? katze { get; set; }
    
}

public class RequiredKatze
{
    public required Katzenum Katzenum { get; init; }

}