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
        Context.Sets.Add(set);
        Context.SaveChanges();

        LegoSet? actual = Context
            .Sets
            .Find(set.SetNum);

        actual.Should().NotBeNull();
    }

    [Theory]
    [LegoAutoData]
    public void Has_one_Theme(LegoSet set, LegoTheme theme)
    {
        set.Theme = theme;

        Context.Sets.Add(set);
        Context.SaveChanges();

        LegoTheme actual = Context
            .Themes
            .Include(t => t.Sets)
            .Single(t => t.Id == theme.Id);

        actual.Sets.Should().Contain(set);
    }

    [Theory]
    [LegoAutoData]
    public void Has_many_LegoInventory(LegoSet set, LegoInventory inventory1, LegoInventory inventory2)
    {
        set.Inventories.Add(inventory1);
        set.Inventories.Add(inventory2);

        Context.Sets.Add(set);
        Context.SaveChanges();

        LegoSet actual = Context.Sets
            .Include(s => s.Inventories)
            .Single(s => s.SetNum == set.SetNum);

        actual.Inventories.Should().HaveCount(2)
            .And.Contain(inventory1)
            .And.Contain(inventory2);
    }

    [Theory]
    [LegoAutoData]
    public void Has_many_LegoInventorySet(LegoSet parentSet, LegoInventory inventory, LegoInventorySet inventorySet,
        LegoSet child)
    {
        parentSet.Inventories.Add(inventory);

        inventorySet.Set = child;
        inventorySet.Inventory = inventory;

        parentSet.InventorySets.Add(inventorySet);

        Context.Sets.Add(parentSet);
        Context.SaveChanges();

        LegoSet actual = Context.Sets
            .Include(s => s.InventorySets)
            .Single(s => s.SetNum == parentSet.SetNum);

        actual.InventorySets.Should().HaveCount(1)
            .And.Contain(inventorySet);
    }
}
