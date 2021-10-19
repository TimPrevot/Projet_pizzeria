using System;
using System.Collections.Generic;

namespace Projet_pizzeria
{
    public class Order
    {
        private int orderId;
        private static int count = 0;

        private DateTime date;
        private int client;
        private int clerk;
        private int price;
        private List<Product> items;

        // Constructor
        public Order()
        {
            this.items = new List<Product>();
            this.Items = new List<Product>();
            this.price = 0;
        }

        // Getters and setters
        public int OrderId { get; set; }

        public DateTime Date { get; set; }

        public int Client { get; set; }

        public int Clerk { get; set; }
        
        public int Price { get; set; }
        
        public List<Product> Items { get; set; }
    }
}