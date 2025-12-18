using System.Text.RegularExpressions;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace AIAPI.DTOs
{


    public class Orderlist
    {

        public List<OrderDTO> Order { get; set; }
    }



    public class OrderDTO
    {
        [JsonProperty("ORDER ID")]
        public string? OrderID { get; set; }

        [JsonProperty("Total Order Value")]
        public string? Totalordervalue { get; set; }

        [JsonProperty("Total Discount")]
        public string? Totaldiscount { get; set; }

        [JsonProperty(" Discount Percentage")]
        public string? DiscountPercentage { get; set; }
        [JsonProperty("Total Invoiced Value")]
        public string? Totalinvoicesvalue { get; set; }

        [JsonProperty("Discount Invoiced")]
        public string? Discountinvoiced { get; set; }

        [JsonProperty("Balance Value To Be Invoiced")]
        public string? Balancevaluetobeinvoiced { get; set; }

        [JsonProperty("Balance Discount To Be Invoiced")]
        public string? Balancediscountobeinvoiced { get; set; }

        [JsonProperty("Details")]
        public List<OrderDetails>? Order_Details { get; set; }
    }







    public class OrderDetails
    {

        [JsonProperty("ORDER ID")]
        public string? OrderID { get; set; }

        [JsonProperty("Item Name")]
        public string? Itemname { get; set; }

        [JsonProperty("Quantity In order")]
        public string? QuantityInorder { get; set; }

        public string? Price { get; set; }

        [JsonProperty("Discount In Order")]
        public string? DiscountInOrder { get; set; }

        [JsonProperty("Status OF Order")]
        public string? StatusOfOrder { get; set; }

        public string? Mobileno { get; set; }

        [JsonProperty("Invoiced_Quantity")]
        public string? Invoiced_Quantity { get; set; }

        [JsonProperty("Discount IN Invocie")]
        public string? DiscountINInvocie { get; set; }

        [JsonProperty("Invoiced Number")]
        public string? InvoicedNumber { get; set; }

        [JsonProperty("Cancelled_Quantity")]
        public string? Cancelled_Quantity { get; set; }

        [JsonProperty("Discount IN Cancelled Invocie")]
        public string? DiscountINCancelledInvocie { get; set; }

        [JsonProperty("Cancelled Invoiced Number")]
        public string? CancelledInvoicedNumber { get; set; }
    }




    public class ProcessMetaResponse
    {
        [JsonProperty("Purchase History Master")]
        public List<Dictionary<string, object>> PurchaseHistoryMaster { get; set; }


        [JsonProperty("Scheme Master")]
        public List<Dictionary<string, object>> SchemeMaster { get; set; }

        [JsonProperty("Frequent Purchased Master")]
        public List<Dictionary<string, object>> FrequentPurchasedMaster { get; set; }
    }

    public class ProcessMetaResponseTyped
    {
        public Orderlist PurchaseHistoryMaster { get; set; }
        public List<Dictionary<string, object>> SchemeMaster { get; set; }
        public List<Dictionary<string, object>> FrequentPurchasedMaster { get; set; }
    }


    public class Schemelist
    {

        public List<SchemeDTO> Scheme { get; set; }
    }



    public class SchemeDTO
    {
        public string? SchemeDocno { get; set; }
        public string? SchemeName { get; set; }
        public string? Purchase_Value { get; set; }
        public string? Benifit_value { get; set; }
        public string? Ratio { get; set; }
        public List<SchemeDetails>? SchemeDetails { get; set; }
    }

    public class SchemeDetails
    {
        
        public string? SchemeName { get; set; }
        public string? SchemeGroup { get; set; }
        public string? MinuminValue { get; set; }
        public string? MaximumValue { get; set; }
        public string? DiscountType { get; set; }
        public string? DiscountValue { get; set; }
        public string? GiftDetails { get; set; }
        public string? GiftQuantity { get; set; }
        public string? ValidFrom { get; set; }
        public string? ValidTo { get; set; }
        public string? ProductGroup { get; set; }
        public string? SchemeType { get; set; }
        public string? BannerImage { get; set; }
        public string? CX { get; set; }
        public string? CY { get; set; }
        public string? DECO { get; set; }
        public string? Benifit_value { get; set; }

    }




    public class AiprocessDTO
    {
        public List<Orderlist> PurchaseHistoryMaster { get; set; }

        public List<Schemelist> SchemeMaster { get; set; }

        public string? FrequentPurchasedMaster { get; set; }
    }

    public class purchaselist
    {
        public string? Product_Type { get; set; }
        public List<PurchaseDTO> Purchase { get; set; }
    }

   
    public class PurchaseDTO
    {        
        public string? Item_Group { get; set; }
       
        public List<PurchaseDetails>? PurchaseDetails { get; set; }      
    }
    public class PurchaseDetails
    {
        public string? Item_Name { get; set; }
        public string? Max_Quantity { get; set; }
        public string? Avg_quantity { get; set; }

    }


    public class Customerlist
    {
        public List<CustomerDTO> Customer { get; set; }
    }


    public class CustomerDTO
    {
        public string? Customer_ID { get; set; }
        public string? Customer_Name { get; set; }
        public string? Customer_Phoneno { get; set; }
       
    }

}