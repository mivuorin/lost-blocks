using FluentAssertions;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Database;

public class LegoSetTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Insert(LegoSet set)
    {
        Context.LegoSets.Add(set);
        Context.SaveChanges();

        var actual = Context
            .LegoSets
            .Find(set.SetNum);

        actual.Should().NotBeNull();
    }

    [Theory, LegoAutoData]
    public void Has_single_Theme(LegoSet set, LegoTheme theme)
    {
        set.Theme = theme;

        Context.LegoSets.Add(set);
        Context.SaveChanges();

        var actual = Context
            .Themes
            .Include(t => t.Sets)
            .Single(t => t.Id == theme.Id);

        actual.Sets.Should().Contain(set);
    }
}
