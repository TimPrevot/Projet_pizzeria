using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Schema;
using Npgsql;

namespace Projet_pizzeria
{
    public class App
    {
        public App()
        {
            this.actualClient = new User();
            this.connectedClerk = new User();
            this.menu = new List<Product>();
        }

        private User connectedClerk;
        private User actualClient;
        private List<Product> menu;

        private const string PostgreConStr =
            "Server=localhost;Port=5432;UserId=postgres;Password=abcd1234;Database=Projet_pizzeria;";

        public User ConnectedClerk { get; set; }

        public User ActualClient { get; set; }

        public bool IsConnected { get; set; }

        public List<Product> Menu { get; set; }

        // Function to connect the clerk
        public bool connectClerk()
        {
            var ncon = new NpgsqlConnection(PostgreConStr);
            Console.WriteLine("username: ");
            var username = Console.ReadLine();
            Console.WriteLine("password: ");
            var pwd = Console.ReadLine();
            while (username.GetType() != typeof(string) || pwd.GetType() != typeof(string) || username == "" ||
                   pwd == "")
            {
                Console.WriteLine("Please enter correct values");
                Console.WriteLine("username: ");
                username = Console.ReadLine();
                Console.WriteLine("password: ");
                pwd = Console.ReadLine();
            }

            var hashedPwd = "";
            var user_id = -1;
            var firstname = "";
            var lastname = "";
            var telephone = "";
            var cmd = new NpgsqlCommand("SELECT * FROM users WHERE username = '" + username + "'", ncon);
            Console.WriteLine("SELECT * FROM users WHERE username = '" + username + "'");
            ncon.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                hashedPwd = dr["password"].ToString();
                user_id = Convert.ToInt32(dr["user_id"].ToString());
                firstname = dr["firstname"].ToString();
                lastname = dr["lastname"].ToString();
                telephone = dr["telephone"].ToString();
            }

            ncon.Close();

            using (SHA256 sha256Hash = SHA256.Create())
            {
                string hash = GetHash(sha256Hash, pwd);
                Console.WriteLine("Verifying password...");
                if (VerifyHash(sha256Hash, pwd, hash))
                {
                    Console.WriteLine("Authentication successful.");
                    connectedClerk.UserId = user_id;
                    connectedClerk.FirstName = firstname;
                    connectedClerk.LastName = lastname;
                    connectedClerk.Tel = telephone;
                    connectedClerk.Username = username;

                    Console.WriteLine("Clerk connected : " + connectedClerk.FirstName + " " +
                                      connectedClerk.LastName + ", tel: " + connectedClerk.Tel);
                    return true;
                }
                else
                {
                    Console.WriteLine("Authentication failed");
                    return false;
                }
            }
        }

        // Function to get all the available items from the db
        public void getMenu()
        {
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT * FROM menu WHERE available ORDER BY size", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();

            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("product_id", typeof(int)));
            dt.Columns.Add(new DataColumn("product_type", typeof(int)));
            dt.Columns.Add(new DataColumn("price", typeof(int)));
            dt.Columns.Add(new DataColumn("name", typeof(string)));
            dt.Columns.Add(new DataColumn("size", typeof(int)));
            dt.Columns.Add(new DataColumn("available", typeof(bool)));
            Console.WriteLine("Collecting data...");
            while (dr.Read())
            {
                var row = dt.NewRow();
                row["product_id"] = dr["product_id"];
                row["product_type"] = dr["product_type"];
                row["price"] = dr["price"];
                row["name"] = dr["name"];
                row["size"] = dr["size"];
                row["available"] = dr["available"];
                dt.Rows.Add(row);
                var newItem = new Product();
                newItem.ProductId = Convert.ToInt32(dr["product_id"].ToString());
                newItem.ProductType = Convert.ToInt32(dr["product_type"].ToString());
                newItem.Price = Convert.ToInt32(dr["price"].ToString());
                newItem.Name = dr["product_id"].ToString();
                newItem.Size = dr["product_id"].ToString();
                newItem.Available = Convert.ToBoolean(dr["product_id"]);
                menu.Add(newItem);
            }

