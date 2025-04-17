using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

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
