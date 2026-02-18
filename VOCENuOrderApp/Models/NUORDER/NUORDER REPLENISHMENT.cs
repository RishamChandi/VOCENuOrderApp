namespace VOCENuOrderApp.Models.NUORDER
{
    // API Documentation: https://nuorder.com/api-docs/#replenishment
    public class NUORDER_REPLENISHMENT_BULK
    {
        public List<NuOrderReplenishment> replenishments { get; set; }
    }

    public class NuOrderReplenishment
    {
        //public string id { get; set; }
        public string warehouse_id { get; set; }
        public string sku_id { get; set; }
        public long created_on { get; set; }
        public long modified_on { get; set; }
        public string display_name { get; set; }
        public bool prebook { get; set; }
        public string? period_start { get; set; }
        public string? period_end { get; set; }
        public float? quantity { get; set; }
        public bool active { get; set; }
    }


    // Sent by Breon NuOrder
    public class NuOrderReplenRootobject
    {
        public List<Replenishment> replenishments { get; set; }
    }

    public class Replenishment
    {
        public string warehouse_id { get; set; }
        public string sku_id { get; set; }
        public string display_name { get; set; }
        public bool prebook { get; set; }
        public string period_start { get; set; }
        public object period_end { get; set; }
        public int? quantity { get; set; }
        public bool active { get; set; }
    }


    // Replenishment response
    public class NuOrderReplenResponse
    {
        public Class2[] Property1 { get; set; }
    }

    public class Class2
    {
        public string warehouse_id { get; set; }
        public string sku_id { get; set; }
        public int quantity { get; set; }
        public string display_name { get; set; }
        public long period_start { get; set; }
        public long? period_end { get; set; }
        public bool prebook { get; set; }
        public string id { get; set; }
        public string display_quantity { get; set; }
        public string __key { get; set; }
    }



}
