using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Komair.Specifications.Abstract;
using Komair.Specifications.EntityFrameworkCore.Extensions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Komair.Specifications.EntityFrameworkCore.UnitTests.Extensions;

[TestFixture]
public sealed class QueryableSpecificationExtensionsTests
{
    [Test]
    public async Task WhereIf_WhenConditionFalseWithPredicate_ReturnsAllRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var query = fixture.Context.Items.AsQueryable().WhereIf(false, t => t.Id == 2);
        var rows = await query.ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task WhereIf_WhenConditionFalseWithSpecification_ReturnsAllRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var query = fixture.Context.Items.AsQueryable().WhereIf(false, new ByIdSpecification(2));
        var rows = await query.ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(2));
    }

    [Test]
    public async Task WhereIf_WhenConditionTrueWithPredicate_ReturnsFilteredRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var query = fixture.Context.Items.AsQueryable().WhereIf(true, t => t.Id == 2);
        var rows = await query.ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(1));
        Assert.That(rows[0].Name, Is.EqualTo("beta"));
    }

    [Test]
    public async Task WhereIf_WhenConditionTrueWithSpecification_ReturnsFilteredRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var query = fixture.Context.Items.AsQueryable().WhereIf(true, new ByIdSpecification(2));
        var rows = await query.ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(1));
        Assert.That(rows[0].Name, Is.EqualTo("beta"));
    }

    [Test]
    public async Task Where_WhenCombinedSpecificationsMatch_ReturnsMatchingRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var specification = new NameContainsSpecification("et").And(new ByIdSpecification(2));
        var rows = await fixture.Context.Items.AsQueryable().Where(specification).ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(1));
        Assert.That(rows[0].Name, Is.EqualTo("beta"));
    }

    [Test]
    public async Task Where_WhenSpecificationMatches_ReturnsMatchingRows()
    {
        await using var fixture = await SqliteFixture.CreateSeededAsync();
        var rows = await fixture.Context.Items.AsQueryable().Where(new ByIdSpecification(1)).ToListAsync();

        Assert.That(rows, Has.Count.EqualTo(1));
        Assert.That(rows[0].Name, Is.EqualTo("alpha"));
    }

    [Test]
    public async Task Where_WhenSpecificationUsesClosureCapturedId_QueryStringUsesParameterPlaceholder()
    {
        await using var fixture = await SqliteFixture.CreateEmptyAsync();
        var captured = 99;
        var specification = new ByIdSpecification(captured);
        var sql = fixture.Context.Items.AsQueryable().Where(specification).ToQueryString();

        Assert.That(Regex.IsMatch(sql, @"=\s*@\w+"), Is.True, () => sql);
        Assert.That(sql, Does.Not.Contain("= 99"));
    }

    private sealed class ByIdSpecification(Int32 id) : SpecificationBase<InventoryItem>
    {
        private readonly Int32 _id = id;

        public override Expression<Func<InventoryItem, Boolean>> ToExpression()
        {
            return t => t.Id == _id;
        }
    }

    private sealed class InventoryItem
    {
        public Int32 Id { get; set; }

        public String Name { get; set; } = "";
    }

    private sealed class NameContainsSpecification(String fragment) : SpecificationBase<InventoryItem>
    {
        private readonly String _fragment = fragment;

        public override Expression<Func<InventoryItem, Boolean>> ToExpression()
        {
            return t => t.Name.Contains(_fragment);
        }
    }

    private sealed class SqliteFixture : IAsyncDisposable
    {
        private readonly SqliteConnection _connection;

        private SqliteFixture(SqliteConnection connection, TestDbContext context)
        {
            _connection = connection;

            Context = context;
        }

        public TestDbContext Context { get; }

        public static async Task<SqliteFixture> CreateEmptyAsync()
        {
            var connection = new SqliteConnection("Data Source=:memory:");
            await connection.OpenAsync();
            var options = new DbContextOptionsBuilder<TestDbContext>().UseSqlite(connection).Options;
            var context = new TestDbContext(options);
            await context.Database.EnsureCreatedAsync();

            return new SqliteFixture(connection, context);
        }

        public static async Task<SqliteFixture> CreateSeededAsync()
        {
            var fixture = await CreateEmptyAsync();

            fixture.Context.Items.AddRange(new InventoryItem { Id = 1, Name = "alpha" }, new InventoryItem { Id = 2, Name = "beta" });

            await fixture.Context.SaveChangesAsync();

            return fixture;
        }

        public async ValueTask DisposeAsync()
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }
    }

    private sealed class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
    {
        public DbSet<InventoryItem> Items => Set<InventoryItem>();
    }
}
