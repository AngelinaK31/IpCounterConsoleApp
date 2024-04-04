using IpConsoleApp;

var dateStart = new DateTime();

Console.Write("-file-log ");
var path = Console.ReadLine();
Console.Write("-file-output ");
var path2 = Console.ReadLine();
Console.Write("-address-start ");
var start = Console.ReadLine();
Console.Write("-address-mask ");
var mask = Console.ReadLine();

try
{
    if (!string.IsNullOrEmpty(start))
    {
        dateStart = DateTime.ParseExact(start, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
    }
}
catch
{
    Console.WriteLine("Неправильный формат даты");
    return;
}


IpCounter ipCounter = new(dateStart, mask);
var result = await ipCounter.SaveCountedIp(path, path2);

if (result.IsSuccess)
    Console.WriteLine("Данные успешно сохранены");
else
    Console.WriteLine(result.Error);
