using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VegetableMarket.VegetableStore;

namespace VegetableMarket
{


    public class Vegetable
    {

        public VegetableStore.VegetableAssortment VegetableName { get; set; }
        public VegetableStore.VegetableStatus VegetableStatus { get; set; } = RandomWhenBuyVegetableStatus();
        // fresh , normal , rotten , toxic
        public double BuyingPrice { get; set; }
        public double SalePrice { get; set; }
        public static double rating = 1;
        public static double Rating { get => rating; set => rating = value < 1 ? 1 : value; }
        public Vegetable() { }
        public Vegetable(VegetableStore.VegetableAssortment vegetableName)
        {
            VegetableName = vegetableName;
            VegetableStatus = RandomWhenBuyVegetableStatus();
            BuyingPrice = GetVegetableBuyingPrice(vegetableName);
            SalePrice = GetVegetableSalesPrice(vegetableName);
            Rating = 1;

        }
        public static VegetableStatus RandomWhenBuyVegetableStatus()
        {
            var random = new Random();
            int randomInt = random.Next() % 10;

            bool doRot = randomInt > 8; // 10% ehtimalla Toxic

            if (doRot)
                return VegetableStatus.Toxic;
            else
                return VegetableStatus.Fresh;
        }
    }
}