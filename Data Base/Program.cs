using Data_Base;
using Whois;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;

string path = @"D:\ru_domains\ru_domains.txt";
//DBController.SaveToDBInBatches(path);

DBController.GetUserFromExpirationDate();



Console.WriteLine( DBController.GetWhoisInformation("whois.ripn.net", "SMART-ONLINE.RU"));