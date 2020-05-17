using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PService
{
    public class ServicesHandler
    {
        public static service_type getSelectedService(int index)
        {
            using (PSEntity db = new PSEntity())
            {
                var serviceType = from st in db.service_type
                                  where st.service_id == index + 1
                                  select st;

                return serviceType.FirstOrDefault<service_type>();
            }
        }

        public static service_metrcis getSelectedServiceMetrics(int index, int client_id, int month)
        {
            using (PSEntity db = new PSEntity())
            {
                var servicesMetrics = from sc in db.service_metrcis
                                      where (sc.client_id == client_id)
                                      && (sc.service_id == index + 1)
                                      && (sc.last_checkup == month)
                                      select sc;

                return servicesMetrics.FirstOrDefault<service_metrcis>();
            }
        }

        public static decimal getServiceUsage(int index, int client_id, int month) => decimal.Parse(getSelectedServiceMetrics(index, client_id, month).curr_month.ToString()) - decimal.Parse(getSelectedServiceMetrics(index, client_id, month).prev_month.ToString());

        public static decimal getServiceTariff(int index) => decimal.Parse(getSelectedService(index).tariff.ToString());

        public static void CreateServiceMetrics(int index, int client_id, int currMonth)
        {
            metrics_archive prevStateSelector = GetServiceMetricsArchive(index, client_id, currMonth - 1);

            metrics_archive currStateSelector = GetServiceMetricsArchive(index, client_id, currMonth);

            using (PSEntity db = new PSEntity())
            {
                db.service_metrcis.Add(new service_metrcis
                {
                    id = db.service_metrcis.Count() + 1,
                    service_id = index + 1,
                    client_id = client_id,
                    prev_month = prevStateSelector.usage,
                    curr_month = currStateSelector.usage,
                    last_checkup = currMonth
                    
                });

                db.SaveChanges();
            }
        }

        public static metrics_archive GetServiceMetricsArchive(int index, int client_id, int months)
        {
            using (PSEntity db = new PSEntity())
            {
                var selector = from pm in db.metrics_archive
                               where (pm.month_id == months)
                               && (pm.service_id == index + 1)
                               && (pm.client_id == client_id)
                               select pm;

                return selector.FirstOrDefault<metrics_archive>();
            }   
        }

        public static bool ScoutForSelectedServiceMetrics(int index, int client_id, int currMonth)
        {
            if (getSelectedServiceMetrics(index, client_id, currMonth) == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

      
    }
}
