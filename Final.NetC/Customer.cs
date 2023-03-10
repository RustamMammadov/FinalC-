using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VegetableMarket
{
    
    public class Customer
    {
        private static int id = 0;
        public int Id
        {
            get { return id; }
            set { Id = id; }
        }
        public Customer()
        {
            ++id;
        }
    }
}