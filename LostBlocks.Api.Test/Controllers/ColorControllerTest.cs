using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class ColorControllerTest : DatabaseTest
{
    private readonly ColorController controller;

    public ColorControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new ColorController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_returns_all_colors(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        var result = await controller.Get();

        var expected = new ColorDto
        {
            Id = color.Id,
            Name = color.Name,
            Rgb = color.Rgb,
            IsTransparent = color.IsTransparent
        };

        ColorDto actual = result.Single(c => c.Id == color.Id);
        actual.Should().BeEquivalentTo(expected);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_by_id_returns_color_by_id(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        var result = await controller.GetById(color.Id);

        var expected = new ColorDto
        {
            Id = color.Id,
            Name = color.Name,
            Rgb = color.Rgb,
            IsTransparent = color.IsTransparent
        };

        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_by_id_returns_404_when_color_is_not_found()
    {
        var result = await controller.GetById(int.MinValue);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_saves_new_color(string name, RgbColor rgb, bool isTransparent)
    {
        var colorDto = new CreateColorDto
        {
            Name = name,
            Rgb = rgb.Value(),
            IsTransparent = isTransparent
        };

        ActionResult result = await controller.Post(colorDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<int>();
        createdResult.ActionName.Should().Be(nameof(ColorController.GetById));

        var actualId = (int)createdResult.Value;

        LegoColor actual = Context.Colors.Single(c => c.Id == actualId);

        actual.Name.Should().Be(name);
        actual.IsTransparent.Should().Be(isTransparent);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_updates_color(LegoColor color, LegoColor expected)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        var colorDto = new UpdateColorDto
        {
            Name = expected.Name,
            Rgb = expected.Rgb,
            IsTransparent = expected.IsTransparent
        };

        ActionResult result = await controller.Put(color.Id, colorDto);

        result.Should().BeOfType<NoContentResult>();

        LegoColor actual = Context.Colors.Single(c => c.Id == color.Id);

        expected.Id = color.Id;
        actual.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Put_return_404_when_color_not_found()
    {
        var colorDto = new UpdateColorDto
        {
            Name = "name",
            Rgb = "000000",
            IsTransparent = false
        };

        ActionResult result = await controller.Put(int.MinValue, colorDto);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_deletes_color(LegoColor color)
    {
        Context.Colors.Add(color);
        Context.SaveChanges();

        IActionResult result = await controller.Delete(color.Id);

        // ExecuteDelete does not update change tracker, so deleted entity is still  tracked.
        Context.ChangeTracker.Clear();

        result.Should().BeOfType<NoContentResult>();

        LegoColor? actual = Context.Colors.Find(color.Id);
        actual.Should().BeNull();
    }
}
