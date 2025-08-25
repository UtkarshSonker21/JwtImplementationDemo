using System;
using System.Collections.Generic;

namespace JwtImplementationDemo.Entities;

public partial class Student
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int Age { get; set; }

    public int Grade { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Password { get; set; } = null!;
}
