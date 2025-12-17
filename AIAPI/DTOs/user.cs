namespace AIAPI.DTOs
{
    
        public class UserDTO
        {

            public string? userName { get; set; }
            public string? password { get; set; }
        }

    public class ApiResponse<T>
    {
        public int status { get; set; }
        public string statusText { get; set; }
        public T body { get; set; }
    }

    public class ProcessMetaApiBody
    {
        public Orderlist OrderList { get; set; }
        public List<Dictionary<string, string>> SchemeMaster { get; set; }
        public List<Dictionary<string, string>> FrequentPurchasedMaster { get; set; }
    }

}
