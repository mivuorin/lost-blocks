using FluentAssertions;
using LostBlocks.Api.Api.Inventory;
using LostBlocks.Api.Api.Set;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Api.Test.Controllers.Set;

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

        var expected = new LegoSetDetailsDto
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
}
