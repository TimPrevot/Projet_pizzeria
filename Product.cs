using System;

namespace Projet_pizzeria
{
    public class Product
    {
        private int productId;
        private static int count = 0;

        private string productType;
        private int price;
        private string name;
        private string size;
        private bool available;

        public Product()
        {
            this.productId = count++;
        }
    }
}