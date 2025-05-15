using FluentAssertions;
using LostBlocks.Api.Theme;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Xunit;

namespace LostBlocks.Test.Controllers.Theme;

public class ThemeControllerTest : DatabaseTest
{
    private readonly ThemeController controller;

    public ThemeControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new ThemeController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_returns_themes(LegoTheme expected)
    {
        Context.Themes.Add(expected);
        Context.SaveChanges();

        var themes = await controller.Get();

        ThemeDto actual = themes.Single(t => t.Id == expected.Id);

        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Name, actual.Name);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_return_theme_hierarchy_and_set_count_in_theme(
        LegoTheme root,
        LegoTheme middle,
        LegoTheme leaf,
        LegoSet set1,
        LegoSet set2,
        LegoSet set3
    )
    {
        root.Sets.Add(set1);
        leaf.Sets.Add(set2);
        leaf.Sets.Add(set3);

        middle.Childs.Add(leaf);
        root.Childs.Add(middle);

        Context.Themes.Add(root);
        Context.SaveChanges();

        var themes = await controller.Get();

        ThemeDto actualRoot = themes.Single(t => t.Id == root.Id);
        actualRoot.Id.Should().Be(root.Id);
        actualRoot.Sets.Should().Be(3);

        ThemeDto actualMiddle = actualRoot.Themes.Single();
        actualMiddle.Id.Should().Be(middle.Id);
        actualMiddle.Sets.Should().Be(2);

        ThemeDto actualLeaf = actualMiddle.Themes.Single();
        actualLeaf.Id.Should().Be(leaf.Id);
        actualLeaf.Sets.Should().Be(2);

        actualLeaf.Themes.Should().BeEmpty();
    }
}
