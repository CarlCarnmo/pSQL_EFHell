using GBM.PasswordHasher;
using GBM.psqlDB;
using GBM.User;

namespace pSQL_GBM.Tests
{
    internal class Testmethods
    {
        public static void ConsoleMenu()
        {
            var db = new psqlContext();
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Create user");
                Console.WriteLine("2. Log in");
                Console.WriteLine("3. Yeet a user");
                Console.WriteLine("4. Add example users (10 of each type)");
                Console.WriteLine("5. List users by type");
                Console.WriteLine("0. Exit");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        UserManager.Create(db);
                        break;
                    case "2":
                         User user = UserManager.Login(db);
                        Console.WriteLine($"Logged in as: {user?.ToString()}");
                        break;
                    case "3":
                        UserManager.RemoveUser(db);
                        break;
                    case "4":
                        AddExampleUsers();
                        break;
                    case "5":
                        ListUsersByType();
                        break;
                    case "0":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }

                Console.WriteLine();
            }
        }

        public static void Ftest()
        {
            var db = new psqlContext();
            // Create a new user and add it to the database
            var user = new User
            {
                Name = "John Smith",
                Email = "john.smith@example.com",
                Phone = "555-1234",
                UserType = "Customer",
                SaltAndHashString = "SALT_AND_HASH_HERE"
            };
            db.Users.Add(user);
            db.SaveChanges();

            // Retrieve the user from the database and print its properties
            var retrievedUser = db.Users.Find(user.Id);
            System.Console.WriteLine($"Name: {retrievedUser.Name}");
            System.Console.WriteLine($"Email: {retrievedUser.Email}");
            System.Console.WriteLine($"Phone: {retrievedUser.Phone}");
            System.Console.WriteLine($"UserType: {retrievedUser.UserType}");
            System.Console.WriteLine($"SaltAndHashString: {retrievedUser.SaltAndHashString}");

            // Update the user and save changes to the database
            retrievedUser.Phone = "555-4321";
            db.SaveChanges();

            // Retrieve the user from the database again and print its updated phone number
            var updatedUser = db.Users.Find(user.Id);
            System.Console.WriteLine($"Updated phone: {updatedUser.Phone}");

            // Remove the user from the database
            db.Users.Remove(updatedUser);
            db.SaveChanges();

            // Make sure the user has been removed from the database
            var removedUser = db.Users.Find(updatedUser.Id);
            if (removedUser == null)
            {
                System.Console.WriteLine("User removed successfully");
            }
            else
            {
                System.Console.WriteLine("User removal failed");
            }
        }

        private static void AddExampleUsers()
        {
            var db = new psqlContext();
            for (int i = 1; i <= 10; i++)
            {
                string name = $"Customer{i}";
                string email = $"customer{i}@example.com";
                string phone = $"123456789{i}";
                string password = $"password{i}";

                var customer = new Customer
                {
                    Name = name,
                    Email = email,
                    Phone = phone,
                    UserType = "Customer",
                    SaltAndHashString = PasswordHasher.HashPassword(password)
                };
                db.Customers.Add(customer);

                var staff = new Staff
                {
                    Name = $"Staff{i}",
                    Email = $"staff{i}@example.com",
                    Phone = $"234567890{i}",
                    UserType = "Staff",
                    SaltAndHashString = PasswordHasher.HashPassword($"password{i}")
                };
                db.Staffs.Add(staff);

                var admin = new Admin
                {
                    Name = $"Admin{i}",
                    Email = $"admin{i}@example.com",
                    Phone = $"345678901{i}",
                    UserType = "Admin",
                    SaltAndHashString = PasswordHasher.HashPassword($"password{i}")
                };
                db.Admins.Add(admin);

                var service = new Service
                {
                    Name = $"Service{i}",
                    Email = $"service{i}@example.com",
                    Phone = $"456789012{i}",
                    UserType = "Service",
                    SaltAndHashString = PasswordHasher.HashPassword($"password{i}")
                };
                db.Services.Add(service);
            }

            db.SaveChanges();
            Console.WriteLine("Example users added to database.");
        }

        private static void ListUsersByType()
        {
            var db = new psqlContext();

            Console.WriteLine("Enter the user type (Customer, Staff, Admin or Service):");
            string userType = Console.ReadLine();

            var users = db.Users
                .Where(u => u.UserType == userType)
                .ToList();

            if (users.Count == 0)
            {
                Console.WriteLine($"No {userType} users found.");
            }
            else
            {
                Console.WriteLine($"List of {userType} users:");

                foreach (var user in users)
                {
                    Console.WriteLine($"- {user.Name} ({user.Email}, {user.Phone})");
                }
            }
        }
    }
}
