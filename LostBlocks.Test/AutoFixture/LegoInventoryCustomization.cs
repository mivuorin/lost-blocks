using AutoFixture;
using LostBlocks.Models;

namespace LostBlocks.Test.AutoFixture;

internal class LegoInventoryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoInventory>(composer => composer
            .Without(i => i.Id)
            .Without(i => i.InventoryParts)
            .Without(i => i.SetNum)
            .Without(i => i.Set)
            .Without(i => i.InventorySets)
        );
    }
}
