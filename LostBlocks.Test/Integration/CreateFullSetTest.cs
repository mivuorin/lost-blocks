using FluentAssertions;
using LostBlocks.Api.Category;
using LostBlocks.Api.Color;
using LostBlocks.Api.Inventory;
using LostBlocks.Api.InventoryPart;
using LostBlocks.Api.InventorySet;
using LostBlocks.Api.Part;
using LostBlocks.Api.Set;
using LostBlocks.Api.Theme;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace LostBlocks.Test.Integration;

public class CreateFullSetTest : DatabaseTest
{
    private readonly CategoryController categoryController;
    private readonly ColorController colorController;
    private readonly InventoryController inventoryController;
    private readonly InventoryPartController inventoryPartController;
    private readonly InventorySetController inventorySetController;
    private readonly PartController partController;
    private readonly SetDetailsController setController;
    private readonly ThemeController themeController;

    public CreateFullSetTest(DatabaseFixture fixture) : base(fixture)
    {
        themeController = new ThemeController(Context);
        setController = new SetDetailsController(Context);
        inventoryController = new InventoryController(Context);
        categoryController = new CategoryController(Context);
        partController = new PartController(Context);
        colorController = new ColorController(Context);
        inventoryPartController = new InventoryPartController(Context);
        inventorySetController = new InventorySetController(Context);
    }

    [Fact]
    public async Task CreateFullSet()
    {
        var createThemeDto = new CreateThemeDto
        {
            Name = "Programming",
            ParentId = null
        };
        ActionResult themeResult = await themeController.Post(createThemeDto);
        var themeId = ResultToId(themeResult);

        var createSetDto = new CreateSetDto
        {
            SetNum = "064-1",
            Name = "Commodore-64",
            Year = 1982,
            ThemeId = themeId
        };
        ActionResult setResult = await setController.Post(createSetDto);
        var setNum = ResultToString(setResult);

        var inventoryDto = new CreateInventoryDto
        {
            Version = 1,
            SetNum = setNum
        };
        ActionResult inventoryResult = await inventoryController.Post(inventoryDto);

        var inventoryId = ResultToId(inventoryResult);

        ActionResult categoryResult = await categoryController.Post(new CreateCategoryDto
        {
            Name = "Lost Blocks"
        });

        var categoryId = ResultToId(categoryResult);

        await partController.Post(new CreatePartDto
        {
            Name = "Main Unit",
            CategoryId = categoryId,
            PartNum = "C64A"
        });
        await partController.Post(new CreatePartDto
        {
            Name = "Datasette",
            CategoryId = categoryId,
            PartNum = "C64B"
        });

        ActionResult greyColorResult = await colorController.Post(new CreateColorDto
        {
            Name = "Dirty Grey",
            Rgb = "8F8B66",
            IsTransparent = false
        });

        var greyColorId = ResultToId(greyColorResult);

        ActionResult blackColorResult = await colorController.Post(new CreateColorDto
        {
            Name = "Dark Black",
            Rgb = "0C0C0C",
            IsTransparent = false
        });

        var blackColorId = ResultToId(blackColorResult);

        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = inventoryId,
            PartNum = "C64A",
            ColorId = greyColorId,
            Quantity = 1,
            IsSpare = false
        });
        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = inventoryId,
            PartNum = "C64B",
            ColorId = greyColorId,
            Quantity = 1,
            IsSpare = false
        });
        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = inventoryId,
            PartNum = "C64B",
            ColorId = greyColorId,
            Quantity = 1,
            IsSpare = true
        });

        var createJoysticksSetDto = new CreateSetDto
        {
            SetNum = "064-2",
            Name = "C64 Joysticks",
            Year = 1982,
            ThemeId = themeId
        };
        ActionResult joysticksSetResult = await setController.Post(createJoysticksSetDto);
        var joysticksSetNum = ResultToString(joysticksSetResult);

        var joysticksInventoryDto = new CreateInventoryDto
        {
            SetNum = joysticksSetNum,
            Version = 1
        };
        ActionResult joysticksInventoryResult = await inventoryController.Post(joysticksInventoryDto);
        var joysticksInventoryId = ResultToId(joysticksInventoryResult);

        await inventorySetController.Post(new CreateInventorySetDto
        {
            InventoryId = inventoryId,
            SetNum = joysticksSetNum,
            Quantity = 2
        });

        await partController.Post(new CreatePartDto
        {
            Name = "Classic Joystick",
            CategoryId = categoryId,
            PartNum = "C64JC"
        });
        await partController.Post(new CreatePartDto
        {
            Name = "Grenade Joystick",
            CategoryId = categoryId,
            PartNum = "C64JG"
        });

        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = joysticksInventoryId,
            Quantity = 1,
            PartNum = "C64JC",
            ColorId = greyColorId,
            IsSpare = false
        });

        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = joysticksInventoryId,
            Quantity = 1,
            PartNum = "C64JC",
            ColorId = greyColorId,
            IsSpare = true
        });

        await inventoryPartController.Post(new CreateInventoryPartDto
        {
            InventoryId = joysticksInventoryId,
            Quantity = 1,
            PartNum = "C64JG",
            ColorId = blackColorId,
            IsSpare = false
        });

        var result = await inventoryController.Get(inventoryId);
        result.Value.Should().NotBeNull();

        var expected = new InventoryDetailsDto
        {
            Id = inventoryId,
            SetNum = "064-1",
            Version = 1,
            Sets =
            [
                new InventorySetDto
                {
                    SetNum = "064-2",
                    Quantity = 2
                }
            ],
            Parts =
            [
                new InventoryPartDto
                {
                    Category = "Lost Blocks",
                    Color = "Dirty Grey",
                    Name = "Main Unit",
                    PartNum = "C64A",
                    Quantity = 1,
                    Rgb = "8F8B66",
                    Transparent = false
                },
                new InventoryPartDto
                {
                    Category = "Lost Blocks",
                    Color = "Dirty Grey",
                    Name = "Datasette",
                    PartNum = "C64B",
                    Quantity = 1,
                    Rgb = "8F8B66",
                    Transparent = false
                }
            ],
            Spares =
            [
                new InventoryPartDto
                {
                    Category = "Lost Blocks",
                    Color = "Dirty Grey",
                    Name = "Datasette",
                    PartNum = "C64B",
                    Quantity = 1,
                    Rgb = "8F8B66",
                    Transparent = false
                }
            ]
        };

        result.Value.Should().BeEquivalentTo(expected);
    }

    private string ResultToString(ActionResult result)
    {
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdAtActionResult = (CreatedAtActionResult)result;

        createdAtActionResult
            .Value
            .Should()
            .NotBeNull("ActionResult value is null.");

        return (string)createdAtActionResult.Value;
    }

    private static int ResultToId(ActionResult result)
    {
        result.Should().BeOfType<CreatedAtActionResult>();

        var createdAtActionResult = (CreatedAtActionResult)result;

        createdAtActionResult
            .Value
            .Should()
            .NotBeNull("ActionResult value is null.");

        return (int)createdAtActionResult.Value;
    }
}
