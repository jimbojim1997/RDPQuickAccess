using System;

namespace RDPManager.Utilities
{
    class InvalidTypeException : Exception
    {
        private readonly string _message;
        public override string Message
        {
            get
            {
                return _message;
            }
        }
        public InvalidTypeException(string message)
        {
            _message = message;
        }
    }
}
