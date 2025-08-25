namespace JwtImplementationDemo.Dto
{
    public static class DummyUsers
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            new UserModel
            {
                UserId = 101,
                UserName = "UtkarshSonker",
                Name = "Utkarsh",
                Role = "Admin",
                Password = "UtkarshSonker123",
                Contact = "9999999999"
            },
            new UserModel
            {
                UserId = 102,
                UserName = "RohitKumar",
                Name = "Rohit",
                Role = "EndUser",
                Password = "RohitKumar123",
                Contact = "8888888888"
            },
            new UserModel
            {
                UserId = 103,
                UserName = "NehaGupta",
                Name = "Neha",
                Role = "Manager",
                Password = "NehaGupta123",
                Contact = "7777777777"
            }
        };
    }
}
