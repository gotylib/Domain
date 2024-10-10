using System.Globalization;
using System.Net.Sockets;
using System.Text;

namespace Data_Base_Controll.Extensions
{
    public static class StringExtensions
    {
        // Словарь WHOIS серверов для разных доменов
        private static readonly Dictionary<string, string> WhoisServers = new Dictionary<string, string>
    {
        { "com", "whois.verisign-grs.com" },
        { "net", "whois.verisign-grs.com" },
        { "org", "whois.publicdomainregistry.com" },
        { "ru", "whois.tci.ru" },
        { "su", "whois.tci.ru" },
        { "rf", "whois.tci.ru" }
    };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Domain? ConvertStringToDomain(this string value)
        {
            var splitLine = value.Split(';');
            string format = "yyyy-MM-dd";
            try
            {
                var domain = new Domain
                {
                    Name = splitLine[0],
                    Registrar = splitLine[1],
                    DateOfRegistration = DateTime.SpecifyKind(DateTime.ParseExact(splitLine[2], format, CultureInfo.InvariantCulture), DateTimeKind.Utc),
                    ExpirationDate = DateTime.SpecifyKind(DateTime.ParseExact(splitLine[3], format, CultureInfo.InvariantCulture), DateTimeKind.Utc),
                    Status = splitLine[4],
                    Price = null
                };
                
                return domain;
            }
            catch(Exception ex) 
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
            
        }

        /// <summary>
        /// Gets the whois information.
        /// </summary>
        /// <param name="whoisServer">The whois server.</param>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static Domain? GetWhoisInformation(this string url)
        {
            string whoisServer = url.Split('.')[0];

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

            return stringBuilderResult.ToString().ConvertStringToDomain();
        }
    }
}
