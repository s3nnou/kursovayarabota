using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PService
{
    public class DebtHandler
    {
        public static decimal getDebtCost(debt_table debt) => (decimal) debt.debt;

        public static decimal getTotalDebt (int index, int client_id, int month)
        {
            decimal total = 0;
            int currMonths = month - 1;
            while (getDebtRecord(index, client_id, currMonths) != null)
            {
                total += (decimal) getDebtRecord(index, client_id, currMonths).debt;

                currMonths--;
            }

            return total;
        }

        public static decimal DebtHandler1(int index, int client_id, int month)
        {
            if(ServiceDebtScout(getDebtRecord(index, client_id, month - 1)))
            {
                return getTotalDebt(index, client_id, month);
            }
            else
            {
               return -1;
            }
        }

        public static bool ServiceDebtScout(debt_table debt)
        {
            if (debt == null)
                return false;
            else
                return true;
        }

        public static debt_table getDebtRecord(int index, int client_id, int month)
        {
            using (PSEntity db = new PSEntity())
            {
                var debtRecord = db.debt_table.SingleOrDefault(dr => (dr.service_id == index + 1)
                                 && (dr.client_id == client_id)
                                 && (dr.month_id == month));

                return debtRecord;
            }
        }


        public static debt_table getDebtRecordById(int id)
        {
            using (PSEntity db = new PSEntity())
            {
                var result = db.debt_table.SingleOrDefault(b => b.id == id);
                return result;
            }
        }

        public static List<active_debts> getActiveDebtRecods(int client_id)
        {
            using (PSEntity db = new PSEntity())
            {
                var result = db.active_debts.Where(b => b.client_id == client_id).Select(b => new { b.id, b.debt_id, b.client_id }).ToList()
                    .Select(b => new active_debts() { id = b.id, client_id = client_id, debt_id = b.debt_id }).ToList();


                return result;
            }
        }

        public static active_debts GetActiveDebt(int debt_id)
        {
            using (PSEntity db = new PSEntity())
            {
                var result = db.active_debts.SingleOrDefault(b => b.debt_id == debt_id);



                return result;
            }
        }

        public static bool ActiveDebtScout(active_debts debt)
        {
            if (debt == null)
                return false;
            else
                return true;
        }

        public static decimal MainDebtHandler(int index, int client_id, int month)
        {
            List<active_debts> debts = getActiveDebtRecods(client_id);
            foreach (active_debts item in debts)
            {
                debt_table debt = getDebtRecordById((int)item.debt_id);

               if ((debt.service_id == index + 1) && (debt.month_id == month - 1))
               {
                    return (decimal) getDebtRecordById((int)item.debt_id).debt;
               }
            }

            return -1;
        }
    }
}
