namespace LostBlocks.Api.Test.AutoFixture;

public record RgbColor(byte R, byte G, byte B)
{
    public string Value()
    {
        return Convert.ToHexString([R, G, B]);
    }
}
