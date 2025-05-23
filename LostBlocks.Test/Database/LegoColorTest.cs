﻿using FluentAssertions;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Xunit;

namespace LostBlocks.Test.Database;

public class LegoColorTest(DatabaseFixture fixture) : DatabaseTest(fixture)
{
    [Theory]
    [LegoAutoData]
    public void Rgb_is_generated_properly(LegoColor color)
    {
        color.Rgb.Should().HaveLength(6);

        var bytes = color.Rgb.Chunk(2);
        bytes
            .Should()
            .HaveCount(3)
            .And.AllSatisfy(b =>
                string.Concat(b).Should().MatchRegex("^[0-9A-F]{2}$"));
    }

    [Theory]
    [LegoAutoData]
    public void Insert_with_generated_id(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        color.Id.Should().NotBe(0);

        LegoColor? actual = Context.Colors.Find(color.Id);
        actual.Should().NotBe(null);
    }

    [Theory]
    [LegoAutoData]
    public void IsTransparent_is_mapped_correctly(LegoColor transparent, LegoColor opaque)
    {
        opaque.IsTransparent = false;
        transparent.IsTransparent = true;

        Context.Colors.AddRange(transparent, opaque);
        Context.SaveChanges();

        LegoColor actualOpaque = Context.Colors.Single(c => c.Id == opaque.Id && c.IsTransparent == false);
        actualOpaque.IsTransparent.Should().BeFalse();

        LegoColor actualTransparent = Context.Colors.Single(c => c.Id == transparent.Id && c.IsTransparent == true);
        actualTransparent.IsTransparent.Should().BeTrue();
    }

    [Theory]
    [LegoAutoData]
    public void Delete(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        Context.Colors.Remove(color);
        Context.SaveChanges();

        LegoColor? actual = Context.Colors.Find(color.Id);

        actual.Should().BeNull();
    }

    [Theory]
    [LegoAutoData]
    public void Delete_should_not_cascade(LegoColor color, LegoSet set, LegoInventory inventory,
        LegoInventoryPart inventoryPart,
        LegoPart part)
    {
        inventory.Set = set;

        inventoryPart.Inventory = inventory;
        inventoryPart.Color = color;
        inventoryPart.Part = part;

        color.InventoryParts.Add(inventoryPart);
        Context.Colors.Add(color);
        Context.SaveChanges();

        var act = () => Context.Remove(color);
        act.Should().Throw<InvalidOperationException>();
    }
}
