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

        public void Run()//Heftelik reportda olunacaqlar
        {
            Serialize();//heftelik report serialize olunur
            ShowWeekReport(); //heftelik report gosterilir
            Thread.Sleep(4000);
            CleanReport();//heftelik report sifirlanir
        }
        public void ShowWeekTrash()
        {
            if (TrashReportWeek.Count > 0)
                foreach (var item in TrashReportWeek)
                {
                    Console.WriteLine($"{item.Key}  {item.Value} eded.");
                }
            else { Console.WriteLine("Atilan terevez yoxdur.\n"); }
        }
        public void ShowWeekReport()
        {
            Console.WriteLine("\nHeftelik Hesabat:");
            Console.WriteLine($"Marketin reytinqi : {Rating}\nHeftelik alici sayi : {CustomerCountWeak}\nMarketin heftelik qazanci : {ReportCashBoxWeak}");
            Console.WriteLine("\nAtilan terevezler : ");
            ShowWeekTrash();
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
            File.WriteAllText($"Report{weekNum}.json", json);//jsona yazmaq
            File.WriteAllText($"Report{weekNum}.txt", json);//texte yazmaq
        }
    }
}