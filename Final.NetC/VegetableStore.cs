using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace VegetableMarket
{
    public static class VegetableStore
    {
        public enum VegetableAssortment
        {
            Sogan = 0,
            Kartof,
            Biber,
            Kelem,
            Pomidor,
            Xiyar,
            Badimcan
        }



        public enum VegetableStatus
        {
            Fresh = 0,
            Normal,
            Rotten,
            Toxic
        }

        public static double GetVegetableBuyingPrice(VegetableAssortment vegetableAssortment)
        {
            switch (vegetableAssortment)
            {
                case VegetableAssortment.Kelem:
                    return 0.3;
                case VegetableAssortment.Pomidor:
                    return 0.4;
                case VegetableAssortment.Biber:
                    return 0.5;
                case VegetableAssortment.Xiyar:
                    return 0.6;
                case VegetableAssortment.Badimcan:
                    return 0.7;
                case VegetableAssortment.Kartof:
                    return 0.8;
                case VegetableAssortment.Sogan:
                    return 0.9;
                default: return 0;
            }
        }

        public static double GetVegetableSalesPrice(VegetableAssortment vegetableAssortment)
        {
            switch (vegetableAssortment)
            {
                case VegetableAssortment.Kelem:
                    return 0.6;
                case VegetableAssortment.Pomidor:
                    return 0.7;
                case VegetableAssortment.Biber:
                    return 0.8;
                case VegetableAssortment.Xiyar:
                    return 0.9;
                case VegetableAssortment.Badimcan:
                    return 1;
                case VegetableAssortment.Kartof:
                    return 1.1;
                case VegetableAssortment.Sogan:
                    return 1.2;
                default: return 0;
            }
        }
    }
}
