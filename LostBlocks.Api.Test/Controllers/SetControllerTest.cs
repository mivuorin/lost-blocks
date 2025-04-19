using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class SetControllerTest : DatabaseTest
{
    private readonly SetController controller;

    public SetControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new SetController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Query_by_theme(LegoTheme theme, LegoSet set, LegoSet other)
    {
        set.Theme = theme;
        other.Theme = null;

        Context.Sets.AddRange(set, other);
        Context.SaveChanges();

        var expected = Context.Sets
            .Where(s => s.ThemeId == theme.Id)
            .ToLookup(s => s.SetNum);

        var actual = await controller.Query(theme.Id);

        actual.Should().HaveCount(1)
            .And.ContainSingle(s => s.SetNum == set.SetNum)
            .And.NotContain(s => s.SetNum == other.SetNum);
    }

    [Theory]
    [LegoAutoData]
    public async Task Query_maps_to_SetDto(LegoTheme theme, LegoSet set)
    {
        set.Theme = theme;

        Context.Sets.Add(set);
        Context.SaveChanges();

        var result = await controller.Query(theme.Id);

        LegoSetDto actual = result.Single(dto => dto.SetNum == set.SetNum);

        var expected = new LegoSetDto
        {
            SetNum = set.SetNum,
            Name = set.Name,
            Year = set.Year,
            NumParts = set.NumParts
        };

        actual.Should().Be(expected);
    }
}
