namespace SalesTeam.Web.Models
{
    public class FaxDocumentModel
    {
        //public string DocumentPath { get; set; }
        public string DocumentIdEncrypted { get; set; }
        public string FaxNumber { get; set; }
        public string Email { get; set; }
        public int PhysicianId { get; set; }

    }
}