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
        #region Property

        public Dictionary<VegetableAssortment, Stack<Vegetable>> MarketStands { get; set; } = new();
        public Queue<Customer> CustomerQueueWeek { get; set; } = new();
        public Queue<Customer> CustomerQueueHour { get; set; } = new();
        public int WorkerCount { get; set; } = 1;
        public Dictionary<VegetableAssortment, int> TrashHour { get; set; } = new();
        public Dictionary<VegetableAssortment, int> TrashWeek { get; set; } = new();
        public double CashBoxWeek { get; set; } = 0;
        public double CashBoxHour { get; set; } = 0;
        public double SpendMoney { get; set; } = 0;

        private double rating = 2;
        public double Rating { get => rating; set => rating = value < 1 ? 1 : value; }
        #endregion

        #region Marketin reytinqini artirib azaltma ve ishchi goturme

        public void DoIncreaseRating()
        {
            if (Rating < 100) { Rating++; }
            else if (Rating >= 100)
            {
                Rating = 100;
            }
        }
        public void DoDecreaseRating()
        {
            if (Rating > 0) { Rating--; }
        }
        public void EmployeeRecruitment()// Ishchi goturme methodu
        {
            WorkerCount += 1;


        }
        #endregion

        #region Customer
        public int ArrivalOfTheCustomerCount()//neche dene alici gelecek
        {
            int count;
            count = (int)Rating;
            return count;
        }
        public Dictionary<VegetableAssortment, int> ShoppingList()//hansi terevezleri ve neche dene alacaq
        {
            List<VegetableAssortment> vegetableAssortments = new();//alinan terevezlerin adlari olacaq.
            var existingVegetablesTypeList = MarketStands.Keys.ToList();//movcud terevez novleri siyahisi
            var random = new Random();
            int randomInt = random.Next(1, existingVegetablesTypeList.Count + 1);//neche nov terevez alacaq
            for (int i = 1; i <= randomInt; i++)//
            {
                int c = random.Next(0, existingVegetablesTypeList.Count);//hansi terevezi alacaq
                if (!vegetableAssortments.Contains(existingVegetablesTypeList[c]))
                    vegetableAssortments.Add(existingVegetablesTypeList[c]);
                else i--;
            }

            Dictionary<VegetableAssortment, int> shoppingList = new();

            foreach (var vegetableAssortment in vegetableAssortments)
            {
                int howMuchVegetable = random.Next(1, 11); //neche dene alacaq
                Tuple<VegetableAssortment, int> tuple = new(vegetableAssortment, howMuchVegetable);
                shoppingList.Add(tuple.Item1, tuple.Item2);
            }
            return shoppingList;
        }
        public bool HaveShoppingListToMarket(Dictionary<VegetableAssortment, int> shoppingList)//Alinacaq terevezler varketde varmi??
        {
            if (MarketStands.Count >= shoppingList.Count)
            {
                foreach (var item in shoppingList)
                {
                    if (MarketStands.ContainsKey(item.Key) && MarketStands[item.Key].Count < item.Value)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public void ShowCustomerChek(Dictionary<VegetableAssortment, int> shoppingList)//Mushterinin cheki
        {
            if (shoppingList.Count >= 0)
            {
                double sum = 0;
                foreach (var item in shoppingList)
                {
                    Console.WriteLine($"{item.Key}  {item.Value} eded --- {GetVegetableSalesPrice(item.Key) * item.Value} AZN");
                    sum += (GetVegetableSalesPrice(item.Key) * item.Value);
                }
                Console.WriteLine($"\nOdenilecek mebleg : {sum}\n");
            }
        }
        public void CustomerBuyingVegetable()//Mushterinin markete gelishi,terevez almagi
        {
            int customerCount = ArrivalOfTheCustomerCount();//nece alici gelecek
            Console.WriteLine("*************************************************************");
            Console.WriteLine($"Markete {customerCount} alici geldi");
            for (int c = 1; c <= customerCount; c++)
            {
                Thread.Sleep(1000);
                Customer customer = new();
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine($"Alici < {customer.Id} >");
                CustomerQueueHour.Enqueue(customer);
                CustomerQueueWeek.Enqueue(customer);

                List<Vegetable> customerBasket = new();//alicinin sebeti
                Dictionary<VegetableAssortment, int> shoppingList = ShoppingList();// hansi terevezleri ve neche dene alacaq
                bool exit = false;
                if (HaveShoppingListToMarket(shoppingList))
                {
                    foreach (var tuple in shoppingList)
                    {
                        if (exit)
                        {
                            break;
                        }
                        for (int i = 0; i < tuple.Value; i++)
                        {
                            var temp = MarketStands[tuple.Key].Pop();
                            if (temp.VegetableStatus == VegetableStore.VegetableStatus.Toxic)
                            {
                                exit = true;
                                Console.WriteLine($"Mushteri toxik {temp.VegetableName} gorduyu uchun dukani terk etdi.");

                                if (TrashHour.ContainsKey(temp.VegetableName))
                                {
                                    TrashHour[temp.VegetableName] = +1;
                                }
                                else
                                {
                                    TrashHour.Add(temp.VegetableName, 1);
                                }
                                VegetableStore.DoDecreaseRatingVegetable(temp.VegetableName);//TerevezinReytinqiniAzaltma
                                break;
                            }
                            else if (temp.VegetableStatus == VegetableStore.VegetableStatus.Rotten)
                            {
                                Console.WriteLine($"Mushteri churuk {temp.VegetableName} atdi\n");
                                if (TrashHour.ContainsKey(temp.VegetableName))
                                {
                                    TrashHour[temp.VegetableName] = +1;
                                }
                                else
                                {
                                    TrashHour.Add(temp.VegetableName, 1);
                                }
                                i--;
                                VegetableStore.DoDecreaseRatingVegetable(temp.VegetableName);//TerevezinReytinqiniAzaltma
                            }
                            else
                            {
                                customerBasket.Add(temp);
                                VegetableStore.DoIncreaseRatingVegetable(temp.VegetableName);//TerevezinReytinqiniArtirma
                            }
                        }
                    }
                    if (exit)
                    {
                        Rating-=0.1;
                    }
                    else
                    {
                        foreach (var vegetable in customerBasket)
                        {
                            CashBoxHour += vegetable.SalePrice;
                        }
                        CashBoxWeek += CashBoxHour;
                        foreach (var tuple in TrashHour)
                        {
                            if (TrashHour.ContainsKey(tuple.Key))
                            {
                                TrashHour[tuple.Key] += tuple.Value;
                            }
                            else
                            {
                                TrashHour.Add(tuple.Key, tuple.Value);
                            }
                        }
                        Rating += 0.01;
                        Console.WriteLine("Mushterinin aldigi terevezler:");
                        ShowCustomerChek(shoppingList);
                        Console.WriteLine("Mushteri Kassaya odenish etdi ve dukani terk etdi.");
                    }
                }
                else if (!exit)
                {
                    Console.WriteLine("Mushterinin almaq istediyi terevezler marketde olmadigi uchun, marketi terk etdi.");
                    Rating -= 0.01;
                }
            }
            Console.WriteLine("*************************************************************");

        }//Mushterinin markete gelishi,terevez almagi
        #endregion

        #region Show
        public void ShowMarketHour()
        {
            Console.WriteLine($"\n<<<<<<<<<<<<<< Market Haqqinda Saatliq Melumat >>>>>>>>>>>>>>\n");

            Console.WriteLine($"Marketin Reytinqi : {Rating}");
            Console.WriteLine($"Bir saat erzinde mushteri sayi : {CustomerQueueHour.Count}");
            Console.WriteLine($"Bir saat erzinde Qazanilmish pul : {CashBoxHour}");
            Console.WriteLine($"Stenddeki mehsullar :");
            if (MarketStands.Count > 0)
            {
                foreach (var stend in MarketStands)
                {
                    Console.WriteLine($"{stend.Key} --- {stend.Value.Count} eded");
                }
            }
            else { Console.WriteLine("\nStend boshdur"); }

            ShowTrash(TrashHour);
            Console.WriteLine($"\n>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>*<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<");
        }//Marketin saatliq melumatlarini print edir
        public void ShowTrash(Dictionary<VegetableAssortment, int> Trash)
        {
            if (Trash.Count > 0)
            {
                Console.WriteLine("\nAtilan terevezler :");
                foreach (var item in Trash)
                {
                    Console.WriteLine($"{item.Key} --- {item.Value} ");
                }
            }
            else { Console.WriteLine("Atilan terevez yoxdur"); }
        }//Trashi gostermek
        #endregion

        #region Terevezin random kohnelmesi

        public Vegetable RotVegetableRandomly(Vegetable vegetable) //bir terevezin 20% ehtimalla kohnelmesi
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
            foreach (var pair in MarketStands)
            {
                Stack<Vegetable> tempStackReverse = new();
                Stack<Vegetable> tempStack = new();
                foreach (var veg in pair.Value)
                {
                    tempStackReverse.Push(RotVegetableRandomly(veg));
                }
                foreach (var veg in tempStackReverse)
                {
                    tempStack.Push(veg);
                }
                MarketStands[pair.Key] = tempStack;
            }
        }

        #endregion

        #region Terevez alinma ve Markete elave olunma

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
                    break;
                }
            }
            return newVegetable;
        }

        public bool TheMarketNeedVegetableAssortment()//Yeni tereveze ehtiyac varsa
        {
            //mümkün Tərəvəz Növləri Siyahısi
            var possibleVegetablesTypeList = Enum.GetValues(typeof(VegetableAssortment)).Cast<VegetableAssortment>().ToList();
            //movcud terevez novleri siyahisi
            var existingVegetablesTypeList = MarketStands.Keys.ToList();

            foreach (var possibleVegetableName in possibleVegetablesTypeList)
            {
                if (!existingVegetablesTypeList.Contains(possibleVegetableName))
                {
                    return true;
                }
            }

            return false;
        }
        Dictionary<VegetableAssortment, double> HowBuyVegetable() //hansi terevez  ve ya terevezler alinmalidir
        {
            Dictionary<VegetableAssortment, double> vegNameAndRating = new();
            if (MarketStands.Count() > 0)
            {
                foreach (var tuple in MarketStands)
                {
                    if (VegetableStore.RatingVegetable(tuple.Key) > 3000)
                    {
                        vegNameAndRating.Add(tuple.Key, VegetableStore.RatingVegetable(tuple.Key));
                    }

                }
                if (TheMarketNeedVegetableAssortment())
                {
                    Vegetable veg = CreateNewVegetable();
                    vegNameAndRating.Add(veg.VegetableName, VegetableStore.RatingVegetable(veg.VegetableName));
                    //alinmasi uchun Markete yeni terevez sechildi
                }
                return vegNameAndRating;
            }
            //market boshdursa alinmasi uchun  yeni terevez
            Vegetable vegetable1 = new(VegetableAssortment.Sogan);
            Vegetable vegetable2 = new(VegetableAssortment.Kartof);
            vegNameAndRating.Add(vegetable1.VegetableName, 1);
            vegNameAndRating.Add(vegetable2.VegetableName, 1);
            return vegNameAndRating;
        }
        Dictionary<VegetableAssortment, Stack<Vegetable>> BuyVegetable()
        //bazardan reytinqe uygun bir nov terevez alma
        {
            Dictionary<VegetableAssortment, double> vegNameAndRating = HowBuyVegetable();//hansi terevezler ve reytinqi
            Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack = new();
            foreach (var item in vegNameAndRating)
            {
                int count = 6000;
                if (item.Value > 3000)
                {
                    
                    count = (int)(item.Value* 1.5);
                }


                Stack<Vegetable> stackVeg = new();
                for (int i = 0; i < count; i++)
                {
                    Vegetable veg = new(item.Key);
                    stackVeg.Push(veg);//toxic ve fresh terevezler ola biler
                    SpendMoney += veg.BuyingPrice;
                }
                dicEnumAndStack.Add(item.Key, stackVeg);
            }
            return dicEnumAndStack;
        }
        public void PrintBuyingVegetable(Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack)
        //Getirilen terevezlerin adi ve sayini  gosterecek
        {
            if (dicEnumAndStack.Count > 0)
            {
                foreach (var item in dicEnumAndStack)
                {
                    Console.WriteLine($"{item.Value.Count} eded {item.Key} alindi.");
                }
            }
            else
            {
                Console.WriteLine("Bu hefte terevez alinmayib");
            }
        }
        public void CleanToxicAndRotten()
        //toxic ve churukleri silib Trasha yazmaq,freshleri alta normallari uste yigmaq
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
                    if (vegetable.VegetableStatus == VegetableStatus.Fresh)
                    {
                        tempFresh.Add(vegetable);
                    }
                    else if (vegetable.VegetableStatus == VegetableStatus.Normal)
                    {
                        tempNormal.Add(vegetable);
                    }
                    else
                    {
                        if (TrashWeek.ContainsKey(stand.Key))
                        {
                            TrashWeek[stand.Key] = +1;
                        }
                        else
                        {
                            TrashWeek.Add(stand.Key, 1);
                        }
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
                temporaryDic.Add(stand.Key, tempStack);
            }
            MarketStands.Clear();
            MarketStands = temporaryDic;
        }
        public void AddVegetablesToMarket()
        //Terevez mARketde yoxdursa yeni Stend yaradacaq, varsa elave edecek
        {
            int numberOfWorkersHired = 0;
            Dictionary<VegetableAssortment, Stack<Vegetable>> dicEnumAndStack = BuyVegetable();//alinacaq terevez
            PrintBuyingVegetable(dicEnumAndStack); //alinan terevezleri gosterecek
            foreach (var item in dicEnumAndStack)
            {
                if (MarketStands.ContainsKey(item.Key))//marketde varsa uygun stende elave et 
                {
                    foreach (var veg in item.Value)
                    {
                        MarketStands[item.Key].Push(veg);//uygun stende elave
                    }
                }
                else // marketde yoxdursa Yeni stand yarat
                {
                    MarketStands.Add(item.Key, item.Value);
                    EmployeeRecruitment(); //yeni stend yarandiqca ishchi goturur
                    numberOfWorkersHired++;
                }
            }
            Console.WriteLine($"\nIshe goturulen ishci sayi : {numberOfWorkersHired}");
            CleanToxicAndRotten(); //toxic ve churukleri silib Trasha yazmaq, freshleri alta normallari uste yigmaq
            ShowTrash(TrashWeek); //atilanlarin hesabati
        }

        #endregion

        #region Clear Statistics

        public void ClearHourStatistics()
        {
            CustomerQueueHour.Clear();
            TrashHour.Clear();
            CashBoxHour = 0;
            Console.Clear();
        }
        public void ClearWeekStatistics()
        {
            CustomerQueueWeek.Clear();
            TrashWeek.Clear();
            CashBoxWeek = 0;
            SpendMoney = 0;
            Console.Clear();
        }
        #endregion
        public void WriteReport(ref Report report)
        {
            report.TrashReportWeek.Clear();
            foreach (var item in TrashWeek)
            {
                report.TrashReportWeek.Add(item.Key.ToString(), item.Value);
            }
            report.Rating = Rating;
            report.ReportCashBoxWeak = CashBoxWeek;
            report.CustomerCountWeak = CustomerQueueWeek.Count;
        }
    }
}
