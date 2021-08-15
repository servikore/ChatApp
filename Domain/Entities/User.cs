using System.Collections.Generic;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public string NickName { get; set; }
        public string Password { get; set; }
        public bool IsBlocked { get; set; }

        public List<Message> Messages { get; set; }
    }
}
