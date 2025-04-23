using AutoFixture;
using LostBlocks.Api.Models;

namespace LostBlocks.Api.Test.AutoFixture;

internal class LegoThemeCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<LegoTheme>(composer => composer
            .Without(t => t.Id)
            .Without(t => t.ParentId)
            .Without(t => t.Parent)
            .Without(t => t.Childs)
            .Without(t => t.Sets)
        );
    }
}
