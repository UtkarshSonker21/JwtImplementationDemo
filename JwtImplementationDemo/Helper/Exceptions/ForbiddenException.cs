﻿namespace JwtImplementationDemo.Helper.Exceptions
{
    public class ForbiddenException:Exception
    {
        public ForbiddenException(string msg) : base(msg) { }
    }
}
