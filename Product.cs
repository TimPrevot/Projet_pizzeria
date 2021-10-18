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

        public int ProductId { get; set; }
        public int ProductType { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public bool Available { get; set; }

    }
}