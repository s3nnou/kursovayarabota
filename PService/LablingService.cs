using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class LablingService
    {
        public static void GetTextAboutServiceWithMetrics(int index,
            int client_id, int month, out string label25, out string label27,
            out string label28, out string label30, out string label32, out string label34)
        {
            label25 = ServicesHandler
                .getServiceTariff(index)
                .ToString();

            label27 = ServicesHandler
                .getServiceUsage(index, client_id, month)
                .ToString();

            label28 = PaymentHandler
                .getServiceCost(index, client_id, month)
                .ToString();

            label30 = PaymentHandler
                .getVirtualMoneyInCash(index, client_id)
                .ToString();

            label32 = PaymentHandler
                .TotalToPay(index, client_id, month)
                .ToString();

            label34 = DebtHandler
                .MainDebtHandler(index, client_id, month)
                .ToString();

        }

        public static void SetTextForStaticService(int index,
            int client_id, int month, decimal dwelling_space, out string label25, out string label27,
            out string label28, out string label30, out string label32, out string label34)
        {
            label25 = ServicesHandler
               .getServiceTariff(index)
               .ToString();

            label27 = "none";

            label28 = PaymentHandler
               .getStaticServiceCost(index, client_id, dwelling_space)
               .ToString();

            label30 = PaymentHandler
               .getVirtualMoneyInCash(index, client_id)
               .ToString();

            label32 = PaymentHandler
                .TotalToPay(index, client_id, dwelling_space)
                .ToString();

            label34 = DebtHandler
            .MainDebtHandler(index, client_id, month)
            .ToString();

        }
    }
}
