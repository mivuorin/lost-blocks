using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

internal class LegoPartCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoPart>(composer =>
            composer
                .Without(p => p.PartCatId)
                .Without(p => p.Category)
                .Without(p => p.InventoryParts)
        );
    }
}
