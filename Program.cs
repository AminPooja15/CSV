using csv;
using System.Text;

Console.WriteLine("Enter File Path for CSV File.");
string? filePath = Console.ReadLine();

Console.WriteLine("Enter Write Path (Folder)");
string? writePath = Console.ReadLine();

if (!string.IsNullOrEmpty(filePath))
{
    List<Person> data = new List<Person>();
    FileStream fileStream = new FileStream(filePath, FileMode.Open);
    using StreamReader reader = new StreamReader(fileStream);

    string? line;
    string? header = reader.ReadLine();
    while ((line = reader.ReadLine()) != null)
    {
        if (!string.IsNullOrEmpty(line))
        {
            string [] csv = line.Trim().Split(",");

            Person person = new Person
            {
                UserId = csv[0].Trim(),
                Name = csv[1].Trim(),
                Version = int.Parse(csv[2]),
                InsuranceCompany = csv[3].Trim()
            };

            data.Add(person);
        }

        var dataByInsurance = data.GroupBy(x => x.InsuranceCompany);

        foreach (var d in dataByInsurance)
        {
            var orderedData = d.OrderBy(x => x.Name).ToList();

            using StreamWriter file = new StreamWriter(File.Create($"{writePath}/{d.Key}.csv"));

            StringBuilder sb = new StringBuilder("User Id, Name, Version, Insurance Company \n");

            orderedData.ForEach(x => sb.AppendLine($"{x.UserId}, {x.Name}, {x.Version}, {x.InsuranceCompany}"));

            file.WriteLine(sb.ToString());
        }
    }
}