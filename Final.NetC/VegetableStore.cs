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
        static int ratingKelem = 1;
        static int ratingPomidor = 1;
        static int ratingBiber = 1;
        static int ratingXiyar = 1;
        static int ratingBadimcan = 1;
        static int ratingKartof = 1;
        static int ratingSogan = 1;
        public static int RatingVegetable(VegetableAssortment vegetableAssortment)
        {
            switch (vegetableAssortment)
            {
                case VegetableAssortment.Kelem:
                    return ratingKelem;
                    
                case VegetableAssortment.Pomidor:
                    return ratingPomidor;
                    
                case VegetableAssortment.Biber:
                    return ratingBiber;
                case VegetableAssortment.Xiyar:
                    return ratingXiyar;
                case VegetableAssortment.Badimcan:
                    return ratingBadimcan;
                case VegetableAssortment.Kartof:
                    return ratingKartof;
                case VegetableAssortment.Sogan:
                    return ratingSogan;
                    default: return 0;
            }
        }
        public static void DoIncreaseRatingVegetable(VegetableAssortment vegetableAssortment)
        {
            switch (vegetableAssortment)
            {
                case VegetableAssortment.Kelem:
                    ratingKelem += 1;
                    break;
                case VegetableAssortment.Pomidor:
                    ratingPomidor += 1;
                    break;
                case VegetableAssortment.Biber:
                    ratingBiber += 1;
                    break;
                case VegetableAssortment.Xiyar:
                    ratingXiyar += 1;
                    break;
                case VegetableAssortment.Badimcan:
                    ratingBadimcan += 1;
                    break;
                case VegetableAssortment.Kartof:
                    ratingKartof += 1;
                    break;
                case VegetableAssortment.Sogan:
                    ratingSogan += 1;
                    break;

            }
        }
        public static void DoDecreaseRatingVegetable(VegetableAssortment vegetableAssortment)
        {
            switch (vegetableAssortment)
            {
                case VegetableAssortment.Kelem:
                    ratingKelem -= 1;
                    if (ratingKelem < 0)
                    {
                        ratingKelem = 1;
                    }
                    break;
                case VegetableAssortment.Pomidor:
                    ratingPomidor -= 1;
                    if (ratingPomidor < 0)
                    {
                        ratingPomidor = 0;
                    }
                    break;
                case VegetableAssortment.Biber:
                    ratingBiber -= 1;
                    if (ratingBiber < 0)
                    {
                        ratingBiber = 1;
                    }
                    break;
                case VegetableAssortment.Xiyar:
                    ratingXiyar -= 1;
                    if (ratingXiyar < 0)
                    {
                        ratingXiyar = 1;
                    }
                    break;
                case VegetableAssortment.Badimcan:
                    ratingBadimcan -= 1;
                    if (ratingBadimcan < 0)
                    {
                        ratingBadimcan = 1;
                    }
                    break;
                case VegetableAssortment.Kartof:
                    ratingKartof -= 1;
                    if (ratingKartof < 0)
                    {
                        ratingKartof = 1;
                    }
                    break;
                case VegetableAssortment.Sogan:
                    ratingSogan -= 1;
                    if (ratingSogan < 0)
                    {
                        ratingSogan = 1;
                    }
                    break;
            }
        }
    }
}