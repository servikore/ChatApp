using System;

namespace Domain.Entities
{
    public class Message : BaseEntity
    {        
        public DateTime TimeStamp { get; set; }        
        public string Content { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}
