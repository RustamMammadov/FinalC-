using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VegetableMarket.VegetableStore;

namespace VegetableMarket
{
    public class Market
    {

        public Dictionary<VegetableAssortment, Stack<Vegetable>> MarketStands { get; set; } = new();
        public Queue<Customer> CustomerQueue { get; set; } = new();
        public int WorkerCount { get; set; } = 2;
        public Dictionary<Vegetable ,int> Trash { get; set; } = new();
        public double MarketCashBox { get; set; } = 1000;
        public double MoneyEarned { get; set; } = 0;
        public double CashBox { get; set; } = 0;
        public double SpendMoney { get; set; } = 0;
        public double Rating { get; set; } = 0;
        public Dictionary<string, int> BuyingVegetableWeek { get; set; } = new();

        public void EmployeeRecruitment()
        {
            if (MarketStands.Count > 1)
            {
                int acceptedWorkerCount = MarketStands.Count - WorkerCount;
                if (acceptedWorkerCount > 0) { Console.WriteLine($"\nMarkete {acceptedWorkerCount} ishchi qebul olundu\n"); }
                WorkerCount += acceptedWorkerCount;
            }
        }

        #region Marketin reytinqini artirib azaltma

        public void DoIncreaseRating()
        {
            if (Rating < 100) { Rating++; }
        }
        public void DoDecreaseRating()
        {
            if (Rating > 0) { Rating--; }
        }
        #endregion

        #region Terevez alinma

        Vegetable CreateNewVegetable() // Mehsul cesidini artirma//Marketde olmayan terevez  qaytarir
        {
            //mümkün Tərəvəz Növləri Siyahısi
            var possibleVegetablesTypeList = Enum.GetValues(typeof(VegetableAssortment)).Cast<VegetableAssortment>().ToList();
            //movcud terevez novleri siyahisi
            var existingVegetablesTypeList = MarketStands.Keys.ToList();

            var newVegetable = new Vegetable();
            foreach (var possibleItem in possibleVegetablesTypeList)
            {
                if (!existingVegetablesTypeList.Contains(possibleItem))
                {
                    newVegetable = new(possibleItem);
                }
            }

            return newVegetable;
        }

        Dictionary<VegetableAssortment, double> HowBuyVegetable() //hansi terevez  ve ya terevezler alinmalidir
        {
            Dictionary<VegetableAssortment, double> vegNameAndRating = new();
            if (!(MarketStands.Count() == 0))
            {
                foreach (var stack in MarketStands.Values)
                {
                    foreach (var item in stack)
                    {
                        if (item.Rating > 1)
                            vegNameAndRating.Add(item.VegetableName, item.Rating);
                    }
                }
                if (Rating % 10 > 0)
                {
                    vegNameAndRating.Add(CreateNewVegetable().VegetableName, 10); //Markete yeni terevez elave olundu

                }
                return vegNameAndRating;
            }
            vegNameAndRating.Add(CreateNewVegetable().VegetableName, 10);//Markete terevez alindi
            return vegNameAndRating;
        }

        Dictionary<VegetableAssortment, Stack<Vegetable>> BuyVegetable()
        //bazardan reytinqe uygun ne qeder terevez alinmali
        {
            Dictionary<VegetableAssortment, double> vegNameAndRating = HowBuyVegetable();//hansi terevez ve reytinqi
            Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack = new();
            foreach (var item in vegNameAndRating)
            {
                Stack<Vegetable> stackVeg = new();
                for (int i = 0; i < (item.Value * 10); i++)
                {
                    Vegetable veg = new(item.Key);
                    stackVeg.Push(veg);//toxic ve fresh terevezler ola biler
                }
                dicEnumAndStack.Add(stackVeg.Peek().VegetableName, stackVeg);

            }
            return dicEnumAndStack;
        }

        public void BuyingVegetableCount(Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack)
        //Getirilen terevezlerin adi ve sayini BuyingVegetableWeek elave edecek ve gosterecek
        {
            BuyingVegetableWeek.Clear();
            if (dicEnumAndStack.Count > 0)
            {
                foreach (var stend in dicEnumAndStack)
                {
                    
                    BuyingVegetableWeek.Add(stend.Key.ToString(), stend.Value.Count);
                    Console.WriteLine($"\n{stend.Value.Count()} eded {stend.Key} alindi.\n");
                }
            }

        }

