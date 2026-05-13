namespace Zuhid.Base.Tests;

public class BaseEntityTests
{
    [Fact]
    public void Properties_Should_SetAndGet()
    {
        // Arrange
        var entity = new BaseEntity();
        var id = Guid.NewGuid();
        var updatedById = Guid.NewGuid();
        var updated = DateTime.UtcNow;

        // Act
        entity.Id = id;
        entity.UpdatedById = updatedById;
        entity.Updated = updated;

        // Assert
        Assert.Equal(id, entity.Id);
        Assert.Equal(updatedById, entity.UpdatedById);
        Assert.Equal(updated, entity.Updated);
    }
}

public class BaseModelTests
{
    [Fact]
    public void Should_Inherit_From_BaseEntity()
    {
        // Arrange
        var model = new BaseModel();

        // Assert
        Assert.IsAssignableFrom<BaseEntity>(model);
    }
}

public class UpdatedModelTests
{
    [Fact]
    public void Properties_Should_SetAndGet()
    {
        // Arrange
        var model = new UpdatedModel();
        var updated = DateTime.UtcNow;

        // Act
        model.Updated = updated;

        // Assert
        Assert.Equal(updated, model.Updated);
    }
}

public enum TestEnum
{
    Value1,
    Value2
}

public class BaseListEntityTests
{
    [Fact]
    public void Properties_Should_SetAndGet()
    {
        // Arrange
        var entity = new BaseListEntity<TestEnum>();
        var id = TestEnum.Value1;
        var updatedById = Guid.NewGuid();
        var updated = DateTime.UtcNow;
        var isActive = true;
        var text = "Test Text";
        var sortOrder = 1;

        // Act
        entity.Id = id;
        entity.UpdatedById = updatedById;
        entity.Updated = updated;
        entity.IsActive = isActive;
        entity.Text = text;
        entity.SortOrder = sortOrder;

        // Assert
        Assert.Equal(id, entity.Id);
        Assert.Equal(updatedById, entity.UpdatedById);
        Assert.Equal(updated, entity.Updated);
        Assert.Equal(isActive, entity.IsActive);
        Assert.Equal(text, entity.Text);
        Assert.Equal(sortOrder, entity.SortOrder);
    }
}

public class BaseListModelTests
{
    [Fact]
    public void Properties_Should_SetAndGet()
    {
        // Arrange
        var model = new BaseListModel<TestEnum>();
        var id = TestEnum.Value2;
        var text = "List Text";

        // Act
        model.Id = id;
        model.Text = text;

        // Assert
        Assert.Equal(id, model.Id);
        Assert.Equal(text, model.Text);
    }
}
