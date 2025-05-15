using AutoFixture;
using LostBlocks.Models;

namespace LostBlocks.Test.AutoFixture;

internal class LegoInventoryPartCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoInventoryPart>(composer => composer
            .Without(ip => ip.InventoryId)
            .Without(ip => ip.Inventory)
            .Without(ip => ip.PartNum)
            .Without(ip => ip.Part)
            .Without(ip => ip.ColorId)
            .Without(ip => ip.Color)
        );
    }
}
