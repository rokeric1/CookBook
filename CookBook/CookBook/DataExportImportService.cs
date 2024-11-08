using System.Text.Json;
using System.Xml.Serialization;

public class DataExportImportService
{
    public void EksportujRecepte(string format, List<string> recepti)
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

    private void ExportToJson(List<string> recepti)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        File.WriteAllText("recepti.json", JsonSerializer.Serialize(recepti, options));
        Console.WriteLine("Recepti su uspješno eksportovani u JSON format.");
    }

    private void ExportToXml(List<string> recepti)
    {
        var serializer = new XmlSerializer(typeof(List<string>));
        using var writer = new FileStream("recepti.xml", FileMode.Create);
        serializer.Serialize(writer, recepti);
        Console.WriteLine("Recepti su uspješno eksportovani u XML format.");
    }

    public List<string> ImportFromJson()
    {
        var jsonData = File.ReadAllText("recepti.json");
        return JsonSerializer.Deserialize<List<string>>(jsonData) ?? new List<string>();
    }

    public List<string> ImportFromXml()
    {
        var serializer = new XmlSerializer(typeof(List<string>));
        using var reader = new FileStream("recepti.xml", FileMode.Open);
        return (List<string>)serializer.Deserialize(reader) ?? new List<string>();
    }
}
