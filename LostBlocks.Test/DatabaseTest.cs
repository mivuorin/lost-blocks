using LostBlocks.Data;
using Microsoft.EntityFrameworkCore.Storage;
using Xunit;

namespace LostBlocks.Test;

[Collection("Database")]
public class DatabaseTest : IDisposable, IAsyncDisposable
{
    private readonly IDbContextTransaction transaction;

    public DatabaseTest(DatabaseFixture fixture)
    {
        Context = fixture.Factory.CreateDbContext();
        transaction = Context.Database.BeginTransaction();
    }

    protected LegoContext Context { get; }

    public async ValueTask DisposeAsync()
    {
        await transaction.RollbackAsync();
        await transaction.DisposeAsync();
        await Context.DisposeAsync();
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        transaction.Rollback();
        transaction.Dispose();
        Context.Dispose();
        GC.SuppressFinalize(this);
    }
}
