using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trulioo.Client.V1;
using Trulioo.Client.V1.Model;

namespace PhysicianPortal.Trulioo
{
    public class TruliooVerifyUser
    {
        public async Task TestApi()
        {
            var truliooClient = new TruliooApiClient("ITLink_Demo_API", "*Tobasco2014");

            var response = await truliooClient.Connection.TestAuthenticationAsync();

            //var responseString = response.Result; //get result of the request
        }
        public async Task TestApi2()
        {
            var truliooClient = new TruliooApiClient("ITLink_Demo_API", "*Tobasco2014");

            VerifyRequest verifyRequest = new VerifyRequest();

            verifyRequest.AcceptTruliooTermsAndConditions = true;
            //verifyRequest.Demo = false;
            verifyRequest.CleansedAddress = false;
            verifyRequest.ConfigurationName = "Identity Verification";
            //verifyRequest.ConsentForDataSources = new string[] { "DataSource Name 1", "DataSource Name 2"};
            verifyRequest.CountryCode = "US";

            verifyRequest.DataFields = new DataFields();
            verifyRequest.DataFields.PersonInfo = new PersonInfo();
            verifyRequest.DataFields.Location = new Location();
            verifyRequest.DataFields.Communication = new Communication();
            verifyRequest.DataFields.DriverLicence = new DriverLicence();
            //verifyRequest.DataFields.NationalIds = new NationalId();
            verifyRequest.DataFields.Passport = new Passport();
            //verifyRequest.DataFields.CountrySpecific = new CountrySpecific();

            //PersonInfo
            verifyRequest.DataFields.PersonInfo.FirstGivenName = "Justin";
            verifyRequest.DataFields.PersonInfo.MiddleName = "Mark";
            verifyRequest.DataFields.PersonInfo.FirstSurName = "Williams";
            //verifyRequest.DataFields.PersonInfo.SecondSurname = "Williams";
            verifyRequest.DataFields.PersonInfo.ISOLatin1Name = "";
            verifyRequest.DataFields.PersonInfo.DayOfBirth = 4;
            verifyRequest.DataFields.PersonInfo.MonthOfBirth = 8;
            verifyRequest.DataFields.PersonInfo.YearOfBirth = 1988;
            //verifyRequest.DataFields.PersonInfo.MinimumAge = 28;
            //verifyRequest.DataFields.PersonInfo.Gender = "";
            //AdditionalFields
            //verifyRequest.DataFields.PersonInfo.AdditionalFields = new AdditionalFields();
            //verifyRequest.DataFields.PersonInfo.AdditionalFields.FullName = "";

            //Location
            verifyRequest.DataFields.Location.BuildingNumber = "420";
            //verifyRequest.DataFields.Location.BuildingName = "";
            verifyRequest.DataFields.Location.UnitNumber = "18";
            verifyRequest.DataFields.Location.StreetName = "9th";
            verifyRequest.DataFields.Location.StreetType = "Avenue";
            verifyRequest.DataFields.Location.City = "New York";
            //verifyRequest.DataFields.Location.Suburb = "";
            //verifyRequest.DataFields.Location.County = "";
            verifyRequest.DataFields.Location.StateProvinceCode = "NY";
            //verifyRequest.DataFields.Location.Country = "";
            verifyRequest.DataFields.Location.PostalCode = "10001";

            /*
            verifyRequest.DataFields.Location.AdditionalFields = "";  // Location-AdditionalFields
            verifyRequest.DataFields.Location.POBox = "";             // Not found
            */

            //Communication
            //verifyRequest.DataFields.Communication.MobileNumber = "";
            verifyRequest.DataFields.Communication.Telephone = "802 660 9697";
            //verifyRequest.DataFields.Communication.EmailAddress = "";

            //DriverLicence
            verifyRequest.DataFields.DriverLicence.Number = "0812319884104";
            //verifyRequest.DataFields.DriverLicence.State = "CA";
            //verifyRequest.DataFields.DriverLicence.DayOfExpiry = "";
            //verifyRequest.DataFields.DriverLicence.MonthOfExpiry = "";
            //verifyRequest.DataFields.DriverLicence.YearOfExpiry = "";

            //Passport
            verifyRequest.DataFields.Passport.Mrz1 = "P<USAWILLIAMS<<JUSTIN<<<<<<<<<<<<<<<<<<<<<<<";
            verifyRequest.DataFields.Passport.Mrz2 = "99003853<1USA1101018M1207046110101111<<<<<94";
            verifyRequest.DataFields.Passport.Number = "S85416687";
            verifyRequest.DataFields.Passport.DayOfExpiry = 5;
            verifyRequest.DataFields.Passport.MonthOfExpiry = 1;
            verifyRequest.DataFields.Passport.YearOfExpiry = 2021;

            verifyRequest.DataFields.NationalIds = new NationalId[] { new NationalId { Number= "000568791", Type = "SocialService" } };

            var response = await truliooClient.Verification.VerifyAsync(verifyRequest);

            //var responseString = response.Result; //get result of the request
        }
    }
}
