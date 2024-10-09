using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Data_Base
{
    public class DBController
    {
        private readonly ILogger<DBController> _logger;
        public DBController(ILogger<DBController> logger)
        {
            _logger = logger;
        }

        public static void SaveToDBInBatches(string path)
        {
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
                int batchSize = 10000; // Размер пакета
                int count = 0;
                using (var db = new ApplicationContext())
                {
                    foreach (var line in lines)
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
                        count++;
                        if (count == batchSize)
                        {
                            db.SaveChanges();
                            Console.WriteLine("Сохранен 3000 записей");
                            count = 0;

                        }
                    }

                    // Сохраняем изменения после обработки пакета
                    db.SaveChanges();
                    Console.WriteLine($"Сохранено {lines.Count} записей");
                }

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

        }
        public static void GetUserFromExpirationDate()
        {
            using(ApplicationContext db = new ApplicationContext())
            {
                var minExpirationDate = db.Domains.Min(d=> d.ExpirationDate);
                db.Domains.Where(d => d.ExpirationDate == minExpirationDate).Load();
                foreach (var domain in db.Domains)
                {
                    Console.WriteLine(domain.Name);
                    Console.WriteLine(domain.DateOfRegistration);
                    Console.WriteLine(domain.ExpirationDate);
                }
            }
        }

        /// <summary>
        /// Gets the whois information.
        /// </summary>
        /// <param name="whoisServer">The whois server.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static string GetWhoisInformation(string whoisServer, string url)
        {
            StringBuilder stringBuilderResult = new StringBuilder();
            TcpClient tcpClinetWhois = new TcpClient(whoisServer, 43);
            NetworkStream networkStreamWhois = tcpClinetWhois.GetStream();
            BufferedStream bufferedStreamWhois = new BufferedStream(networkStreamWhois);
            StreamWriter streamWriter = new StreamWriter(bufferedStreamWhois);

            streamWriter.WriteLine(url);
            streamWriter.Flush();

            StreamReader streamReaderReceive = new StreamReader(bufferedStreamWhois);

            while (!streamReaderReceive.EndOfStream)
                stringBuilderResult.AppendLine(streamReaderReceive.ReadLine());

            return stringBuilderResult.ToString();
        }
    }
}
