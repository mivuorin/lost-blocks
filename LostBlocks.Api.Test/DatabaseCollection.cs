using Xunit;

namespace LostBlocks.Api.Test;

[CollectionDefinition("Database")]
public class DatabaseCollection : ICollectionFixture<DatabaseFixture>
{
}