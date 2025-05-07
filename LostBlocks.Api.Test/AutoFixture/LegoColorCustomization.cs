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
            .With(c => c.Rgb, () => fixture.Create<RgbColor>().Value())
        );
    }
}
