namespace lemco_id_core_api.Models
{
    public class ResponseFormat
    {
        public List<ResponseFormatNode> data { get; set; } = new List<ResponseFormatNode>();
    }

    public class ResponseFormatNode {
        public string? label { get; set; }
        public Employee? value { get; set; }
    }
}
