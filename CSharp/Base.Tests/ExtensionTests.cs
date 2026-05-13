using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base.Tests;

public class ExtensionTests
{
    private DbContextOptions<TestDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public void DbSet_LoadCsvData_Should_Attempt_To_Load()
    {
        // Arrange
        using var context = new TestDbContext(CreateOptions());
        // Since LoadCsvData uses Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName)
        // it will look for the file in the test execution directory.
        // We can create a dummy file there.
        var fileName = "test_entities.csv";
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
        File.WriteAllText(filePath, "Id\n" + Guid.NewGuid().ToString());

        try
        {
            // Act
            context.TestEntities.LoadCsvData(fileName);

            // Assert
            Assert.True(context.TestEntities.Local.Count > 0);
        }
        finally
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }

    [Fact]
    public void ModelBuilder_ToSnakeCase_Should_SetNames()
    {
        // Arrange
        var builder = new ModelBuilder();
        builder.Entity<TestEntity>();

        // Act
        builder.ToSnakeCase("custom_schema");

        // Assert
        var entity = builder.Model.FindEntityType(typeof(TestEntity));
        Assert.Equal("test_entity", entity!.GetTableName());
        Assert.Equal("custom_schema", entity.GetSchema());
    }
}
