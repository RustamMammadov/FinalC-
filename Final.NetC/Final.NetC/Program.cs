using VegetableMarket;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using static VegetableMarket.VegetableStore;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

Market market = new();
var random = new Random();
long hourTime = 0;
bool IsPandemic = false;
Report report = new Report();

#region Main

market.AddVegetablesToMarket();//ilk bashda bosh markete terevez almaq 
Thread.Sleep(4000);

while (true)
{
    Console.Clear();
    hourTime++;

    if (IsPandemic)
    {
        Console.Clear();
        Console.WriteLine("Pandemiya bashladigi uchun magaza baglidir.");
    }

    if (!IsPandemic)
    {
        market.CustomerBuyingVegetable();//Mushterinin markete gelishi,terevez almagi
        Thread.Sleep(1000);
        market.ShowMarketHour(); // Her saatda (10 saniyede bir) butun melumatlari print edir
        Thread.Sleep(10000); // real vaxtla 10 saniyede 1 isleyir = market vaxti ile 1 saatda 1 defe
        Console.Clear();
        market.ClearDayStatistics();
    }

    if (hourTime % 24 == 0)
    {
        market.RotExistingVegetables(); //Her gun (24 saatda 1 defe) terevezlerin xarab olma methodu
        if (!IsPandemic)
        {
            market.DoIncreaseRating();// 24 saatda bir reytinqi 1 artirir
        }
    }

    if (hourTime % 168 == 0)
    {
        IsPandemic = HappenPandemicRandomly();
        if (!IsPandemic)
        {
            market.AddVegetablesToMarket();// Heftede 1 defe markete terevezler getirilir
        }
            market.WriteReport(ref report);//heftelik hesabat reporta yaziir 
            report.Run();
            Thread.Sleep(5000);
            market.ClearWeekStatistics();
    }
}

#endregion

static bool HappenPandemicRandomly() //100-de bir pandemiya ehtimali.
{
    return new Random().Next(0, 100) == 0;
}