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

        public string GetFirstName() {
            return this.firstName;
        }

        public string SetFirstName() {
            this.firstName = FirstName;
        }

        public string GetLastName() {
            return this.lastName;
        }

        public string SetLastName() {
            this.lastName = LastName;
        }

        public string GetTel(){
            return this.tel;
        }

        public string SetTel(){
            this.tel = Tel;
        }

        public string GetAddress(){
            return this.address;
        }

        public string SetAddress(){
            this.address = Address;
        }

        public string GetCity(){
            return this.city;
        }

        public string SetCity(){
            this.city = City;
        }

        public string GetPostalCode(){
            return this.postalCode;
        }

        public string SetPostalCode(){
            this.postalCode = PostalCode;
        }

        public string GetEntity(){
            return this.entity;
        }

        public string SetEntity(string entity){

            if( entity = "commis" | entity = "livreur" | entity = "client"){
                this.entity = Entity;
            }
            else{
                Console.WriteLine("Unknown entity");
            }
            
        }


    }
}
