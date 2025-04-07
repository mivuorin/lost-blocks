using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using Xunit;

namespace LostBlocks.Api.Test;

[Collection("Database")]
public class SetControllerTest(DatabaseFixture fixture)
{
    private readonly SetController controller = new(fixture.Context);

    [Fact]
    public async Task Return_all_sets_by_theme()
    {
        // TODO Use proper test data
        LegoTheme theme = fixture.Context.Themes.First();

        var expected = fixture.Context.LegoSets
            .Where(s => s.ThemeId == theme.Id)
            .ToLookup(s => s.SetNum);

        var actual = await controller.Get(theme.Id);

        Assert.Contains(actual, dto => expected.Contains(dto.SetNum));
    }

    [Fact]
    public async Task Maps_to_SetDto()
    {
        LegoTheme theme = fixture.Context.Themes.First();

        LegoSet expected = fixture.Context.LegoSets.First(s => s.ThemeId == theme.Id);

        var result = await controller.Get(theme.Id);

        SetDto actual = result.Single(dto => dto.SetNum == expected.SetNum);

        Assert.Equal(expected.SetNum, actual.SetNum);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Year, actual.Year);
        Assert.Equal(expected.NumParts, actual.NumParts);
    }
}
