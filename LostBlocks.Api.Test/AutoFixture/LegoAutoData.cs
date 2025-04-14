using AutoFixture;
using AutoFixture.Xunit2;

namespace LostBlocks.Api.Test.AutoFixture;

/// <summary>
///     Automatic test data generation.
///     Note! All relations and hierarchies are not initialized.
/// </summary>
public class LegoAutoData() : AutoDataAttribute(FixtureFactory)
{
    private static IFixture FixtureFactory()
    {
        var fixture = new Fixture();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        fixture
            .Customize(new LegoThemeCustomization())
            .Customize(new LegoSetCustomization())
            .Customize(new LegoColorCustomization());
        return fixture;
    }
}
