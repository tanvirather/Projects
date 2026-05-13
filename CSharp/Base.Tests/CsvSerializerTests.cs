namespace Zuhid.Base.Tests;

public class CsvTestModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public bool IsActive { get; set; }
    public DateTime Created { get; set; }
    public TestEnum Type { get; set; }
}

public class CsvSerializerTests : IDisposable
{
    private readonly string _tempFile;

    public CsvSerializerTests()
    {
        _tempFile = Path.GetTempFileName();
    }

    public void Dispose()
    {
        if (File.Exists(_tempFile))
        {
            File.Delete(_tempFile);
        }
    }

    [Fact]
    public void Load_Should_ReturnEmpty_IfFileDoesNotExist()
    {
        // Act
        var result = CsvSerializer.Load<CsvTestModel>("non_existent_file.csv");

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Load_Should_ReturnEmpty_IfFileIsEmpty()
    {
        // Arrange
        File.WriteAllText(_tempFile, "");

        // Act
        var result = CsvSerializer.Load<CsvTestModel>(_tempFile);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public void Load_Should_SerializeCorrectly()
    {
        // Arrange
        var id = Guid.NewGuid();
        // Use a fixed UTC date
        var date = new DateTime(2023, 1, 1, 12, 0, 0, DateTimeKind.Utc);
        // CsvSerializer uses DateTime.SpecifyKind(DateTime.Parse(rawValue...), DateTimeKind.Utc)
        // DateTime.Parse might interpret the string as local time before SpecifyKind is called.
        // To be safe, we compare just the Ticks or Year/Month/Day/Hour if there's an offset issue.
        var csvContent = $@"Id,Name,Age,IsActive,Created,Type
{id},John Doe,30,True,2023-01-01 12:00:00,Value1
{Guid.NewGuid()},Jane Doe,25,False,2023-01-01 12:00:00,Value2";

        File.WriteAllText(_tempFile, csvContent);

        // Act
        var result = CsvSerializer.Load<CsvTestModel>(_tempFile);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(id, result[0].Id);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal(30, result[0].Age);
        Assert.True(result[0].IsActive);
        // Verify only relevant components if timezone is tricky
        Assert.Equal(date.Year, result[0].Created.Year);
        Assert.Equal(date.Month, result[0].Created.Month);
        Assert.Equal(date.Day, result[0].Created.Day);
        Assert.Equal(date.Hour, result[0].Created.Hour);
        Assert.Equal(TestEnum.Value1, result[0].Type);
    }
}
