using AutoFixture;
using LostBlocks.Models;

namespace LostBlocks.Test.AutoFixture;

internal class LegoPartCategoryCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoPartCategory>(composer => composer
            .Without(p => p.Id)
            .Without(p => p.Parts)
        );
    }
}
