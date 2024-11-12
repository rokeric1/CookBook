using System.Text.Json;
using System.Xml.Serialization;

public class DataExportImportService
{
    public void EksportujRecepte(string format, List<Recipe> recepti)
    {
        if (format.Equals("JSON", StringComparison.OrdinalIgnoreCase))
        {
            ExportToJson(recepti);
        }
        else if (format.Equals("XML", StringComparison.OrdinalIgnoreCase))
        {
            ExportToXml(recepti);
        }
        else
        {
            throw new ArgumentException("Nepodržan format: " + format);
        }
    }

    private void ExportToJson(List<Recipe> recepti)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText("receptiExport.json", JsonSerializer.Serialize(recepti, options));
        Console.WriteLine("Recepti su uspješno eksportovani u JSON format.");
    }

    private void ExportToXml(List<Recipe> recepti)
    {
        var serializer = new XmlSerializer(typeof(List<Recipe>));
        using var writer = new FileStream("recepti.xml", FileMode.Create);
        serializer.Serialize(writer, recepti);
        Console.WriteLine("Recepti su uspješno eksportovani u XML format.");
    }

    public List<Recipe> ImportFromJson()
    {
        var jsonData = File.ReadAllText("recepti.json");
        return JsonSerializer.Deserialize<List<Recipe>>(jsonData) ?? new List<Recipe>();
    }

    public List<Recipe> ImportFromXml()
    {
        var serializer = new XmlSerializer(typeof(List<Recipe>));
        using var reader = new FileStream("recepti.xml", FileMode.Open);
        return (List<Recipe>)serializer.Deserialize(reader) ?? new List<Recipe>();
    }
}
