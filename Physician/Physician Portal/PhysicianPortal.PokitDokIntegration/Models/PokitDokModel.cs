using System;
using System.Collections.Generic;

namespace PhysicianPortal.PokitDokIntegration.Models
{
    public class Message
    {
        public string message { get; set; }
    }

    public class Coinsurance
    {
        public List<Message> messages { get; set; }
        public string plan_description { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public double benefit_percent { get; set; }
        public string in_plan_network { get; set; }
        public string coverage_level { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public List<string> address_lines { get; set; }
    }

    public class Contact
    {
        public string contact_type { get; set; }
        public string name { get; set; }
        public Address address { get; set; }
    }

    public class Message2
    {
        public string message { get; set; }
    }

    public class Copayment
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Copay
    {
        public List<Message2> messages { get; set; }
        public string plan_description { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public string in_plan_network { get; set; }
        public string coverage_level { get; set; }
        public Copayment copayment { get; set; }
    }

    public class Message3
    {
        public string message { get; set; }
    }

    public class BenefitAmount
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Deductible
    {
        public List<Message3> messages { get; set; }
        public string eligibility_date { get; set; }
        public string plan_description { get; set; }
        public string in_plan_network { get; set; }
        public string coverage_level { get; set; }
        public string time_period { get; set; }
        public BenefitAmount benefit_amount { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
    }

    public class BenefitAmount2
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class OutOfPocket
    {
        public string plan_description { get; set; }
        public string in_plan_network { get; set; }
        public string coverage_level { get; set; }
        public BenefitAmount2 benefit_amount { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public string time_period { get; set; }
    }

    public class Message4
    {
        public string message { get; set; }
    }

    public class Limitation
    {
        public List<Message4> messages { get; set; }
        public string time_period_qualifier { get; set; }
        public string plan_description { get; set; }
        public string coverage_level { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public string in_plan_network { get; set; }
    }

    public class Plan
    {
        public string plan_description { get; set; }
        public string plan_number { get; set; }
        public string group_description { get; set; }
        public string group_number { get; set; }
        public string insurance_type { get; set; }
    }

    public class Coverage
    {
        public string eligibility_begin_date { get; set; }
        public string plan_begin_date { get; set; }
        public string service_date { get; set; }
        public string insurance_type { get; set; }
        public string plan_description { get; set; }
        public bool active { get; set; }
        public string plan_number { get; set; }
        public string group_number { get; set; }
        public string group_description { get; set; }
        public List<Coinsurance> coinsurance { get; set; }
        public List<Contact> contacts { get; set; }
        public List<Copay> copay { get; set; }
        public List<Deductible> deductibles { get; set; }
        public List<OutOfPocket> out_of_pocket { get; set; }
        public List<Limitation> limitations { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public string level { get; set; }
        public List<Plan> plans { get; set; }
    }

    public class Payer
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Provider
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string npi { get; set; }
    }

    public class Address2
    {
        public string city { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public List<string> address_lines { get; set; }
    }

    public class Subscriber
    {
        public Address2 address { get; set; }
        public string birth_date { get; set; }
        public string gender { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string id { get; set; }
        public string group_number { get; set; }
    }

    public class Limit
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class InNetwork
    {
        public Limit limit { get; set; }
        public Applied applied { get; set; }
        public Remaining remaining { get; set; }
    }

    public class Limit2
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied2
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining2
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class OutOfNetwork
    {
        public Limit2 limit { get; set; }
        public Applied2 applied { get; set; }
        public Remaining2 remaining { get; set; }
    }

    public class Family
    {
        public InNetwork in_network { get; set; }
        public OutOfNetwork out_of_network { get; set; }
    }

    public class Limit3
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied3
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining3
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class InNetwork2
    {
        public Limit3 limit { get; set; }
        public Applied3 applied { get; set; }
        public Remaining3 remaining { get; set; }
    }

    public class Limit4
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied4
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining4
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class OutOfNetwork2
    {
        public Limit4 limit { get; set; }
        public Applied4 applied { get; set; }
        public Remaining4 remaining { get; set; }
    }

    public class Individual
    {
        public InNetwork2 in_network { get; set; }
        public OutOfNetwork2 out_of_network { get; set; }
    }

    public class Deductible2
    {
        public Family family { get; set; }
        public Individual individual { get; set; }
    }

    public class Limit5
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied5
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining5
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class InNetwork3
    {
        public Limit5 limit { get; set; }
        public Applied5 applied { get; set; }
        public Remaining5 remaining { get; set; }
    }

    public class Limit6
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied6
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining6
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class OutOfNetwork3
    {
        public Limit6 limit { get; set; }
        public Applied6 applied { get; set; }
        public Remaining6 remaining { get; set; }
    }

    public class Family2
    {
        public InNetwork3 in_network { get; set; }
        public OutOfNetwork3 out_of_network { get; set; }
    }

    public class Limit7
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied7
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining7
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class InNetwork4
    {
        public Limit7 limit { get; set; }
        public Applied7 applied { get; set; }
        public Remaining7 remaining { get; set; }
    }

    public class Limit8
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Applied8
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class Remaining8
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class OutOfNetwork4
    {
        public Limit8 limit { get; set; }
        public Applied8 applied { get; set; }
        public Remaining8 remaining { get; set; }
    }

    public class Individual2
    {
        public InNetwork4 in_network { get; set; }
        public OutOfNetwork4 out_of_network { get; set; }
    }

    public class OutOfPocket2
    {
        public Family2 family { get; set; }
        public Individual2 individual { get; set; }
    }

    public class Summary
    {
        public Deductible2 deductible { get; set; }
        public OutOfPocket2 out_of_pocket { get; set; }
    }

    public class Pharmacy
    {
        public bool is_eligible { get; set; }
    }

    public class Address3
    {
        public string city { get; set; }
        public string zipcode { get; set; }
        public string state { get; set; }
        public List<string> address_lines { get; set; }
    }

    public class BenefitAmount3
    {
        public string amount { get; set; }
        public string Amount_C
        {
            get
            {
                return String.Format("{0:C}", !string.IsNullOrEmpty(amount) ? decimal.Parse(amount) : 0.0m);
            }
        }
        public string currency { get; set; }
    }

    public class BenefitRelatedEntity
    {
        public string entity_identifier_code { get; set; }
        public string entity_type { get; set; }
        public string organization_name { get; set; }
        public Address3 address { get; set; }
        public string eligibility_or_benefit_information { get; set; }
        public BenefitAmount3 benefit_amount { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
    }

    public class PokitDokModel
    {
        public string client_id { get; set; }
        public string correlation_id { get; set; }
        public Coverage coverage { get; set; }
        public Payer payer { get; set; }
        public Provider provider { get; set; }
        public Subscriber subscriber { get; set; }
        public bool valid_request { get; set; }
        public string trading_partner_id { get; set; }
        public List<string> service_types { get; set; }
        public List<string> service_type_codes { get; set; }
        public Summary summary { get; set; }
        public Pharmacy pharmacy { get; set; }
        public string originating_company_id { get; set; }
        public string trace_number { get; set; }
        public List<BenefitRelatedEntity> benefit_related_entities { get; set; }
    }
}
