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
        public int getOrderId()
        {
            return this.orderId;
        }

        public DateTime getDate()
        {
            return this.date;
        }

        public void setDate(DateTime date)
        {
            this.date = date;
        }

        public int getClient()
        {
            return this.client;
        }

        public void setClient(int client)
        {
            this.client = client;
        }

        public int getClerk()
        {
            return this.clerk;
        }

        public void setClerk(int clerk)
        {
            this.clerk = clerk;
        }
    }
}