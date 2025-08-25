using System;
using System.Collections.Generic;

namespace JwtImplementationDemo.Entities;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Contact { get; set; } = null!;
}
