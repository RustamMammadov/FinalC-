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

market.AddVegetablesToMarket();//ilk bashda markete terevez almaq

while (true)
{
    hourTime++;
    IsPandemic = HappenPandemicRandomly();

    if (hourTime % 24 == 0) 
    {
        if (!IsPandemic)
        {
            market.DoIncreaseRating();// 24 saatda bir reytinqi 1 artirir
        }
        market.RotExistingVegetables(); //Her gun (24 saatda 1 defe) terevezlerin xarab olma methodu
        
    }

    if (!IsPandemic)
    {
        if (hourTime % 168 == 0)
        {
            market.AddVegetablesToMarket();// Heftede 1 defe markete yeni mallar getirilir
            market.EmployeeRecruitment();//stendler artdiqca ishchi qebul olunur
            market.CleanToxicAndRotten();//toxic ve churukleri silib Trasha yazmaq,freshleri alta normallari uste yigmaq
            market.ShowTrash();//atilan terevezlerin hesabati
            report.ShowReport();
            report.CleanReport();
        }

    }
    else
    {
        Console.Clear();
        Console.WriteLine("Pandemiya bashladigi uchun magaza baglidir.");

    }

    //ALIcinin dukana gelishi,terevez almagi
    //market.Print(); // Her saatda (10 saniyede bir) butun melumatlari print edir
    //report.HesabatiJsonYazdir();

    Thread.Sleep(10000); // real vaxtla 10 saniyede 1 isleyir = market vaxti ile 1 saatda 1 defe
}



#endregion

static bool HappenPandemicRandomly() //100-de bir pandemiya ehtimali.
{
    return new Random().Next(0, 100) == 0;

}

