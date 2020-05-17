using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PService
{
    public class PaymentHandler
    {
        public static decimal getServiceCost(int index, int client_id, int month) => ServicesHandler.getServiceUsage(index, client_id, month) * ServicesHandler.getServiceTariff(index);

        public static decimal getStaticServiceCost(int index, int client_id, decimal dwelling_space) => dwelling_space * ServicesHandler.getServiceTariff(index);

        public static virtual_money getVirtualMoneyForService(int index, int client_id)
        {
            using (PSEntity db = new PSEntity())
            {
                var virtualMoney = from vm in db.virtual_money
                                  where (vm.service_id == index + 1)
                                  && (vm.client_id == client_id)
                                  select vm;

                return virtualMoney.FirstOrDefault<virtual_money>();
            }
        }

        public static decimal getVirtualMoneyInCash(int index, int client_id)
        {
            decimal result;

            if(getVirtualMoneyForService(index, client_id) == null)
            {
                return -1;
            }

            else
            {
                decimal.TryParse(getVirtualMoneyForService(index, client_id).money.ToString(), out result);
                return result;

            }
        }

        public static decimal TotalToPay(int index, int client_id, int month)
        {
            decimal realmoney = getServiceCost(index, client_id, month);
            decimal virtualmoney = getVirtualMoneyInCash(index, client_id);
            decimal debt = DebtHandler.MainDebtHandler(index, client_id, month);

            if (virtualmoney < 0)
            {
                virtualmoney = 0;
            }

            if (debt < 0)
            {
                debt = 0;
            }

            return Math.Abs(realmoney - virtualmoney - debt);
        }

        public static decimal TotalToPay(int index, int client_id, decimal dwelling_space)
        {
            decimal realmoney = getStaticServiceCost(index, client_id, dwelling_space);
            decimal virtualmoney = getVirtualMoneyInCash(index, client_id);

            if (virtualmoney < 0)
            {
                virtualmoney = 0;
            }

            return realmoney - virtualmoney;
        }

        public static void PaymentStateMachine(int index, int client_id, int month, decimal insertedMoney, decimal virtualMoney, decimal debtMoney, int state, decimal totalFee)
        {

            if (insertedMoney > totalFee)
            {
                AbovePay(index, client_id, insertedMoney, Math.Abs(totalFee - insertedMoney), month, state);
            }

            else if (insertedMoney == totalFee)
            {
                OrdinaryPay(index, client_id, insertedMoney, month, state);
            }

            else if (insertedMoney < totalFee)
            {
                BelowPay(index, client_id, insertedMoney, virtualMoney, debtMoney, state, totalFee, month);
            }
        }

        public static int GetPaymentState(decimal virtualMoney, decimal debtMoney)
        {
            int state = 0;

            if ((virtualMoney < 0) && (debtMoney < 0)) 
            {
                state = 1;
            }

            else if ((virtualMoney < 0))
            {
                state = 2;
            }

            else if ((debtMoney < 0))
            {
                state = 3;
            }

            else
            {
                state = 4;
            }

            return state;
        }

        public static void AbovePay(int index, int client_id, decimal insertedMoney, decimal change, int month, int state)
        {
            using (PSEntity db = new PSEntity())
            {
                CreatePaymentEntry(index, client_id, insertedMoney);

                switch (state)
                {
                    case (int)MoneyStates.NODEBTNOVIRTUALMONEY:
                        {
                            MessageBox.Show("проходи");
                            CreateVirtualMoneyEntry(index, client_id, Math.Abs(change));
                            break;
                        }


                    case (int)MoneyStates.NOVIRTUALMONEY:
                        {
                            CreateVirtualMoneyEntry(index, client_id, Math.Abs(change));
                            db.Entry(DebtHandler.GetActiveDebt(DebtHandler.getDebtRecord(index, client_id, month - 1).id)).State = EntityState.Deleted;
                            break;
                        }

                    case (int)MoneyStates.NODEBTMONEY:
                        {
                            var result = db.virtual_money.SingleOrDefault(b => b.client_id == client_id && b.service_id == index + 1);
                            result.money = change;
                            db.SaveChanges();
                            break;
                        }

                    case (int)MoneyStates.ALLMONEYPRESENT:
                        {
                            var result = db.virtual_money.SingleOrDefault(b => b.client_id == client_id && b.service_id == index + 1);
                            result.money = change;
                            db.SaveChanges();

                            db.Entry(DebtHandler.GetActiveDebt(DebtHandler.getDebtRecord(index, client_id, month - 1).id)).State = EntityState.Deleted;


                            break;
                        }
                }

                db.SaveChanges();
            }
        }

        public static void OrdinaryPay(int index, int client_id, decimal insertedMoney, int month, int state)
        {
            using (PSEntity db = new PSEntity())
            {
                CreatePaymentEntry(index, client_id, insertedMoney);

                switch (state)
                {
                    case (int)MoneyStates.NODEBTNOVIRTUALMONEY:
                        {
                            MessageBox.Show("проходи");
                            break;
                        }

                    case (int)MoneyStates.NOVIRTUALMONEY:
                        {
                            db.Entry(DebtHandler.GetActiveDebt(DebtHandler.getDebtRecord(index, client_id, month - 1).id)).State = EntityState.Deleted;

                            break;
                        }

                    case (int)MoneyStates.NODEBTMONEY:
                        {
                            db.Entry(getVirtualMoneyForService(index, client_id)).State = EntityState.Deleted;
                            break;
                        }

                    case (int)MoneyStates.ALLMONEYPRESENT:
                        {
                            db.Entry(getVirtualMoneyForService(index, client_id)).State = EntityState.Deleted;
                            db.Entry(DebtHandler.GetActiveDebt(DebtHandler.getDebtRecord(index, client_id, month - 1).id)).State = EntityState.Deleted;

                            break;
                        }
                }

                db.SaveChanges();
            }
        }

        public static void BelowPay(int index, int client_id, decimal insertedMoney, decimal virtualMoney, decimal debtMoney, int state, decimal totalFee, int month)
        {
            using (PSEntity db = new PSEntity())
            {
                CreatePaymentEntry(index, client_id, insertedMoney);

                switch (state)
                {
                    case (int)MoneyStates.NODEBTNOVIRTUALMONEY:
                        {
                            MessageBox.Show("проходи");
                            CreateDebtEntry(index, client_id, month, Math.Abs(totalFee - insertedMoney));
                            CreateActiveDebtEntry(DebtHandler.getDebtRecord(index, client_id, month).id, client_id);
                            break;
                        }

                    case (int)MoneyStates.NOVIRTUALMONEY:
                        {
                            CreateDebtEntry(index, client_id, month, Math.Abs(totalFee - insertedMoney - debtMoney));

                            var result = db.active_debts.SingleOrDefault(b => b.id == DebtHandler.getDebtRecord(index, client_id, month - 1).id);

                            result.debt_id = DebtHandler.getDebtRecord(index, client_id, month).id;

                            break;
                        }

                    case (int)MoneyStates.NODEBTMONEY:
                        {
                            db.Entry(getVirtualMoneyForService(index, client_id)).State = EntityState.Deleted;

                            CreateDebtEntry(index, client_id, month, Math.Abs(totalFee - insertedMoney - virtualMoney));

                            CreateActiveDebtEntry(DebtHandler.getDebtRecord(index, client_id, month).id, client_id);
                            break;
                        }

                    case (int)MoneyStates.ALLMONEYPRESENT:
                        {
                            db.Entry(getVirtualMoneyForService(index, client_id)).State = EntityState.Deleted;

                            CreateDebtEntry(index, client_id, month, Math.Abs(totalFee - insertedMoney - debtMoney));

                            var result = db.active_debts.SingleOrDefault(b => b.id == DebtHandler.getDebtRecord(index, client_id, month - 1).id);

                            result.debt_id = DebtHandler.getDebtRecord(index, client_id, month).id;

                            break;
                        }
                }

                db.SaveChanges();
            }
        }

        public static void CreatePaymentEntry(int index, int client_id, decimal insertedMoney)
        {
            using (PSEntity db = new PSEntity())
            {
                db.service_payments.Add(new service_payments
                {
                    id = db.service_payments.Count() + 1,
                    client_id = client_id,
                    service_id = index + 1,
                    usage = 100,
                    date = Misc.GetTodayMonth(),
                    price = insertedMoney

                });

                db.SaveChanges();
            }
        }

        public static void CreateVirtualMoneyEntry(int index, int client_id, decimal change)
        {
            using (PSEntity db = new PSEntity())
            {
                db.virtual_money.Add(new virtual_money
                {
                    id = db.service_payments.Count() + 1,
                    client_id = client_id,
                    service_id = index + 1,
                    money = change

                });

                db.SaveChanges();
            }
        }

        public static void CreateDebtEntry(int index, int client_id, int month, decimal money)
        {
            using (PSEntity db = new PSEntity())
            {
                db.debt_table.Add(new debt_table
                {
                    id = db.debt_table.Count() + 1,
                    client_id = client_id,
                    service_id = index + 1,
                    debt = money,
                    month_id = month
                });

                db.SaveChanges();
            }
        }

        public static void CreateActiveDebtEntry(int debt_id, int client_id)
        {
            using (PSEntity db = new PSEntity())
            {
                db.active_debts.Add(new active_debts
                {
                    id = db.active_debts.Count() + 1,
                    client_id = client_id,
                    debt_id = debt_id
                });

                db.SaveChanges();
            }
        }
    }
}
