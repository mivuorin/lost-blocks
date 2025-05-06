using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

internal class LegoColorCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoColor>(composer => composer
            .Without(c => c.Id)
            .Without(c => c.InventoryParts)
            .With(c => c.Rgb, () =>
                {
                    var r = fixture.Create<byte>();
                    var g = fixture.Create<byte>();
                    var b = fixture.Create<byte>();

                    return Convert.ToHexString([r, g, b]);
                }
            )
        );
    }
}
