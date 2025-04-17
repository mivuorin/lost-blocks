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
    public void Has_one_Theme(LegoSet set, LegoTheme theme)
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

    [Theory, LegoAutoData]
    public void Has_many_Inventories(LegoSet set, LegoInventory inventory1, LegoInventory inventory2)
    {
        set.Inventories.Add(inventory1);
        set.Inventories.Add(inventory2);

        Context.LegoSets.Add(set);
        Context.SaveChanges();

        LegoSet actual = Context.LegoSets
            .Include(s => s.Inventories)
            .Single(s => s.SetNum == set.SetNum);

        actual.Inventories.Should().HaveCount(2)
            .And.Contain(inventory1)
            .And.Contain(inventory2);
    }
}
