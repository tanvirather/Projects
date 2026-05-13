using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base.Tests;

public class TestDbContext(DbContextOptions<TestDbContext> options) : DbContext(options)
{
    public DbSet<TestEntity> TestEntities { get; set; }
    public DbSet<TestListEntity> TestListEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestListEntity>().HasKey(e => e.Id);
    }
}

public class TestEntity : BaseEntity { }
public class TestModel : TestEntity { }

public class TestRepository(TestDbContext context) : BaseRepository<TestDbContext, TestEntity, TestModel>(context)
{
    protected override IQueryable<TestModel> Query => context.TestEntities.Select(e => new TestModel
    {
        Id = e.Id,
        Updated = e.Updated,
        UpdatedById = e.UpdatedById
    });
}

public class TestListEntity : BaseListEntity<TestEnum> { }

public class RepositoryTests
{
    private (TestDbContext, SqliteConnection) CreateSqliteContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseSqlite(connection)
            .Options;
        var context = new TestDbContext(options);
        context.Database.EnsureCreated();
        return (context, connection);
    }

    [Fact]
    public async Task BaseRepository_Add_Should_AddItem()
    {
        // Arrange
        var (context, connection) = CreateSqliteContext();
        try
        {
            var repo = new TestRepository(context);
            var entity = new TestEntity { Id = Guid.NewGuid() };

            // Act
            var result = await repo.Add(entity);

            // Assert
            Assert.NotEqual(default, result.Updated);
            Assert.Equal(1, await context.TestEntities.CountAsync());
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task BaseRepository_Get_Should_ReturnItems()
    {
        // Arrange
        var (context, connection) = CreateSqliteContext();
        try
        {
            var id = Guid.NewGuid();
            context.TestEntities.Add(new TestEntity { Id = id });
            await context.SaveChangesAsync();
            var repo = new TestRepository(context);

            // Act
            var result = await repo.Get(id);

            // Assert
            Assert.Single(result);
            Assert.Equal(id, result[0].Id);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task BaseRepository_Update_Should_UpdateItem()
    {
        // Arrange
        var (context, connection) = CreateSqliteContext();
        try
        {
            var id = Guid.NewGuid();
            var entity = new TestEntity { Id = id };
            context.TestEntities.Add(entity);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            var repo = new TestRepository(context);

            // Act
            entity.UpdatedById = Guid.NewGuid();
            var result = await repo.Update(entity);

            // Assert
            Assert.NotEqual(default, result.Updated);
            var updatedEntity = await context.TestEntities.FindAsync(id);
            Assert.Equal(entity.UpdatedById, updatedEntity!.UpdatedById);
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task BaseRepository_Delete_Should_RemoveItem()
    {
        // Arrange
        var (context, connection) = CreateSqliteContext();
        try
        {
            var id = Guid.NewGuid();
            context.TestEntities.Add(new TestEntity { Id = id });
            await context.SaveChangesAsync();
            var repo = new TestRepository(context);

            // Act
            await repo.Delete(id);

            // Assert
            Assert.Equal(0, await context.TestEntities.CountAsync());
        }
        finally
        {
            connection.Close();
        }
    }

    [Fact]
    public async Task BaseListRepository_Get_Should_ReturnFilteredItems()
    {
        // Arrange
        var (context, connection) = CreateSqliteContext();
        try
        {
            context.TestListEntities.AddRange(
                new TestListEntity { Id = TestEnum.Value1, Text = "B", IsActive = true, SortOrder = 2 },
                new TestListEntity { Id = TestEnum.Value2, Text = "A", IsActive = true, SortOrder = 1 }
            );
            await context.SaveChangesAsync();
            var repo = new BaseListRepository<TestDbContext>(context);

            // Act
            var result = await repo.Get<TestListEntity, TestEnum>();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(TestEnum.Value2, result[0].Id); // SortOrder 1 comes before 2
            Assert.Equal(TestEnum.Value1, result[1].Id);
        }
        finally
        {
            connection.Close();
        }
    }
}
