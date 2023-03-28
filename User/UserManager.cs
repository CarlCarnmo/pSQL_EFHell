using GBM.psqlDB;

namespace GBM.User
{
    internal class UserManager
    {

        public static User Create(psqlContext DB)
        {
            Console.WriteLine("Enter your name: ");
            string name = Console.ReadLine();
            bool nameExists = DB.Users.Any(u => u.Name == name);
            while (nameExists)
            {
                Console.WriteLine("Name already exists. Enter a new name: ");
                name = Console.ReadLine();
                nameExists = DB.Users.Any(u => u.Name == name);
            }

            Console.WriteLine("Enter your phone number: ");
            string phone = Console.ReadLine();

            Console.WriteLine("Enter your email: ");
            string email = Console.ReadLine();
            bool emailExists = DB.Users.Any(u => u.Email == email);
            while (emailExists)
            {
                Console.WriteLine("Email already exists. Enter a new email: ");
                email = Console.ReadLine();
                emailExists = DB.Users.Any(u => u.Email == email);
            }

            Console.WriteLine("Enter your choice (1 for Customer, 2 for Staff, 3 for Admin, 4 for Service): ");
            int choice = int.Parse(Console.ReadLine());

            User user;

            switch (choice)
            {
                case 1:
                    user = new Customer();
                    break;
                case 2:
                    user = new Staff();
                    break;
                case 3:
                    user = new Admin();
                    break;
                case 4:
                    user = new Service();
                    break;
                default:
                    throw new ArgumentException("Invalid user type.");
            }
            user.Name = name;
            user.Phone = phone;
            user.Email = email;
            user.UserType = choice == 1 ? "Customer" : choice == 2 ? "Staff" : choice == 3 ? "Admin" : "Service";

            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();
            user.SaltAndHashString = PasswordHasher.PasswordHasher.HashPassword(password);

            DB.Add(user);
            DB.SaveChanges();

            Console.WriteLine($"User created: {user.Name} ({user.UserType})");

            return user;
        }

        public static User Login(psqlContext DB)
        {
            Console.WriteLine("Enter your email or name: ");
            string identifier = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();

            User user = null;

            // Check if identifier is email or name
            if (identifier.Contains("@"))
            {
                user = DB.Users.FirstOrDefault(u => u.Email == identifier);
            }
            else
            {
                user = DB.Users.FirstOrDefault(u => u.Name == identifier);
            }

            // Handle invalid email or name
            if (user == null)
            {
                Console.WriteLine("Invalid email or name. Please try again.");
                return null;
            }

            // Check if password is correct
            if (!PasswordHasher.PasswordHasher.VerifyPasswordHash(password, user.SaltAndHashString))
            {
                Console.WriteLine("Incorrect password. Please try again.");
                return null;
            }

            // Return the logged-in user
            Console.WriteLine($"Logged in as {user.Name} ({user.UserType})");
            return user;
        }

        public static bool RemoveUser(psqlContext DB)
        {
            Console.WriteLine("Enter your email or name: ");
            string identifier = Console.ReadLine();
            Console.WriteLine("Enter your password: ");
            string password = Console.ReadLine();

            User user = null;

            // Check if identifier is email or name
            if (identifier.Contains("@"))
            {
                user = DB.Users.FirstOrDefault(u => u.Email == identifier);
            }
            else
            {
                user = DB.Users.FirstOrDefault(u => u.Name == identifier);
            }

            // Handle invalid email or name
            if (user == null)
            {
                Console.WriteLine("Invalid email or name. Please try again.");
                return false;
            }

            // Check if password is correct
            if (!PasswordHasher.PasswordHasher.VerifyPasswordHash(password, user.SaltAndHashString))
            {
                Console.WriteLine("Incorrect password. Please try again.");
                return false;
            }

            // Remove the user from the database
            DB.Users.Remove(user);
            DB.SaveChanges();

            Console.WriteLine($"User {user.Name} ({user.UserType}) has been removed from the database.");
            return true;
        }
    }


}
