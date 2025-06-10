using FluentAssertions;
using LostBlocks.Api.Theme;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Test.Api.Theme;

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

        actual.Id.Should().Be(expected.Id);
        actual.Name.Should().Be(expected.Name);
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

    [Theory]
    [LegoAutoData]
    public async Task GetById_returns_theme_details(LegoTheme parent, LegoTheme child, LegoSet childSet)
    {
        parent.Childs.Add(child);
        child.Sets.Add(childSet);

        Context.Themes.Add(parent);
        Context.SaveChanges();

        var result = await controller.GetById(child.Id);

        var expected = new ThemeDetailsDto
        {
            Id = child.Id,
            Name = child.Name,
            ParentId = child.ParentId
        };

        ThemeDetailsDto? actual = result.Value;
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetById_returns_404_when_theme_is_not_found()
    {
        var result = await controller.GetById(int.MinValue);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_create_new_Theme(string name)
    {
        var themeDto = new CreateThemeDto
        {
            Name = name,
            ParentId = null
        };

        ActionResult result = await controller.Post(themeDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<int>();
        createdResult.ActionName.Should().Be(nameof(ThemeController.GetById));

        var id = (int)createdResult.Value;

        LegoTheme actual = Context.Themes.Single(t => t.Id == id);
        actual.Name.Should().Be(name);
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_create_new_child_theme(LegoTheme parent, string name)
    {
        Context.Themes.Add(parent);
        Context.SaveChanges();

        var themeDto = new CreateThemeDto
        {
            Name = name,
            ParentId = parent.Id
        };

        ActionResult result = await controller.Post(themeDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<int>();

        var id = (int)createdResult.Value;

        LegoTheme actual = Context.Themes.Single(t => t.Id == id);
        actual.Name.Should().Be(name);
        actual.ParentId.Should().Be(parent.Id);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_updates_theme(LegoTheme theme, string name)
    {
        Context.Themes.Add(theme);
        Context.SaveChanges();

        var themeDto = new UpdateThemeDto
        {
            Name = name,
            ParentId = null
        };

        ActionResult result = await controller.Put(theme.Id, themeDto);
        result.Should().BeOfType<NoContentResult>();

        LegoTheme actual = Context.Themes.Single(t => t.Id == theme.Id);
        actual.Name.Should().Be(name);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_update_parent_theme(LegoTheme theme, LegoTheme parent)
    {
        Context.Themes.AddRange(theme, parent);
        Context.SaveChanges();

        var themeDto = new UpdateThemeDto
        {
            Name = theme.Name,
            ParentId = parent.Id
        };

        ActionResult result = await controller.Put(theme.Id, themeDto);
        result.Should().BeOfType<NoContentResult>();

        LegoTheme actual = Context
            .Themes.Include(legoTheme => legoTheme.Parent)
            .Single(t => t.Id == theme.Id);

        actual.Parent.Should().BeEquivalentTo(parent);
    }

    [Fact]
    public async Task Put_return_404_when_theme_not_found()
    {
        var themeDto = new UpdateThemeDto
        {
            Name = "name",
            ParentId = null
        };

        ActionResult result = await controller.Put(int.MinValue, themeDto);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_removes_theme(LegoTheme expected, LegoTheme theme)
    {
        Context.Themes.AddRange(expected, theme);
        Context.SaveChanges();

        ActionResult result = await controller.Delete(expected.Id);
        result.Should().BeOfType<NoContentResult>();

        LegoTheme? actual = Context.Themes.SingleOrDefault(t => t.Id == expected.Id);
        actual.Should().BeNull();

        LegoTheme existing = Context.Themes.Single(t => t.Id == theme.Id);
        existing.Should().NotBeNull();
    }
}