        public void AddVegetablesToMarket()
        //vegetable, mARketde yoxdusa yeni stend yaradacaq varsa elave edecek,xerclenen pullari SpendMoney elave edecek
        {
            Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack = BuyVegetable();//alinacaq terevez
            BuyingVegetableCount(dicEnumAndStack); //alinan terevezleri gosterecek
            foreach (var item in dicEnumAndStack)
            {
                if (MarketStands.ContainsKey(item.Key))//marketde varsa uygun stende elave et
                {
                    foreach (var veg in item.Value)
                    {
                        MarketCashBox -= veg.BuyingPrice;//bazardan terevez alir
                        SpendMoney += veg.BuyingPrice; // xerclenen pullara elave olunur
                        MarketStands[item.Key].Push(veg);
                    }
                }
                else // marketde yoxdursa Yeni stand yarat
                {
                    MarketCashBox -= item.Value.Peek().BuyingPrice * item.Value.Count;//bazardan terevez alir
                    SpendMoney += item.Value.Peek().BuyingPrice * item.Value.Count;// xerclenen pullara elave olunur
                    MarketStands.Add(item.Key, item.Value);
                }
            }
        }

        #endregion

        #region Terevez kohnelme

        Vegetable RotVegetableRandomly(Vegetable vegetable) //bir terevezin 20% ehtimalla kohnelmesi
        {
            var random = new Random();
            int randomInt = random.Next() % 10;

            bool doRot = randomInt > 7; // 20% ehtimalla kohnelecek

            if (doRot)
            {
                switch (vegetable.VegetableStatus)
                {
                    case VegetableStatus.Fresh:
                        vegetable.VegetableStatus = VegetableStatus.Normal;
                        break;
                    case VegetableStatus.Normal:
                        vegetable.VegetableStatus = VegetableStatus.Rotten;
                        break;
                    case VegetableStatus.Rotten:
                        vegetable.VegetableStatus = VegetableStatus.Toxic;
                        break;
                    default: break;
                }
            }
            return vegetable;
        }

        public void RotExistingVegetables() //Terevezlerin churume metodu
        {
            Dictionary<VegetableAssortment, Stack<Vegetable>> tempMarketStands = new();
            foreach (var pair in MarketStands)
            {
                Stack<Vegetable> tempStack = new();
                foreach (var veg in pair.Value)
                {
                    tempStack.Push(RotVegetableRandomly(veg));
                }
                tempStack.Reverse();
                tempMarketStands.Add(tempStack.Peek().VegetableName, tempStack);
            }
            MarketStands = tempMarketStands;
        }

        #endregion

        public void CleanToxicAndRotten()//toxic ve churukleri silib Trasha yazmaq,freshleri alta normallari uste yigmaq
        {
            Dictionary<VegetableAssortment, Stack<Vegetable>> temporaryDic = new();

            foreach (var stand in MarketStands)
            {
                Stack<Vegetable> tempStack = new();
                List<Vegetable> tempFresh = new();
                List<Vegetable> tempNormal = new();
                List<Vegetable> tempTrash = new();
                foreach (var vegetable in stand.Value)
                {
                    if (vegetable.VegetableStatus == VegetableStatus.Fresh && vegetable.VegetableStatus == VegetableStatus.Normal)
                    {
                        if (vegetable.VegetableStatus == VegetableStatus.Fresh)
                        {
                            tempFresh.Add(vegetable);
                        }
                        else
                        {
                            tempNormal.Add(vegetable);
                        }
                    }
                    else
                    {
                        tempTrash.Add(vegetable);
                    }

                }
                foreach (var vegetable in tempFresh)
                {
                    tempStack.Push(vegetable);
                }
                foreach (var vegetable in tempNormal)
                {
                    tempStack.Push(vegetable);
                }
                temporaryDic.Add(tempStack.Peek().VegetableName, tempStack);
                foreach (var vegetable in tempTrash)
                {
                    Trash.Add(tempTrash[0],tempTrash.Count);
                }
            }
            MarketStands.Clear();
            MarketStands = temporaryDic;
        }

        public void ShowTrash()
        {
            
            foreach (var item in Trash)
            {
                Console.WriteLine("Atilan terevezler :\n");
                Console.WriteLine($"{item.Key.VegetableName} {item.Value} ");
                
            }
        }

        public void WriteReport(Report report)
        {
            report.TrashReportWeek.Clear();
            foreach (var item in Trash)
            {
                report.TrashReportWeek.Add(item.Key.VegetableName.ToString(),item.Value);
            }

            report.Rating = Rating;
            report.ReportEarnedManeyWeak = MoneyEarned;    
            report.CustomerCountWeak = CustomerQueue.Count;
        }
        
    }
}
