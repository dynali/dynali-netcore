using System;

namespace Dynali
{
    public class DynaliException : Exception
    {
        public DynaliException(int code, string message) : base("[" + code.ToString() + "] " + message)
        {

        }
    }
}
