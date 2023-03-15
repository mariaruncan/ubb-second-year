using System;

namespace Services
{
    public class MyException : Exception
    {
        public MyException(string msg): base(msg) { }
    }
}
