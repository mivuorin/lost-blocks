using FluentAssertions;
using LostBlocks.Api.Inventory;
using LostBlocks.Api.Set;
using LostBlocks.Models;
using LostBlocks.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Test.Api.Set;

public class SetDetailsControllerTest : DatabaseTest
{
    private readonly SetDetailsController controller;

    public SetDetailsControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new SetDetailsController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_by_SetNum(LegoSet set, LegoInventory inventory1, LegoInventory inventory2)
    {
        set.Inventories.Add(inventory1);
        set.Inventories.Add(inventory2);

        Context.Sets.Add(set);
        Context.SaveChanges();

        var result = await controller.Get(set.SetNum);

        var expected = new SetDetailsDto
        {
            Name = set.Name,
            Year = set.Year,
            NumParts = set.NumParts,
            Inventories =
            [
                new LegoInventoryDto
                {
                    Id = inventory1.Id,
                    Version = inventory1.Version
                },
                new LegoInventoryDto
                {
                    Id = inventory2.Id,
                    Version = inventory2.Version
                }
            ]
        };

        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_by_SetNum_404()
    {
        var result = await controller.Get("does-not-exist");
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_saves_new_set(LegoTheme theme, string name, string setNum, int year)
    {
        Context.Themes.Add(theme);
        Context.SaveChanges();

        var setDto = new CreateSetDto
        {
            SetNum = setNum,
            Name = name,
            Year = year,
            ThemeId = theme.Id
        };

        ActionResult result = await controller.Post(setDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<string>();
        createdResult.ActionName.Should().Be(nameof(SetDetailsController.Get));

        var actualSetNum = (string)createdResult.Value;

        LegoSet actual = Context.Sets.Single(s => s.SetNum == actualSetNum);

        actual.Name.Should().Be(name);
        actual.ThemeId.Should().Be(theme.Id);
        actual.NumParts.Should().Be(0);
        actual.Year.Should().Be(year);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_updates_set(LegoSet set, LegoTheme theme, LegoTheme updated, string name, int year)
    {
        set.Theme = theme;

        Context.Sets.Add(set);
        Context.SaveChanges();

        var setDto = new UpdateSetDto
        {
            Name = name,
            Year = year,
            ThemeId = updated.Id
        };

        ActionResult result = await controller.Put(set.SetNum, setDto);

        result.Should().BeOfType<NoContentResult>();

        LegoSet actual = Context.Sets.Single(s => s.SetNum == set.SetNum);
        actual.Name.Should().Be(name);
        actual.ThemeId.Should().Be(updated.Id);
        actual.Year.Should().Be(year);
    }

    [Fact]
    public async Task Put_return_404_when_part_not_found()
    {
        var partDto = new UpdateSetDto
        {
            Name = "name",
            Year = 0,
            ThemeId = 0
        };

        ActionResult result = await controller.Put("", partDto);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_removes_set_when_it_does_belong_to_any_inventory(LegoSet set, LegoTheme theme)
    {
        set.Theme = theme;

        Context.Sets.Add(set);
        Context.SaveChanges();

        ActionResult result = await controller.Delete(set.SetNum);
        result.Should().BeOfType<NoContentResult>();

        LegoSet? actual = Context
            .Sets
            .AsNoTracking()
            .SingleOrDefault(s => s.SetNum == set.SetNum);

        actual.Should().BeNull();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_fails_when_set_is_in_inventory(LegoInventory inventory, LegoTheme theme, LegoSet set)
    {
        inventory.SetNum = set.SetNum;
        set.Theme = theme;

        var inventorySet = new LegoInventorySet
        {
            Set = set,
            Inventory = inventory,
            SetNum = set.SetNum,
            Quantity = 1
        };

        Context.InventorySets.Add(inventorySet);
        Context.SaveChanges();

        var delete = () => controller.Delete(set.SetNum);

        await delete.Should().ThrowAsync<InvalidOperationException>();

        LegoSet actual = Context
            .Sets
            .AsNoTracking()
            .Single(s => s.SetNum == set.SetNum);

        actual.Should().NotBeNull();
    }

    [Fact]
    public async Task Delete_returns_404_when_set_is_not_found()
    {
        ActionResult result = await controller.Delete("");
        result.Should().BeOfType<NotFoundResult>();
    }
}
