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

        // Constructor
        public Product()
        {
            this.productId = count++;
        }

        // Getters and setters
        public int getProductId()
        {
            return this.productId;
        }

        public string getProductType()
        {
            return this.productType;
        }

        public void setProductType(string type)
        {
            if (type == "pizza" | type == "boisson")
            {
                this.productType = type;
            }
            else
            {
                Console.WriteLine("Invalid parameters !");
            }
        }

        public int getPrice()
        {
            return this.price;
        }

        public void setPrice(int price)
        {
            this.price = price;
        }

        public string getName()
        {
            return this.name;
        }

        public void setName(string name)
        {
            this.name = name;
        }

        public string getSize()
        {
            return this.size;
        }

        public void setSize(string size)
        {
            if (size == "m" | size == "l" | size == "xl")
            {
                this.size = size;
            }
            else
            {
                Console.WriteLine("Invalid parameters !");
            }
        }

        public bool getAvailable()
        {
            return this.available;
        }

        public void setAvailable(bool available)
        {
            this.available = available;
        }
    }
}