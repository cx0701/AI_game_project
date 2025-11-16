using System;

namespace Glitch9.AIDevKit.Google
{
    public class BrokenResponseException : Exception
    {
        public BrokenResponseException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}