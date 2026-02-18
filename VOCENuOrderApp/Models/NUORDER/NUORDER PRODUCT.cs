namespace VOCENuOrderApp.Models.NUORDER
{
    // Nu Order Product Create Object
    public class NUORDER_PRODUCT_CREATE
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string category { get; set; }
        public string department { get; set; }
        public string type { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string color_family { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string gender { get; set; }
        public string subtype { get; set; }
        public string design { get; set; }
        public Size[] sizes { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public string cancelled { get; set; }
        public string archived { get; set; }
        public string disabled { get; set; }
        public string description { get; set; }
        public string product_description { get; set; }
        public string composition { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public Pricing pricing { get; set; }
        public string[] seasons { get; set; }
    }

    public class Pricing
    {
        public USD USD { get; set; }
        public CAD CAD { get; set; }
    }

    public class PricingVOCE
    {
        public USD USD { get; set; }
        public CAD CAD { get; set; }
        public AUD AUD { get; set; }
    }

    public class USD
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
    }

    public class AUD
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
    }

    public class CAD
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
    }


    public class Size
    {
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        //public Pricing pricing { get; set; }
    }

    public class Size_PAPER
    {
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        public string xoro_sku { get; set; }
        //public Pricing pricing { get; set; }
    }


    public class Size_VOCE
    {
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        public string xoro_item_number { get; set; }
        //public Pricing pricing { get; set; }
    }


    public class Size_CHLDR
    {
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        public string xoroitemnumber { get; set; }
        //public Pricing pricing { get; set; }
    }

    public class Pricing1
    {
        public USD1 USD { get; set; }
    }

    public class USD1
    {
        public decimal wholesale { get; set; }
        public decimal retail { get; set; }
        public bool disabled { get; set; }
    }

    // Product Create and Update NuOrder Product - RTA
    public class NUORDER_PRODUCT_UPDATE_RTA
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public List<Size> sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public List<string> images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public Pricing pricing { get; set; }
        public List<string> seasons { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string fabric_description { get; set; }
    }


    // Create and Update NuOrder Product - PAPERLABEL
    public class NUORDER_PRODUCT_UPDATE_PAPERLABEL
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public List<Size_PAPER> sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public List<string> images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        //public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public Pricing pricing { get; set; }
        public List<string> seasons { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string fabric_description { get; set; }
        //Custom Fields in NuOrder
        public string country_of_origin { get; set; }
    }


    // Create and Update NuOrder Product - VOCE
    public class NUORDER_PRODUCT_UPDATE_VOCE
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public List<Size_VOCE> sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public List<string> images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public PricingVOCE pricing { get; set; }
        public List<string> seasons { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string fabric_description { get; set; }
        //Custom Fields in NuOrder
        public string country_of_origin { get; set; }
        public string division { get; set; }
        public string department { get; set; }
    }


    // Create and Update NuOrder Product - CHEER LEADER
    public class NUORDER_PRODUCT_UPDATE_CHRLDR
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public List<Size_CHLDR> sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public List<string> images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        //public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public Pricing pricing { get; set; }
        public List<string> seasons { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
        public string fabric_description { get; set; }
        //Custom Fields in NuOrder
        public string country_of_origin { get; set; }
        public string XoroItemNumber { get; set; }
    }


    // NUORDER Product Create/Update Response
    public class NUORDER_PRODUCT_UPDATE_RESPONSE
    {
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public Size[] sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public string[] images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public Pricing pricing { get; set; }
        public string[] seasons { get; set; }
        public string _id { get; set; }
        public string[] __size_ids { get; set; }
        public DateTime modified_on { get; set; }
        public __Inventory_Cache[] __inventory_cache { get; set; }
        public string[] __inventory { get; set; }
    }

    public class __Inventory_Cache
    {
        public string bucket { get; set; }
        public string warehouse { get; set; }
        public string sku_id { get; set; }
        public string _id { get; set; }
        public int quantity { get; set; }
    }


    // NUORDER Product Create/Update - FUTURE
    // Create and Update NuOrder Product
    public class NUORDER_PRODUCT_UPDATE_FUTURE
    {
        public string style_number { get; set; }
        // Required Field
        public string season { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string name { get; set; }
        public string brand_id { get; set; }
        public string unique_key { get; set; }
        public string schema_id { get; set; }
        public List<Size1> sizes { get; set; }
        public string[] banners { get; set; }
        public string[] size_groups { get; set; }
        public bool available_now { get; set; }
        public List<string> images { get; set; }
        public bool cancelled { get; set; }
        public bool archived { get; set; }
        public bool active { get; set; }
        public string description { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public Pricing1 pricing { get; set; }
        public List<string> seasons { get; set; }
        public string category { get; set; }
        public string subcategory { get; set; }
    }

    public class Size1
    {
        public string _id { get; set; }
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        public Pricing1 pricing { get; set; }
    }



    // SAMPLE CREATE NUORDER PRODUCT RESPONSE
    public class SAMPLE_NUORDER_PRODUCT_UPDATE_RESPONSE
    {
        public string _id { get; set; }
        public string schema_id { get; set; }
        public string name { get; set; }
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string category { get; set; }
        public bool active { get; set; }
        public bool archived { get; set; }
        public bool cancelled { get; set; }
        public string[] images { get; set; }
        public bool available_now { get; set; }
        public object size_groups { get; set; }
        public object[] banners { get; set; }
        public bool __original_composite_keys { get; set; }
        public bool __tracks_inventory { get; set; }
        public string[] __inventory { get; set; }
        public object[] __warehouses { get; set; }
        public bool __sold_out { get; set; }
        public int __merchandising_order { get; set; }
        public string __brand_name { get; set; }
        public bool __pending_initial_pricing { get; set; }
        public object[] __pack_ids { get; set; }
        public string[] seasons { get; set; }
        public string subcategory { get; set; }
        public string description { get; set; }
        public string brand_id { get; set; }
        public __Size_Ids[] __size_ids { get; set; }
        public Pricing pricing { get; set; }
        public Size1[] sizes { get; set; }
        public DateTime created_on { get; set; }
        public object[] __inventory_cache { get; set; }
        public object[] __inventory_hashes { get; set; }
        public string __barcode { get; set; }
        public DateTime modified_on { get; set; }
        public string __size_run { get; set; }
        public string __size_range { get; set; }
        public string unique_key { get; set; }
        public string color_code { get; set; }
        public string department { get; set; }
        public string division { get; set; }
        public object[] warehouses { get; set; }
        public string __cdn { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public string? message { get; set; }
        public string? code { get; set; }
    }

    public class __Size_Ids
    {
        public string _id { get; set; }
        public string size { get; set; }
        public string __global_id { get; set; }
    }


    public class Size1_CHRLDR
    {
        public string _id { get; set; }
        public string size { get; set; }
        public string size_group { get; set; }
        public string size_order { get; set; }
        public string upc { get; set; }
        public Pricing1 pricing { get; set; }
        public string xoroitemnumber { get; set; }
    }



    // SAMPLE CREATE NUORDER PRODUCT RESPONSE - CHEER LEEDER
    public class SAMPLE_NUORDER_PRODUCT_UPDATE_RESPONSE_CHLDR
    {
        public string _id { get; set; }
        public string schema_id { get; set; }
        public string name { get; set; }
        public string style_number { get; set; }
        public string season { get; set; }
        public string color { get; set; }
        public string category { get; set; }
        public bool active { get; set; }
        public bool archived { get; set; }
        public bool cancelled { get; set; }
        public string[] images { get; set; }
        public bool available_now { get; set; }
        public object size_groups { get; set; }
        public object[] banners { get; set; }
        public bool __original_composite_keys { get; set; }
        public bool __tracks_inventory { get; set; }
        public string[] __inventory { get; set; }
        public object[] __warehouses { get; set; }
        public bool __sold_out { get; set; }
        public int __merchandising_order { get; set; }
        public string __brand_name { get; set; }
        public bool __pending_initial_pricing { get; set; }
        public object[] __pack_ids { get; set; }
        public string[] seasons { get; set; }
        public string subcategory { get; set; }
        public string description { get; set; }
        public string brand_id { get; set; }
        public __Size_Ids_CHRLDR[] __size_ids { get; set; }
        public Pricing pricing { get; set; }
        public Size1_CHRLDR[] sizes { get; set; }
        public DateTime created_on { get; set; }
        public object[] __inventory_cache { get; set; }
        public object[] __inventory_hashes { get; set; }
        public string __barcode { get; set; }
        public DateTime modified_on { get; set; }
        public string __size_run { get; set; }
        public string __size_range { get; set; }
        public string unique_key { get; set; }
        public string color_code { get; set; }
        public string department { get; set; }
        public string division { get; set; }
        public object[] warehouses { get; set; }
        public string __cdn { get; set; }
        public string available_from { get; set; }
        public string available_until { get; set; }
        public string order_closing { get; set; }
        public string? message { get; set; }
        public string? code { get; set; }
    }

    public class __Size_Ids_CHRLDR
    {
        public string _id { get; set; }
        public string size { get; set; }
        public string __global_id { get; set; }
    }


    // Object to map NuOrder size id to Xoro Product size id
    public class NuOrderToXoroSizeMapping
    {
        public string Style { get; set; }
        public string BrandId { get; set; }
        public string Color { get; set; }
        public string Upc { get; set; }
        public string Size { get; set; }
        public string NuOrderSizeId { get; set; }
    }

}
