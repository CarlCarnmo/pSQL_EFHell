
namespace GBM.User
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string UserType { get; set; }
        public string SaltAndHashString { get; set; }
    }

    internal class Staff : User
    {

    }

    internal class Admin : User
    {

    }

    internal class Service : User
    {

    }

    internal class Customer : User
    {

    }
}