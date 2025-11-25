namespace lemco_id_core_api.Models
{
    public class PrintLogs
    {
        public List<PrintLogItem> data { get; set; } = new List<PrintLogItem>();
    }

    public class PrintLogItem {
        public int systemId {  get; set; }
        public string FullName { get; set; }
        public DateTime DatePrinted { get; set; }
        public string imgURL { get; set; }
    }
}
