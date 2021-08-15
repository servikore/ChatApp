using System;

namespace Domain.Exceptions
{
    public class InvalidSockCodeException : Exception
    {
        public string StockCode { get; set; }
    }
}
