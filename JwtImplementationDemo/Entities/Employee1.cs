using System;
using System.Collections.Generic;

namespace JwtImplementationDemo.Entities;

public partial class Employee1
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Department { get; set; } = null!;

    public string Salary { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateOnly Joindate { get; set; }
}
