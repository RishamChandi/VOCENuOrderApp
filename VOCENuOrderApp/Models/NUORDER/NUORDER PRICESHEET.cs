namespace VOCENuOrderApp.Models.NUORDER
{
    public class NUORDER_PRICESHEET
    {
        public NUORDERPricing[] pricing { get; set; }
    }

    public class NUORDERPricing
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
        public Date_Based_Pricing[] date_based_pricing { get; set; }
        public string template { get; set; }
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string brand_id { get; set; }
    }

    public class Date_Based_Pricing
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }


    // REGULAR MODEL FOR PRICESHEET
    public class NuOrderDateBasedPricingModel
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
        public List<NuOrderSizeEntry> sizes { get; set; }
        public string template { get; set; }
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string brand_id { get; set; }
        public string _id { get; set; }
    }

    public class NuOrderSizeEntry
    {
        public string _id { get; set; }
        public string wholesale { get; set; }
        public string retail { get; set; }
        public string size { get; set; }
    }

}
