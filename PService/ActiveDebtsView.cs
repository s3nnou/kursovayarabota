using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class ActiveDebtsView
    {
        private int debt_id;
        private int client_id;

        private string service_id;

        private string month_id;

        private decimal debt;

        public int Debt_id { get => debt_id; set => debt_id = value; }
        public int Client_id { get => client_id; set => client_id = value; }
        public string Service_id { get => service_id; set => service_id = value; }
        public string Month_id { get => month_id; set => month_id = value; }
        public decimal Debt { get => debt; set => debt = value; }
    }
}
