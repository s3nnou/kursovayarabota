using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class EasyDebtRecordsRepresentation
    {
        private string client_name;
        private string service_name;
        private string date_name;
        private decimal price;

        public EasyDebtRecordsRepresentation() { }

        public string Client_name { get => client_name; set => client_name = value; }
        public string Service_name { get => service_name; set => service_name = value; }
        public string Date_name { get => date_name; set => date_name = value; }
        public decimal Price { get => price; set => price = value; }
    }
}
