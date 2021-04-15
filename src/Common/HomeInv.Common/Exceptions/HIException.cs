using System;

namespace HomeInv.Common.Exceptions
{
    public class HIException : Exception
    {
        public HIException() : this("Bir hata oluştu") { }
        
        public HIException(string message) : this(message, null) { }
        
        public HIException(string message, Exception innerException) : base($"Bir hata oluştu. {message}", innerException) { }
    }
}
