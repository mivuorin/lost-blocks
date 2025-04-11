using LostBlocks.Api.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace LostBlocks.Api.Test;

[Collection("Database")]
public class DatabaseTest(DatabaseFixture fixture) : IDisposable
{
    private readonly IDbContextTransaction transaction = fixture.Context.Database.BeginTransaction();

    /// <summary>
    ///     Shorthand accessor property for fixture.Context
    /// </summary>
    protected LegoContext Context => fixture.Context;

    public void Dispose()
    {
        transaction.Rollback();
        transaction.Dispose();
    }
}
