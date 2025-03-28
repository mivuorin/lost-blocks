using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test;

[Collection("Database")]
public class ThemeControllerTest(DatabaseFixture fixture)
{
    private readonly ThemeController controller = new(fixture.Context);

    [Fact]
    public async Task Get_returns_all_themes()
    {
        var themes = await controller.Get();

        Assert.NotEmpty(themes);
    }

    [Fact]
    public async Task Get_maps_to_theme()
    {
        LegoTheme expected = fixture.Context.Themes.First();

        var themes = await controller.Get();

        ThemeDto actual = themes.First();

        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Fact]
    public async Task Get_should_count_sets_in_theme()
    {
        // TODO Set count should be total of childs
        
        LegoTheme expectedTheme = fixture.Context.Themes.First(t => t.ParentId == null);
        var expectedCount = fixture.Context.LegoSets.Count(s => s.ThemeId == expectedTheme.Id);

        var themes = await controller.Get();

        ThemeDto actual = themes.Single(t => t.Id == expectedTheme.Id);

        Assert.Equal(expectedTheme.Name, actual.Name);
        Assert.Equal(expectedCount, actual.Sets);
    }

    [Fact]
    public async Task Get_should_build_theme_hierarchy()
    {
        // TODO Test is easier to implement when test data set is know.
        
        LegoTheme root = fixture.Context.Themes
            .Include(t => t.Childs)
            .First(t => t.Parent == null && t.Childs.Any());
        
        var themes = await controller.Get();

        ThemeDto actual = themes.Single(t => t.Id == root.Id);

        Assert.Equal(actual.Id, root.Id);
        Assert.Equal(root.Childs.Count, actual.Themes.Length);
    }
}
