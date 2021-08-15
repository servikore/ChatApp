using Domain.Entities;
using System;

namespace Domain.Exceptions
{
    public class InvalidUserException : Exception
    {
        public User User { get; set; }
    }
}
