using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoThemeTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert_with_generated_id(LegoTheme theme)
    {
        Context.Themes.Add(theme);
        Context.SaveChanges();

        theme.Id.Should().NotBe(0);

        LegoTheme? actual = Context.Themes.Find(theme.Id);
        actual.Should().NotBe(null);
    }

    [Theory]
    [LegoAutoData]
    public void Theme_has_one_parent_Theme(LegoTheme child, LegoTheme parent)
    {
        child.Parent = parent;

        Context.Themes.Add(child);
        Context.SaveChanges();

        LegoTheme actual = Context
            .Themes
            .Include(t => t.Parent)
            .Single(t => t.Id == child.Id);

        actual.Parent.Should().Be(parent);
    }

    [Theory]
    [LegoAutoData]
    public void Theme_has_many_child_Themes(LegoTheme child, LegoTheme parent)
    {
        parent.Childs.Add(child);

        Context.Themes.Add(parent);
        Context.SaveChanges();

        LegoTheme actual = Context
            .Themes
            .Include(t => t.Childs)
            .Single(t => t.Id == parent.Id);

        actual.Childs.Should().Contain(child);
    }

    [Fact]
    public void Theme_has_many_sets()
    {
        LegoSet set = Context
            .Sets
            .Include(s => s.Theme)
            .First();

        LegoTheme theme = Context
            .Themes
            .Include(t => t.Sets)
            .Single(t => t.Id == set.ThemeId);

        Assert.Equal(theme, set.Theme);
        Assert.Contains(set, theme.Sets);
    }
}
