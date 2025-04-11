using FluentAssertions;
using LostBlocks.Api.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoThemeTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Fact]
    public void Insert_with_generated_id()
    {
        var theme = new LegoTheme
        {
            ParentId = null,
            Name = "Test Theme"
        };

        Context.Themes.Add(theme);
        Context.SaveChanges();

        theme.Id.Should().NotBe(0);

        var actual = Context.Themes.Find(theme.Id);
        actual.Should().NotBe(null);
    }

    [Fact]
    public void Theme_has_many_child_themes()
    {
        LegoTheme child = fixture.Context
            .Themes
            .Include(t => t.Parent)
            .First(t => t.ParentId != null);

        LegoTheme parent = fixture.Context
            .Themes
            .Include(t => t.Childs)
            .Single(t => t.Id == child.ParentId);

        Assert.Contains(child, parent.Childs);
        Assert.Equal(child.Parent, parent);
    }

    [Fact]
    public void Theme_has_many_sets()
    {
        LegoSet set = fixture.Context
            .LegoSets
            .Include(s => s.Theme)
            .First();

        LegoTheme theme = fixture.Context
            .Themes
            .Include(t => t.Sets)
            .Single(t => t.Id == set.ThemeId);

        Assert.Equal(theme, set.Theme);
        Assert.Contains(set, theme.Sets);
    }
}
