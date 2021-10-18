using System;

namespace Projet_pizzeria
{
    public class User
    {
        private int user_id;
        private string firstName;
        private string lastName;
        private string tel;
        private string address;
        private string city;
        private string postalCode;
        private int entity;
        private string username;
        private string password;


        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Tel { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string PostalCode { get; set; }

        public int Entity { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }
}