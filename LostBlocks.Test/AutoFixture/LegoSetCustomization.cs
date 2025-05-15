using AutoFixture;
using LostBlocks.Models;

namespace LostBlocks.Test.AutoFixture;

internal class LegoSetCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoSet>(composer => composer
            .Without(s => s.Theme)
            .Without(s => s.ThemeId)
            .Without(s => s.Inventories)
            .Without(s => s.InventorySets)
        );
    }
}
