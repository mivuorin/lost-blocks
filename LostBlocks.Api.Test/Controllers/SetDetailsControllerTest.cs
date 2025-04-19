using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class SetDetailsControllerTest : DatabaseTest
{
    private readonly SetDetailsController controller;

    public SetDetailsControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new SetDetailsController(Context);
    }

    [Theory, LegoAutoData]
    public async Task Get_by_SetNum(LegoSet set)
    {
        Context.Sets.Add(set);
        Context.SaveChanges();

        var result = await controller.Get(set.SetNum);

        var expected = new LegoSetDetailsDto
        {
            Name = set.Name,
            Year = set.Year,
            NumParts = set.NumParts
        };

        result.Value.Should().Be(expected);
    }

    [Fact]
    public async Task Get_by_SetNum_404()
    {
        var result = await controller.Get("does-not-exist");
        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
