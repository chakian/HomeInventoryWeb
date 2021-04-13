using System;

namespace HomeInv.Common.Exceptions
{
    public class HICosmosException : Exception
    {
        public HICosmosException() : this("Bir hata oluştu") { }
        
        public HICosmosException(string message) : this(message, null) { }
        
        public HICosmosException(string message, Exception innerException) : base($"Cosmos db hatası: {message}", innerException) { }
    }
}
