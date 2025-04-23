using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

internal class LegoInventorySetCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoInventorySet>(composer => composer
            .Without(s => s.Inventory)
            .Without(s => s.InventoryId)
            .Without(s => s.Set)
            .Without(s => s.SetNum)
        );
    }
}
