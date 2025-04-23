using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

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