            Console.WriteLine("All items collected.");
            Console.WriteLine("The whole menu today is: ");
            PrintTable(dt);
            ncon.Close();
            dt.AcceptChanges();
        }

        // Function to change the available parameter on a product
        public void changeAvailable()
        {
            Console.WriteLine("Please write the name of the product you want to modify: ");
            var productName = Console.ReadLine();
            Console.WriteLine("Please indicate the size of the item (1, 2 or 3");
            var productSize = Convert.ToInt32(Console.ReadLine());
            while (productName.GetType() != typeof(string) || productSize.GetType() != typeof(int) ||
                   productName == "" ||
                   productSize == 0)
            {
                Console.WriteLine("Please enter correct values");
                Console.WriteLine("Product name: ");
                productName = Console.ReadLine();
                Console.WriteLine("Product size: ");
                productSize = Convert.ToInt32(Console.ReadLine());
            }

            var checkProduct = false;

            // Input check
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand(
                "SELECT name, size FROM menu WHERE name = '" + productName + "' AND size = '" + productSize + "'",
                ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr["name"].ToString() == productName && Convert.ToInt32(dr["size"].ToString()) == productSize)
                {
                    checkProduct = true;
                }
                else
                {
                    Console.WriteLine("Failure : this item does not exist");
                }
            }

            ncon.Close();

            if (checkProduct == true)
            {
                var res = false;
                cmd = new NpgsqlCommand("SELECT available FROM menu WHERE name = '" + productName + "' AND size = '" +
                                        productSize + "'", ncon);
                ncon.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    res = Convert.ToBoolean(dr["available"]);
                }

                ncon.Close();

                if (res == true)
                {
                    cmd = new NpgsqlCommand("UPDATE menu SET available = false WHERE name = '" + productName +
                                            "' AND size = '" + productSize + "'", ncon);
                }
                else
                {
                    cmd = new NpgsqlCommand("UPDATE menu SET available = true WHERE name = '" + productName +
                                            "' AND size = '" +
                                            productSize + "'", ncon);
                }

                ncon.Open();
                cmd.ExecuteNonQuery();
                ncon.Close();
                getMenu();
            }
        }

        // Function to hash a string input
        private static string GetHash(HashAlgorithm hashAlgorithm, string input)
        {
            byte[] data = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(input));
            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        // Function to compare a passord with a hashed password stored in the db
        private static bool VerifyHash(HashAlgorithm hashAlgorithm, string input, string hash)
        {
            var hashOfInput = GetHash(hashAlgorithm, input);
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;
            return comparer.Compare(hashOfInput, hash) == 0;
        }

        // Function to get the client from the db if this isn't his first order, or create him and add him in the db instead
        public void checkUser()
        {
            Console.WriteLine("Is this the client's first order ? Type 1 for Yes, 0 for No :");
            var isNew = Convert.ToInt32(Console.ReadLine());
            while (isNew != 0 && isNew != 1)
            {
                Console.WriteLine("Please enter a correct value.");
                isNew = Convert.ToInt32(Console.ReadLine());
            }

            // If this is not the client's first order, we get his info from the database
            if (isNew == 0)
            {
                var ncon = new NpgsqlConnection(PostgreConStr);
                Console.WriteLine("Please enter the client's phone number");
                var telClient = Console.ReadLine();
                while (telClient.GetType() != typeof(string) || telClient == "")
                {
                    Console.WriteLine("Please enter a valid input");
                    Console.WriteLine("Client phone number: ");
                    telClient = Console.ReadLine();
                }

                var cmd = new NpgsqlCommand("SELECT * FROM users WHERE telephone = '" + telClient + "'", ncon);
                ncon.Open();
                var dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    this.actualClient.UserId = Convert.ToInt32(dr["user_id"].ToString());
                    this.actualClient.FirstName = dr["firstname"].ToString();
                    this.actualClient.LastName = dr["lastname"].ToString();
                    this.actualClient.Tel = dr["telephone"].ToString();
                    this.actualClient.Address = dr["address"].ToString();
                    this.actualClient.City = dr["city"].ToString();
                    this.actualClient.PostalCode = dr["postal_code"].ToString();
                    this.actualClient.Entity = Convert.ToInt32(dr["entity_id"].ToString());
                }

                ncon.Close();
                Console.WriteLine("Client : " + this.actualClient.FirstName + " " +
                                  this.actualClient.LastName + ", tel: " + this.actualClient.Tel);
            }
            else
            {
                this.addNewUser();
            }
        }

        // Function to add a new user to the db
        public void addNewUser()
        {
            Console.WriteLine("Please enter the user's first name: ");
            var firstname = Console.ReadLine();
            while (firstname.GetType() != typeof(string) || firstname == "")
            {
                Console.WriteLine("your first name needs to be a string !");
                firstname = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's last name: ");
            var lastname = Console.ReadLine();
            while (lastname.GetType() != typeof(string) || lastname == "")
            {
                Console.WriteLine("your first name needs to be a string !");
                lastname = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's phone number with the indicative: ");
            var tel = Console.ReadLine();
            while (tel.GetType() != typeof(string) || tel == "")
            {
                Console.WriteLine("your first name need to be a string !");
                tel = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's address: ");
            var address = Console.ReadLine();
            while (address.GetType() != typeof(string) || address == "")
            {
                Console.WriteLine("your first name need to be a string !");
                address = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's city: ");
            var city = Console.ReadLine();
            while (city.GetType() != typeof(string) || city == "")
            {
                Console.WriteLine("your first name need to be a string !");
                city = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's postal code: ");
            var postalCode = Console.ReadLine();
            while (postalCode.GetType() != typeof(string) || postalCode == "")
            {
                Console.WriteLine("your first name need to be a string !");
                postalCode = Console.ReadLine();
            }

            Console.WriteLine("Please enter the user's entity type: ");
            Console.WriteLine("1: clerk");
            Console.WriteLine("2: client");
            Console.WriteLine("3: driver");
            var entity = Convert.ToInt32(Console.ReadLine());
            while (entity != 1 && entity != 2 && entity != 3)
            {
                Console.WriteLine("Please type a correct value");
                entity = Convert.ToInt32(Console.ReadLine());
            }

            var username = "";
            var pwd = "";

            if (entity == 1)
            {
                Console.WriteLine("Please enter the user's username: ");
                username = Console.ReadLine();
                Console.WriteLine("Please enter the user's password: ");
                pwd = Console.ReadLine();
                var checkValues = false;
                do
                {
                    if (username.GetType() != typeof(string) || pwd.GetType() != typeof(string))
                    {
                        Console.WriteLine("Please enter correct inputs");
                        Console.Write("username:");
                        username = Console.ReadLine();
                        Console.Write("password: ");
                        pwd = Console.ReadLine();
                    }
                    else
                    {
                        checkValues = true;
                    }
                } while (!checkValues);

                using (SHA256 sha256hash = SHA256.Create())
                {
                    var hashPwd = GetHash(sha256hash, pwd);
                    pwd = hashPwd;
                }
            }

            var ncon = new NpgsqlConnection(PostgreConStr);
            var req = "";
            if (entity == 1)
            {
                req = "INSERT INTO users VALUES (default, '" + firstname + "', '" + lastname + "', '" + tel + "', '" +
                      address +
                      "', '" + city + "', '" + postalCode + "', '" + entity + "', '" + username + "', '" + pwd + "')";
            }
            else if (entity == 3)
            {
                req = "INSERT INTO users VALUES (default, '" + firstname + "', '" + lastname + "', '" + tel + "', '" +
                      address +
                      "', '" + city + "', '" + postalCode + "', '" + entity + "', default, default, 'true')";
            }
            else
            {
                req = "INSERT INTO users VALUES (default, '" + firstname + "', '" + lastname + "', '" + tel + "', '" +
                      address +
                      "', '" + city + "', '" + postalCode + "', '" + entity + "')";
            }

            Console.WriteLine("req: " + req);
            var cmd = new NpgsqlCommand(req, ncon);
            ncon.Open();
            cmd.ExecuteNonQuery();
            ncon.Close();
            Console.WriteLine("User added !");
        }

        // Function to create a new order
        public void createOrder()
        {
            var newOrder = new Order();
            newOrder.Date = DateTime.Now;
            var addMore = true;
            var checkAvailable = false;
            var ncon = new NpgsqlConnection(PostgreConStr);
            while (addMore)
            {
                var newProduct = new Product();
                Console.WriteLine("Please enter the name of the item :");
                var itemName = Console.ReadLine();
                Console.WriteLine("Please enter the size of the item :");
                var itemSize = Console.ReadLine();
                newProduct.Name = itemName;
                newProduct.Size = itemSize;
                var cmd = new NpgsqlCommand(
                    "SELECT price FROM menu WHERE name = '" + itemName + "' AND size = '" + itemSize + "'", ncon);
                ncon.Open();
                var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    newProduct.Price = Convert.ToInt32(dr["price"].ToString());
                }
                ncon.Close();
                /*foreach (var item in this.menu.Where(item => item.Name.Equals(itemName)))
                {
                    newProduct.Price = item.Price;
                }*/

                cmd = new NpgsqlCommand(
                    "SELECT available FROM menu WHERE name = '" + itemName + "' AND size = '" + itemSize + "'", ncon);
                ncon.Open();
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (Convert.ToBoolean(dr["available"]) == false)
                    {
                        Console.WriteLine("Sorry ! This item is not available");
                        checkAvailable = false;
                    }
                    else
                    {
                        checkAvailable = true;
                    }
                }

                ncon.Close();

                if (checkAvailable)
                {
                    newOrder.Items.Add(newProduct);
                    newOrder.Price += newProduct.Price;
                }
                // TODO Input check
                Console.WriteLine("Do you want to add one more item ? 1 for Yes, 0 for No");
                checkAvailable = false;
                var choice = Convert.ToInt32(Console.ReadLine());
                if (choice == 0)
                {
                    addMore = false;
                }
            }
            
            // Choosing a driver
            var driverId = -1;
            var cmd2 = new NpgsqlCommand("SELECT user_id FROM users WHERE entity_id = 3 AND available", ncon);
            ncon.Open();
            var dr2 = cmd2.ExecuteReader();
            while (dr2.Read())
            {
                driverId = Convert.ToInt32(dr2["user_id"].ToString());
            }
            ncon.Close();
            
            var reqItems = "";
            for (int i = 0; i < 6; i++)
            {
                if (i <= newOrder.Items.Count - 1)
                {
                    reqItems = String.Concat(reqItems, "', '", newOrder.Items[i].Name);
                }
                else if (i == newOrder.Items.Count)
                {
                    reqItems = String.Concat(reqItems, "', default");
                }
                else
                {
                    reqItems = String.Concat(reqItems, ", default");
                }
            }

            cmd2 = new NpgsqlCommand("INSERT INTO orders VALUES (default, '" +
                                         newOrder.Date + "', '" + this.actualClient.UserId + "', '" +
                                         this.connectedClerk.UserId + "', '" + driverId + reqItems + ", '" + newOrder.Price + "')", ncon);
            Console.WriteLine("req: INSERT INTO orders VALUES (default, '" +
                                                                         newOrder.Date + "', '" + this.actualClient.UserId + "', '" +
                                                                         this.connectedClerk.UserId + "', '" + driverId + reqItems + ", '" + newOrder.Price + "')");
            ncon.Open();
            cmd2.ExecuteNonQuery();
            ncon.Close();
            Console.WriteLine("Created order");
        }

        // Function to get the number of deliveries made by a driver
        public int getDeliveries()
        {
            Console.WriteLine("Please write the firstname of the driver: ");
            var driverName = Console.ReadLine();
            while (driverName.GetType() != typeof(string) || driverName == "")
            {
                Console.WriteLine("Please enter a correct input");
                driverName = Console.ReadLine();
            }

            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE firstname = '" + driverName + "'", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            var driverId = -1;
            var totalDeliveries = 0;
            while (dr.Read())
            {
                driverId = Convert.ToInt32(dr["user_id"]);
            }

            ncon.Close();

            cmd = new NpgsqlCommand("SELECT COUNT (*) FROM orders WHERE driver = " + driverId, ncon);
            ncon.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                totalDeliveries = Convert.ToInt32(dr["count"]);
            }
            ncon.Close();
            return totalDeliveries;
        }

        // Function to get the nb of orders done by a clerk
        public int getClerkOrders()
        {
            Console.WriteLine("Please write the firstname of the clerk: ");
            var clerkName = Console.ReadLine();
            while (clerkName.GetType() != typeof(string) || clerkName == "")
            {
                Console.WriteLine("Please enter a correct input");
                clerkName = Console.ReadLine();
            }

            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE firstname = '" + clerkName + "'", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            var clerkId = -1;
            var totalOrders = 0;
            while (dr.Read())
            {
                clerkId = Convert.ToInt32(dr["user_id"]);
            }

            ncon.Close();

            cmd = new NpgsqlCommand("SELECT COUNT (*) FROM orders WHERE clerk = " + clerkId, ncon);
            ncon.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                totalOrders = Convert.ToInt32(dr["count"]);
            }

            ncon.Close();
            return totalOrders;
        }

        // Function to get the orders between two dates
        public void getOrdersByDate()
        {
            Console.WriteLine("Please give the first date (format: Jan 1, 2000) :");
            var date1 = Console.ReadLine();
            Console.WriteLine("Please give the second date (format: Jan 1, 2000) :");
            var date2 = Console.ReadLine();
            while (date1.GetType() != typeof(string) || date2.GetType() != typeof(string) || date1 == "" || date2 == "")
            {
                Console.WriteLine("Please enter a correct value");
                Console.Write("First date (format: Jan 1, 2000) : ");
                date1 = Console.ReadLine();
                Console.Write("Second date (format: Jan 1, 2000) : ");
                date2 = Console.ReadLine();
            }
            var parsedDate1 = DateTime.Parse(date1);
            var parsedDate2 = DateTime.Parse(date2);
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand(
                "SELECT * FROM orders WHERE date BETWEEN '" + parsedDate1 + "' AND '" + parsedDate2 + "' ORDER BY date", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("order_id", typeof(int)));
            dt.Columns.Add(new DataColumn("date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("client", typeof(int)));
            dt.Columns.Add(new DataColumn("clerk", typeof(int)));
            dt.Columns.Add(new DataColumn("driver", typeof(int)));
            dt.Columns.Add(new DataColumn("product", typeof(string)));
            dt.Columns.Add(new DataColumn("product2", typeof(string)));
            dt.Columns.Add(new DataColumn("product3", typeof(string)));
            dt.Columns.Add(new DataColumn("product4", typeof(string)));
            dt.Columns.Add(new DataColumn("product5", typeof(string)));
            dt.Columns.Add(new DataColumn("product6", typeof(string)));
            dt.Columns.Add(new DataColumn("price", typeof(int)));
            while (dr.Read())
            {
                var row = dt.NewRow();
                row["order_id"] = dr["order_id"];
                row["date"] = dr["date"];
                row["client"] = dr["client"];
                row["clerk"] = dr["clerk"];
                row["driver"] = dr["driver"];
                row["product"] = dr["product"];
                row["product2"] = dr["product2"];
                row["product3"] = dr["product3"];
                row["product4"] = dr["produc4"];
                row["product5"] = dr["product5"];
                row["product6"] = dr["product6"];
                row["price"] = dr["price"];
                dt.Rows.Add(row);
            }
            Console.WriteLine("All orders collected");
            PrintTable(dt);
            ncon.Close();
            dt.AcceptChanges();
        }
        
        public void displayUser(int entity, string order, string role)
        {
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT * from users WHERE entity_id="+entity+" ORDER BY "+ order +" ", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("user_id", typeof(int)));
            dt.Columns.Add(new DataColumn("firstName", typeof(string)));
            dt.Columns.Add(new DataColumn("lastName", typeof(string)));
            dt.Columns.Add(new DataColumn("phone", typeof(string)));
            dt.Columns.Add(new DataColumn("address", typeof(string)));
            dt.Columns.Add(new DataColumn("city", typeof(string)));
            dt.Columns.Add(new DataColumn("postalCode", typeof(string)));
            Console.WriteLine("Collecting data...");
            
            while (dr.Read())
            {
                var row = dt.NewRow();
                row["user_id"] = Convert.ToInt32(dr["user_id"]);
                row["firstName"] = dr["firstName"];
                row["lastName"] = dr["lastName"];
                row["phone"] = dr["phone"];
                row["address"] = dr["address"];
                row["city"] = dr["city"];
                row["postalCode"] = dr["postalCode"];
                dt.Rows.Add(row);
            }
                
            Console.WriteLine("The whole " + role + " ordered by " + order + ": ");
            PrintTable(dt);
            ncon.Close();
            dt.AcceptChanges();
        }

        public void menuDispUser()
        {
            Console.WriteLine("Do you want to display :");
            Console.WriteLine("1 : Client");
            Console.WriteLine("2 : Employee");
            var choice = Convert.ToInt32(Console.ReadLine());
            while (choice != 1 && choice != 2)
            {
                Console.WriteLine("Please enter a correct number");
                choice = Convert.ToInt32(Console.ReadLine());
            }

            if (choice == 1)
            {
                Console.WriteLine("How do you want to order ?");
                Console.WriteLine("1 : By city");
                Console.WriteLine("2 : By alphabetic order");
                choice = Convert.ToInt32(Console.ReadLine());
                while (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Please enter a correct number");
                    choice = Convert.ToInt32(Console.ReadLine());
                }

                if (choice == 1)
                {
                    displayUser(1,"city", "clients");
                }
                else
                {
                    displayUser(1,"alphabetic", "clients");
                }
            }
            else
            {
                Console.WriteLine("How do you want to order ?");
                Console.WriteLine("1 : By city");
                Console.WriteLine("2 : By alphabetic order");
                choice = Convert.ToInt32(Console.ReadLine());
                while (choice != 1 && choice != 2)
                {
                    Console.WriteLine("Please enter a correct number");
                    choice = Convert.ToInt32(Console.ReadLine());
                }

                if (choice == 1)
                {
                    displayUser(2,"city","employees");
                }
                else
                {
                    displayUser(2,"alphabetic","employees");
                }
            }
        }

        public int getAvgPrice()
        {
            var avgPrice = 0;
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT AVG(price) FROM orders", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                avgPrice = Convert.ToInt32(dr["avg"]);
            }
            ncon.Close();
            return avgPrice;
        }

        public int getAvgPriceClient()
        {
            var avgPrice = 0;
            var clientId = 0;
            Console.WriteLine("Please write the client's first name");
            var clientName = Console.ReadLine();
            var ncon = new NpgsqlConnection(PostgreConStr);
            var cmd = new NpgsqlCommand("SELECT user_id FROM users WHERE firstname = '" + clientName + "'", ncon);
            ncon.Open();
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                clientId = Convert.ToInt32(dr["user_id"].ToString());
            }
            ncon.Close();
            cmd = new NpgsqlCommand("SELECT AVG(price) FROM orders", ncon);
            ncon.Open();
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                avgPrice = Convert.ToInt32(dr["avg"]);
            }
            ncon.Close();
            return avgPrice;
        }

        public static void Main(string[] args)
        {
            var myApp = new App();
            bool closeApp = false;
            bool isConnected = false;
            while (!closeApp)
            {
                while (!isConnected)
                {
                    isConnected = myApp.connectClerk();
                }

                myApp.getMenu();
                Console.WriteLine("Please indicate your choice");
                Console.WriteLine("1: Create a new order");
                Console.WriteLine("2: Add a new user");
                Console.WriteLine("3: Change the availability of an item");
                Console.WriteLine("4: Stats module");
                Console.WriteLine("0: Exit the app");
                var choice = Convert.ToInt32(Console.ReadLine());
                while (choice != 1 && choice != 2 && choice != 3 && choice != 4 && choice != 0)
                {
                    Console.WriteLine("Please enter a correct value.");
                    choice = Convert.ToInt32(Console.ReadLine());
                }

                if (choice == 1)
                {
                    myApp.checkUser();
                    myApp.createOrder();
                }
                else if (choice == 2)
                {
                    myApp.addNewUser();
                }
                else if (choice == 3)
                {
                    myApp.changeAvailable();
                }
                else if (choice == 4)
                {
                    Console.WriteLine("Welcome to the stats module");
                    Console.WriteLine("Please indicate your choice");
                    Console.WriteLine("1: Get the number of orders taken by a clerk");
                    Console.WriteLine("2: Get the number of deliveries made by a driver");
                    Console.WriteLine("3: Get a list of the orders created between two dates");
                    Console.WriteLine("4: Get the average price of all the orders");
                    Console.WriteLine("5: Get the average price of the orders of a client");
                    var choice2 = Convert.ToInt32(Console.ReadLine());
                    switch (choice2)
                    {
                        case 1:
                            Console.WriteLine("Total commands: " + myApp.getClerkOrders());
                            break;
                        case 2:
                            Console.WriteLine("Total deliveries: " + myApp.getDeliveries());
                            break;
                        case 3:
                            myApp.getOrdersByDate();
                            break;
                        case 4:
                            Console.WriteLine("The average price of all orders is " + myApp.getAvgPrice());
                            break;
                        case 5:
                            Console.WriteLine("Average price of this client's orders is " + myApp.getAvgPriceClient());
                            break;
                        default:
                            break;
                    }
                }
                else if (choice == 0)
                {
                    closeApp = true;
                }
            }
        }

        static void PrintTable(DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                Console.Write(dc.ColumnName + "   ");
            }

            Console.WriteLine();
            Console.WriteLine();
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < dr.ItemArray.Length; i++)
                {
                    Console.Write(dr[i] + "\t");
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}