using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableMarket
{
    public class Report
    {
        public double Rating { get; set; } = 0;
        public int CustomerCountWeak { get; set; } = 0;
        public double ReportCashBox { get; set; } = 0;
        public double ReportEarnedManeyWeak { get; set; } = 0;
        public Dictionary<string,int> TrashReportWeek { get; set; } = new();


        public Report() { }

        public void ShowReport()
        {
            Console.WriteLine($"Marketin reytinqi : {Rating}\nHeftelik alici sayi : {CustomerCountWeak}\nMarketin heftelik qazanci : {ReportEarnedManeyWeak}");
            foreach (var item in TrashReportWeek)
            {
                Console.WriteLine("Atilan terevezler : ");
                Console.WriteLine($"{item.Key}  {item.Value} eded.\n");
            }
        }

        public void CleanReport()
        {
            Rating = 0;
            CustomerCountWeak = 0;
            ReportCashBox = 0;
            ReportEarnedManeyWeak = 0;
            TrashReportWeek = new();
        }
    }
}
