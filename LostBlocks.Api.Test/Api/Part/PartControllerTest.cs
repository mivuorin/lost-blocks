using FluentAssertions;
using LostBlocks.Api.Api.Part;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Controllers.Part;

public class PartControllerTest : DatabaseTest
{
    private readonly PartController controller;

    public PartControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new PartController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_by_id_returns_part_by_id(LegoPart part)
    {
        Context.Parts.Add(part);
        Context.SaveChanges();

        var result = await controller.GetByPartNum(part.PartNum);

        var expected = new PartDto
        {
            PartNum = part.PartNum,
            Name = part.Name,
            CategoryId = part.CategoryId
        };

        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_by_id_returns_404_when_part_is_not_found()
    {
        var result = await controller.GetByPartNum("");
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_saves_new_part(string partNum, string name, LegoPartCategory category)
    {
        Context.PartCategories.Add(category);

        var create = new CreatePartDto
        {
            PartNum = partNum,
            Name = name,
            CategoryId = category.Id
        };

        ActionResult result = await controller.Post(create);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<string>();
        createdResult.ActionName.Should().Be(nameof(PartController.GetByPartNum));

        var actualPartNum = (string)createdResult.Value;

        LegoPart actual = Context.Parts.Single(p => p.PartNum == actualPartNum);

        actual.Name.Should().Be(name);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_updates_part(LegoPart part, LegoPartCategory category, LegoPartCategory expectedCategory,
        string expectedName)
    {
        Context.PartCategories.Add(expectedCategory);

        part.Category = category;
        Context.Parts.Add(part);

        Context.SaveChanges();

        var partDto = new UpdatePartDto
        {
            Name = expectedName,
            CategoryId = expectedCategory.Id
        };

        ActionResult result = await controller.Put(part.PartNum, partDto);
        result.Should().BeOfType<NoContentResult>();

        LegoPart actual = Context.Parts.Single(c => c.PartNum == part.PartNum);
        actual.Name.Should().BeEquivalentTo(expectedName);
        actual.CategoryId.Should().Be(expectedCategory.Id);
    }

    [Fact]
    public async Task Put_return_404_when_part_not_found()
    {
        var partDto = new UpdatePartDto
        {
            Name = "name",
            CategoryId = 0
        };

        ActionResult result = await controller.Put("", partDto);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_removes_category(LegoPart part, LegoPartCategory category)
    {
        part.Category = category;
        
        Context.Parts.Add(part);
        Context.SaveChanges();
    
        ActionResult result = await controller.Delete(part.PartNum);
        result.Should().BeOfType<NoContentResult>();
    
        LegoPart? actual = Context
            .Parts
            .AsNoTracking()
            .SingleOrDefault(p => p.PartNum == part.PartNum);
        
        actual.Should().BeNull();
    }
}
