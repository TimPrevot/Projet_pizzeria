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

        public Order()
        {
            this.orderId = count++;
        }
    }
}