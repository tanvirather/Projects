using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base.Tests;

public class TestCrudController(TestRepository repository)
    : BaseCrudController<TestDbContext, TestRepository, TestEntity, TestModel>(repository);

public class TestListController(BaseListRepository<TestDbContext> repository)
    : BaseListController<TestDbContext, TestListEntity, TestEnum>(repository);

public class ControllerTests
{
    private DbContextOptions<TestDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task BaseCrudController_Get_Should_ReturnItems()
    {
        // Arrange
        using var context = new TestDbContext(CreateOptions());
        var id = Guid.NewGuid();
        context.TestEntities.Add(new TestEntity { Id = id });
        await context.SaveChangesAsync();
        var repo = new TestRepository(context);
        var controller = new TestCrudController(repo);

        // Act
        var result = await controller.Get(id);

        // Assert
        Assert.Single(result);
        Assert.Equal(id, result[0].Id);
    }

    [Fact]
    public async Task BaseCrudController_Add_Should_CallRepository()
    {
        // Arrange
        using var context = new TestDbContext(CreateOptions());
        var repo = new TestRepository(context);
        var controller = new TestCrudController(repo);
        var model = new TestModel { Id = Guid.NewGuid() };

        // Act
        var result = await controller.Add(model);

        // Assert
        Assert.NotEqual(default, result.Updated);
        Assert.Equal(1, await context.TestEntities.CountAsync());
    }

    [Fact]
    public async Task BaseListController_Get_Should_ReturnItems()
    {
        // Arrange
        using var context = new TestDbContext(CreateOptions());
        context.TestListEntities.Add(new TestListEntity { Id = TestEnum.Value1, Text = "Test", IsActive = true });
        await context.SaveChangesAsync();
        var repo = new BaseListRepository<TestDbContext>(context);
        var controller = new TestListController(repo);

        // Act
        var result = await controller.Get();

        // Assert
        Assert.Single(result);
        Assert.Equal(TestEnum.Value1, result[0].Id);
    }
}
