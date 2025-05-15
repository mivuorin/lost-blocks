using AutoFixture;
using LostBlocks.Models;

namespace LostBlocks.Test.AutoFixture;

internal class LegoPartCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoPart>(composer => composer
            .Without(p => p.CategoryId)
            .Without(p => p.Category)
            .Without(p => p.InventoryParts)
        );
    }
}
