using System.Text.Json;
using System.Text.Json.Serialization;

namespace task13;

public class Subject
{
    public string? Name { get; set; }
    public int Grade { get; set; }
}

public class Student
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    [JsonConverter(typeof(DateConverter))]
    public DateTime BirthDate { get; set; }
    public List<Subject>? Grades { get; set; }
}

public class DateConverter : JsonConverter<DateTime>
{
    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateStr = reader.GetString();
        if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out var result))
        {
            return result;
        }
        throw new JsonException($"Invalid date format. Expected dd.MM.yyyy, but got '{dateStr}'.");
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString("dd.MM.yyyy"));
    }
}

public static class JsonHandler
{
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new DateConverter() }
    };

    public static string ToJson(Student student)
    {
        return JsonSerializer.Serialize(student, Options);
    }

    public static Student Deserialize(string json)
    {
        return JsonSerializer.Deserialize<Student>(json, Options)!;
    }

    public static void SaveJson(Student student, string path)
    {
        File.WriteAllText(path, ToJson(student));
    }

    public static Student LoadJson(string path)
    {
        return Deserialize(File.ReadAllText(path));
    }
}