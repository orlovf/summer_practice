using System.Text.Json;
using Xunit;
using task13; 

namespace task13tests; 

public class JsonTests
{
    private readonly Student _testStudent = new()
    {
        FirstName = "Philipp",
        LastName = "Orlov",
        BirthDate = new DateTime(2006, 5, 24),
        Grades = new List<Subject> { new Subject { Name = "History", Grade = 4 } }
    };

    [Fact]
    public void ToJson_IgnoresNullAndFormatsDate()
    {
        var student = new Student { FirstName = "Philipp", BirthDate = new DateTime(2006, 5, 24) };
        string json = JsonHandler.ToJson(student);
        Assert.Contains("\"FirstName\": \"Philipp\"", json);
        Assert.Contains("\"BirthDate\": \"24.05.2006\"", json);
        Assert.DoesNotContain("\"LastName\"", json);
        Assert.DoesNotContain("\"Grades\"", json);
    }

    [Fact]
    public void Deserialize_ValidJson_CreatesStudent()
    {
        string json = @"{""FirstName"":""Philipp"",""LastName"":""Orlov"",""BirthDate"":""24.05.2006"",""Grades"":[{""Name"":""History"",""Grade"":4}]}";
        var student = JsonHandler.Deserialize(json);
        Assert.NotNull(student);
        Assert.Equal("Philipp", student.FirstName);
        Assert.Equal("Orlov", student.LastName);
        Assert.Equal(new DateTime(2006, 5, 24), student.BirthDate);
        Assert.NotNull(student.Grades);
        Assert.Single(student.Grades);
        Assert.Equal("History", student.Grades[0].Name);
        Assert.Equal(4, student.Grades[0].Grade);
    }

    [Fact]
    public void SaveAndLoadJson_RoundTrip()
    {
        string path = Path.Combine(Path.GetTempPath(), "test.json");
        try
        {
            JsonHandler.SaveJson(_testStudent, path);
            var loaded = JsonHandler.LoadJson(path);
            Assert.NotNull(loaded);
            Assert.Equal(_testStudent.FirstName, loaded.FirstName);
            Assert.Equal(_testStudent.LastName, loaded.LastName);
            Assert.Equal(_testStudent.BirthDate, loaded.BirthDate);
            Assert.NotNull(loaded.Grades);
            Assert.Single(loaded.Grades);
        }
        finally
        {
            if (File.Exists(path)) File.Delete(path);
        }
    }

    [Fact]
    public void Deserialize_InvalidDate_Throws()
    {
        string json = @"{""FirstName"":""Philipp"",""BirthDate"":""2021/05/24""}";
        Assert.Throws<JsonException>(() => JsonHandler.Deserialize(json));
    }
}
