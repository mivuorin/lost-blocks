using FluentAssertions;
using LostBlocks.Api.Controllers;
using LostBlocks.Api.Models;
using LostBlocks.Api.Test.AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace LostBlocks.Api.Test.Controllers;

public class CategoryControllerTest : DatabaseTest
{
    private readonly CategoryController controller;

    public CategoryControllerTest(DatabaseFixture fixture) : base(fixture)
    {
        controller = new CategoryController(Context);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_returns_all_categories(LegoPartCategory category1, LegoPartCategory category2)
    {
        Context.PartCategories.AddRange(category1, category2);
        Context.SaveChanges();

        var actual = await controller.Get();

        var expectedCategory1 = new CategoryDto
        {
            Id = category1.Id,
            Name = category1.Name
        };

        var expectedCategory2 = new CategoryDto
        {
            Id = category2.Id,
            Name = category2.Name
        };

        actual.Should().ContainEquivalentOf(expectedCategory1);
        actual.Should().ContainEquivalentOf(expectedCategory2);
    }

    [Theory]
    [LegoAutoData]
    public async Task Get_by_id_returns_category_by_id(LegoPartCategory category)
    {
        Context.PartCategories.Add(category);
        Context.SaveChanges();

        var result = await controller.GetById(category.Id);

        var expected = new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        };

        result.Value.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Get_by_id_returns_404_when_category_is_not_found()
    {
        var result = await controller.GetById(int.MinValue);
        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Post_creates_new_category(string name)
    {
        var categoryDto = new CreateCategoryDto
        {
            Name = name
        };

        ActionResult result = await controller.Post(categoryDto);
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdResult = (CreatedAtActionResult)result;
        createdResult.Value.Should().BeOfType<int>();
        createdResult.ActionName.Should().Be(nameof(CategoryController.GetById));

        var actualId = (int)createdResult.Value;

        LegoPartCategory actual = Context.PartCategories.Single(c => c.Id == actualId);

        actual.Name.Should().Be(name);
    }

    [Theory]
    [LegoAutoData]
    public async Task Put_updates_category(LegoPartCategory category, string expectedName)
    {
        Context.PartCategories.Add(category);
        Context.SaveChanges();

        var categoryDto = new UpdateCategoryDto
        {
            Name = expectedName
        };

        ActionResult result = await controller.Put(category.Id, categoryDto);
        result.Should().BeOfType<NoContentResult>();

        LegoPartCategory actual = Context.PartCategories.Single(c => c.Id == category.Id);
        actual.Name.Should().BeEquivalentTo(expectedName);
    }

    [Fact]
    public async Task Put_return_404_when_category_not_found()
    {
        var categoryDto = new UpdateCategoryDto
        {
            Name = "name"
        };

        ActionResult result = await controller.Put(int.MinValue, categoryDto);
        result.Should().BeOfType<NotFoundResult>();
    }

    [Theory]
    [LegoAutoData]
    public async Task Delete_removes_category(LegoPartCategory category)
    {
        Context.PartCategories.Add(category);
        Context.SaveChanges();

        ActionResult result = await controller.Delete(category.Id);
        result.Should().BeOfType<NoContentResult>();

        LegoPartCategory? actual = Context
            .PartCategories
            .AsNoTracking()
            .SingleOrDefault(c => c.Id == category.Id);

        actual.Should().BeNull();
    }
}
