using Org.BouncyCastle.Bcpg.OpenPgp;

namespace lemco_id_core_api.Models
{
    public class Employee
    {
        public int systemId { get; set; }
        public string? id { get; set; }
        public string? lastName { get; set; }
        public string? firstName { get; set; }
        public string? middleName { get; set; }
        public DateTime birthDate { get; set; }
        public string? address { get; set; }
        public string? mID { get; set; }
        public string? sssId { get; set; }
        public string? tin { get; set; }
        public string? philhealth { get; set; }
        public string? pagibig {  get; set; }
        public string? contactPerson { get; set; }
        public string? contactAddress { get; set; }
        public string? contactNumber { get; set; }
        public DateTime? DatePrinted { get; set; }
        public string? imgURL { get; set; }
    }
}