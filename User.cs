using System;

namespace Projet_pizzeria
{
    public class User
    {
        private int user_id;
        private static int count = 0;
        private string firstName;
        private string lastName;
        private string tel;
        private string address;
        private string city;
        private string postalCode;
        private string entity;


        public User()
        {
            this.user_id = count++;
        }
    }
}
