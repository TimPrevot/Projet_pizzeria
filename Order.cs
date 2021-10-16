using System;

namespace Projet_pizzeria
{
    public class Order
    {
        private int orderId;
        private static int count = 0;

        private DateTime date;
        private int client;
        private int clerk;

        // Constructor
        public Order()
        {
            this.orderId = count++;
        }

        // Getters and setters
        public int OrderId { get; set; }

        public DateTime Date { get; set; }

        public int Client { get; set; }

        public int Clerk { get; set; }
    }
}