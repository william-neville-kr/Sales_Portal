using Newtonsoft.Json;
using pokitdokcsharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhysicianPortal.Core.Helpers;

namespace PhysicianPortal.PokitDokIntegration.Models
{
    public class EligibilityRecord
    {
        public PokitDokModel RetrieveData(string birthDate, string memFirstName, string memLastName, string npi, string prodFirstName, string prodLastName, string tradingPartnerId)
        {
            // member
            //{"id", "W000000000"},
            //{"birth_date", "1970-01-01"},
            //{"first_name", "Jane"},
            //{"last_name", "Doe"}

            // provider
            //{"npi", "1467560003"},
            //{"last_name", "AYA-AY"},
            //{"first_name", "JEROME"}

            //
            //{"trading_partner_id", "MOCKPAYER"}

            PokitDokModel pokitDokModel = null;
            try
            {
                var client = new PlatformClient("Rz2zrn9OLVz6CDKf7W7L", "GTqNQVuA0OnhKbJxnslZnwoR2wCfQwD53F6BywFe");

                var tradingPartnersList = client.tradingPartners();

                var resp = client.eligibility(
                    new Dictionary<string, object>
                    {
                        {
                            "member", new Dictionary<string, object>
                            {
                                {"birth_date", birthDate},
                                {"first_name", memFirstName},
                                {"last_name", memLastName}
                            }
                        },
                        {
                            "provider", new Dictionary<string, object>
                            {
                                {"npi", npi},
                                {"last_name", prodLastName},
                                {"first_name", prodFirstName}
                            }
                        },
                        {"service_types", new[] {"health_benefit_plan_coverage"}},
                        {"trading_partner_id", tradingPartnerId}
                    });

                if (resp.status_code == 200) // this is the HTTP status code
                {
                    string str = Convert.ToString(client.Data);
                    pokitDokModel = JsonConvert.DeserializeObject<PokitDokModel>(str);
                }
            }
            catch (Exception exception)
            {
                Logger.InsertLog("Exception thrown in Method RetrieveData EligibilityRecord class. " + exception.Message, exception);
            }
            return pokitDokModel;
        }
    }
}
