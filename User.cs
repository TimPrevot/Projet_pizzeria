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
        private int entity;
        private string username;
        private string password;


        public int UserId
        {
            get { return user_id; }
            set
            {
                if (user_id == 0)
                {
                    user_id = value;
                }
            }
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Tel { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public int Entity { get; set; }
        
        public string Username { get; set; }
        
        public string Password { get; set; }

        public User()
        {
            this.user_id = count++;
        }
    }
}