using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class SetControllerTest : DatabaseTest
{
    private readonly SetController controller;

    public SetControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new SetController(Context);
    }

    [Fact]
    public async Task Return_all_sets_by_theme()
    {
        // TODO Use proper test data
        LegoTheme theme = Context.Themes.First();

        var expected = Context.LegoSets
            .Where(s => s.ThemeId == theme.Id)
            .ToLookup(s => s.SetNum);

        var actual = await controller.Get(theme.Id);

        actual.Should().Contain(dto => expected.Contains(dto.SetNum));
    }

    [Fact]
    public async Task Maps_to_SetDto()
    {
        LegoTheme theme = Context.Themes.First();
        LegoSet set = Context.LegoSets.First(s => s.ThemeId == theme.Id);

        var expected = new LegoSetDto
        {
            SetNum = set.SetNum,
            Name = set.Name,
            Year = set.Year,
            NumParts = set.NumParts
        };

        var result = await controller.Get(theme.Id);

        LegoSetDto actual = result.Single(dto => dto.SetNum == set.SetNum);

        actual.Should().Be(expected);
    }
}
