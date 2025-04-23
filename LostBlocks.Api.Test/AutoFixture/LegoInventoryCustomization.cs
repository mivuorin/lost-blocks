using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

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
