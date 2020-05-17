using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PService
{
    public class ServicePayRequests
    {
        public static bool MainServiceDataProvider(int index, int client_id, int currMonth, decimal dwelling_space)
        {
            if (index + 1 < 5)
            {
                if (ServicesHandler.ScoutForSelectedServiceMetrics(index, client_id, currMonth))
                {
                    return true;
                }
                else
                {
                    ServicesHandler.CreateServiceMetrics(index, client_id, currMonth);
                }

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
