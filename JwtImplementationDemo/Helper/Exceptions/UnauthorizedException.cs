namespace JwtImplementationDemo.Helper.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string msg):base(msg) { }
    }
}
