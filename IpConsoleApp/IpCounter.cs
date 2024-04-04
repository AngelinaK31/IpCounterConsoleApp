using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IpConsoleApp
{
    /// <summary>
    /// Класс для подсчета ip за определенный период
    /// </summary>
    /// <param name="startDate">дата начала отсчета</param>
    /// <param name="endMask">маска подсети, задающая верхнюю границу диапазона</param>
    public class IpCounter(DateTime startDate, string endMask)
    {
        /// <summary>
        /// Сохранение подсчитанных данных
        /// </summary>
        /// <param name="logPath">путь txt файла с логами ip</param>
        /// <param name="outputPath">путь для сохранения данных</param>
        /// <returns>Возвращает класс Result, который показывает успешно ли сохранение и хранит ошибки</returns>
        public async Task<Result> SaveCountedIp (string logPath, string outputPath)
        {
            if(string.IsNullOrEmpty(logPath))
                return new Result { IsSuccess = false, Error="Путь к логам не найден" };

            List<LogIp> ips = new();
            try
            {
                using (StreamReader writer = new StreamReader($@"{logPath}"))
                {
                    var test = await writer.ReadToEndAsync();
                    foreach (var item in test.Split('\n'))
                    {
                        var date = DateTime.ParseExact(item.Split('>')[0].TrimEnd(), "yyyy-MM-dd HH:mm:ss",
                            System.Globalization.CultureInfo.InvariantCulture);
                        if (startDate <= date)
                        {
                            var fullIp = item.Split('>')[1].TrimStart();
                            var ip = fullIp.Split(" ")[0];
                            var itemMask = fullIp.Split(" ")[1].TrimEnd();
                            if (ips.Any(x => x.Ip == ip))
                            {
                                ips.Find(x => x.Ip == ip).Count += 1;
                            }
                            else
                            {
                                ips.Add(new LogIp
                                {
                                    Ip = ip,
                                    Count = 1
                                });
                            }
                            if (endMask == itemMask)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result { IsSuccess = false, Error = $"Ошибка: {ex}" };
            }

            using (StreamWriter writer = new StreamWriter($@"{outputPath}", false))
            {
                foreach (var logIp in ips)
                {
                    await writer.WriteLineAsync(logIp.Ip + " " + logIp.Count);
                }
            }

            return new Result { IsSuccess = true };
        }

        class LogIp
        {
            public string Ip;
            public int Count;

        }

        public class Result
        {
            public bool IsSuccess;
            public string Error = "";
        }
    }
}
