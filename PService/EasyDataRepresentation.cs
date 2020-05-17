using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class EasyDataRepresentation
    {
        private int month;
        private decimal debt;

        public EasyDataRepresentation() { }

        public int Month { get => month; set => month = value; }
        public decimal Debt { get => debt; set => debt = value; }

        //
    }
}
