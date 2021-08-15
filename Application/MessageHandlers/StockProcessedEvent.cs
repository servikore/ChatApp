using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MessageService
{
    public class StockProcessedEvent : IntegrationEvent
    {
        public override string Topic => "STOCK";
        public string FromUser { get; set; }
        public string ToUser { get; set; }
        public string ChatMessage { get; set; }

    }
}
