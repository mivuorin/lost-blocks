using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

internal class LegoColorCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoColor>(composer =>
        {
            return composer
                .Without(c => c.Id)
                .With(c => c.Rgb, () =>
                {
                    var r = fixture.Create<byte>();
                    var g = fixture.Create<byte>();
                    var b = fixture.Create<byte>();

                    return Convert.ToHexString([r, g, b]);
                });
        });
    }
}
