using FluentAssertions;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Test.Database;

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

    [Theory]
    [LegoAutoData]
    public void Delete_clears_theme_from_related_sets(LegoTheme theme, LegoSet set)
    {
        theme.Sets.Add(set);

        Context.Themes.Add(theme);
        Context.SaveChanges();

        set.ThemeId.Should().Be(theme.Id);

        Context.Remove(theme);
        Context.SaveChanges();

        LegoSet actual = Context.Sets.Single(s => s.SetNum == set.SetNum);
        actual.ThemeId.Should().BeNull();
    }

    [Theory]
    [LegoAutoData]
    public void Delete_clears_parent_from_child(LegoTheme parent, LegoTheme child)
    {
        parent.Childs.Add(child);

        Context.Themes.Add(parent);
        Context.SaveChanges();

        child.ParentId.Should().Be(parent.Id);

        Context.Themes.Remove(parent);
        Context.SaveChanges();

        LegoTheme actual = Context.Themes.Single(t => t.Id == child.Id);
        actual.ParentId.Should().BeNull();
    }
}
