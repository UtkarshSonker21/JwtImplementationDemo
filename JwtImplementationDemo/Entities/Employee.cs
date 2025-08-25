using System;
using System.Collections.Generic;

namespace JwtImplementationDemo.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public int Age { get; set; }

    public int Salary { get; set; }

    public string City { get; set; } = null!;
}
