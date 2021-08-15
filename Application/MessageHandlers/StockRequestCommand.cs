using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.MessageService
{
    public class StockRequestCommand : IntegrationCommand
    {        
        public string StockCode { get; set; }
        public string User { get; set; }
    }
}
