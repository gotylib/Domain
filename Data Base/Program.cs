using Data_Base;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

string path = @"D:\ru_domains\ru_domains.txt";
string format = "yyyy-MM-dd";
List<string> lines = new List<string>();

// Создаем экземпляр Stopwatch для измерения времени
Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();

try
{
    // Чтение всех строк из файла
    using (var reader = new StreamReader(path))
    {
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            lines.Add(line);
        }
    }

    // Определяем размер пакета для параллельной обработки
    int batchSize = 1000; // Размер пакета
    var batches = lines.Select((line, index) => new { line, index })
                       .GroupBy(x => x.index / batchSize)
                       .Select(g => g.Select(x => x.line).ToList())
                       .ToList();

    // Обработка строк параллельно
    Parallel.ForEach(batches, batch =>
    {
        using (var db = new ApplicationContext())
        {
            foreach (var line in batch)
            {
                var splitLine = line.Split(';');

                if (splitLine.Length < 5)
                {
                    Console.WriteLine($"Пропущена строка: {line} (недостаточно полей)");
                    continue; // Пропустить эту строку
                }

                var domain = new Domain
                {
                    Name = splitLine[0],
                    Registrar = splitLine[1],
                    DateOfRegistration = DateTime.SpecifyKind(DateTime.ParseExact(splitLine[2], format, CultureInfo.InvariantCulture), DateTimeKind.Utc),
                    ExpirationDate = DateTime.SpecifyKind(DateTime.ParseExact(splitLine[3], format, CultureInfo.InvariantCulture), DateTimeKind.Utc),
                    Status = splitLine[4],
                    Price = null
                };

                db.Domains.Add(domain);
            }

            // Сохраняем изменения после обработки пакета
            db.SaveChanges();
            Console.WriteLine($"Сохранено {batch.Count} записей");
        }
    });

    Console.WriteLine("Данные успешно добавлены в БД.");
}
catch (Exception ex)
{
    Console.WriteLine($"Ошибка: {ex.Message}");
}
finally
{
    // Останавливаем таймер и выводим время выполнения
    stopwatch.Stop();
    Console.WriteLine($"Время выполнения: {stopwatch.Elapsed.TotalSeconds} секунд");
}
    