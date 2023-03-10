using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VegetableMarket
{
    public class Report
    {
        public double Rating { get; set; } = 0;
        public int CustomerCountWeak { get; set; } = 0;
        public double ReportCashBoxWeak { get; set; } = 0;
        public Dictionary<string, int> TrashReportWeek { get; set; } = new();
        int weekNum = 0;

        public Report() { }

        public void Run()
        {
            Serialize();//heftelik report serialize olunur
            ShowReport(); //heftelik report gosterilir
            Thread.Sleep(4000);
            CleanReport();//heftelik report sifirlanir
        }
        public void ShowTrash()
        {
            if (TrashReportWeek.Count > 0)
                foreach (var item in TrashReportWeek)
                {
                    Console.WriteLine($"{item.Key}  {item.Value} eded.");
                }
            else { Console.WriteLine("\nAtilan terevez yoxdur.\n"); }
        }
        public void ShowReport()
        {
            Console.WriteLine("\nHeftelik Hesabat:");
            Console.WriteLine($"Marketin reytinqi : {Rating}\nHeftelik alici sayi : {CustomerCountWeak}\nMarketin heftelik qazanci : {ReportCashBoxWeak}");
            Console.WriteLine("Atilan terevezler : ");
            ShowTrash();
        }
        public void CleanReport()
        {
            Rating = 0;
            CustomerCountWeak = 0;       
            ReportCashBoxWeak = 0;
            TrashReportWeek.Clear();
        }
        public void Serialize()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            weekNum++;
            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText($"Report{weekNum}.json", json);
            File.WriteAllText($"Report{weekNum}.txt", json);
        }
    }
}