namespace VOCENuOrderApp.Models.NUORDER
{
    // GET NUORDER SINGLE ORDER BY ORDER ID
    public class NUORDER_ORDER
    {
        public string _id { get; set; }
        public string schema_id { get; set; }
        public bool split { get; set; }
        public string submitted_by { get; set; }
        public string edited_by { get; set; }
        public bool buyer_submitted { get; set; }
        public string existing_pdf_linesheet { get; set; }
        public string tech_pdf { get; set; }
        public string admin_pdf { get; set; }
        public string manager_pdf { get; set; }
        public string rep_pdf { get; set; }
        public string buyer_pdf { get; set; }
        public bool easy_order_viewed { get; set; }
        public bool easy_order_ready { get; set; }
        public bool collaborative_draft { get; set; }
        public string order_number { get; set; }
        public string customer_po_number { get; set; }
        public string creator_name { get; set; }
        public Retailer retailer { get; set; }
        public string status { get; set; }
        public string currency_code { get; set; }
        public int discount { get; set; }
        public int additional_percentage { get; set; }
        public int total_quantity { get; set; }
        public float total { get; set; }
        public string notes { get; set; }
        public object[] selected_shipping_locations { get; set; }
        public Billing_Address billing_address { get; set; }
        public Shipping_Address shipping_address { get; set; }
        public bool edited { get; set; }
        public string order_group_id { get; set; }
        public string payment_status { get; set; }
        public bool locked { get; set; }
        public bool use_advanced_promotions { get; set; }
        public DateTime modified_on { get; set; }
        public bool is_drop_ship { get; set; }
        public string[] __shipment_status { get; set; }
        public __Split_Overrides[] __split_overrides { get; set; }
        public bool __uninitiated_order { get; set; }
        public bool __includes_cancelled { get; set; }
        public int __cancelled_units { get; set; }
        public int __cancelled_total { get; set; }
        public bool __is_rtp { get; set; }
        public object[] order_tags { get; set; }
        public string style_number { get; set; }
        public object[] shipments { get; set; }
        public Line_Items[] line_items { get; set; }
        public DateTime created_on { get; set; }
        public object[] __configure_to_order_items { get; set; }
        public DateTime edited_on { get; set; }
        public string rep_name { get; set; }
        public string rep_code { get; set; }
        public string rep_email { get; set; }
        public DateTime start_ship { get; set; }
        public DateTime end_ship { get; set; }
        public Order_Discounts order_discounts { get; set; }
    }

    public class Retailer
    {
        public string _id { get; set; }
        public string retailer_name { get; set; }
        public string retailer_code { get; set; }
        public string buyer_name { get; set; }
        public string buyer_email { get; set; }
    }

    public class Billing_Address
    {
        public string _ref { get; set; }
        public string display_name { get; set; }
        public string line_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string code { get; set; }
    }

    public class Shipping_Address
    {
        public string _ref { get; set; }
        public string display_name { get; set; }
        public string line_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string code { get; set; }
    }

    public class Order_Discounts
    {
        public float DiscountedTotal { get; set; }
        public float OriginalTotal { get; set; }
        public float TotalAfterLineItemDiscount { get; set; }
        public float TotalApplied { get; set; }
    }

    public class __Split_Overrides
    {
        public string key { get; set; }
        public string split_po { get; set; }
        public string _id { get; set; }
    }

    public class Line_Items
    {
        public string id { get; set; }
        public Product product { get; set; }
        public DateTime ship_start { get; set; }
        public DateTime ship_end { get; set; }
        public string retail_string { get; set; }
        public object[] applied { get; set; }
        public ItemSize[] sizes { get; set; }
        public string warehouse { get; set; }
        public bool prebook { get; set; }
    }

    public class Product
    {
        public string _id { get; set; }
        public string style_number { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string brand_id { get; set; }
        public string season { get; set; }
        public string department { get; set; }
        public string division { get; set; }
    }

    public class ItemSize
    {
        public string size { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string price_precise { get; set; }
        public float original_price { get; set; }
        public int units_per_pack { get; set; }
        public string upc { get; set; }
    }


    // NUORDER ORDER LIST BY STATUS

    public class NUORDER_ORDER_STATUS_LIST
    {
        public string[] order_id { get; set; }
    }


    //Brand new NuOrder object - NEW TO BE USED
    //order_type

    public class NuOrderSORootobject
    {
        public string _id { get; set; }
        public string schema_id { get; set; }
        public bool split { get; set; }
        public string submitted_by { get; set; }
        public string edited_by { get; set; }
        public bool buyer_submitted { get; set; }
        public string existing_pdf_linesheet { get; set; }
        public string tech_pdf { get; set; }
        public string admin_pdf { get; set; }
        public string manager_pdf { get; set; }
        public string rep_pdf { get; set; }
        public string buyer_pdf { get; set; }
        public object legal_file { get; set; }
        public bool easy_order_viewed { get; set; }
        public bool easy_order_ready { get; set; }
        public bool collaborative_draft { get; set; }
        public string order_number { get; set; }
        public string order_type { get; set; }
        public string customer_po_number { get; set; }
        public string creator_name { get; set; }
        public RetailerNuOrder retailer { get; set; }
        public string status { get; set; }
        public string currency_code { get; set; }
        public int discount { get; set; }
        public int additional_percentage { get; set; }
        public int total_quantity { get; set; }
        public float total { get; set; }
        public string notes { get; set; }
        public object[] selected_shipping_locations { get; set; }
        public Billing_AddressNuOrder billing_address { get; set; }
        public Shipping_AddressNuOrder shipping_address { get; set; }
        public object[] order_surcharges { get; set; }
        public bool edited { get; set; }
        public string order_group_id { get; set; }
        public string payment_status { get; set; }
        public bool locked { get; set; }
        public bool use_advanced_promotions { get; set; }
        public DateTime modified_on { get; set; }
        public bool is_drop_ship { get; set; }
        public object[] __shipment_status { get; set; }
        public __Split_Overrides[] __split_overrides { get; set; }
        public bool __uninitiated_order { get; set; }
        public bool __includes_cancelled { get; set; }
        public int __cancelled_units { get; set; }
        public int __cancelled_total { get; set; }
        public bool __is_rtp { get; set; }
        public string[] order_tags { get; set; }
        public string style_number { get; set; }
        public object[] shipments { get; set; }
        public Line_Items[] line_items { get; set; }
        public DateTime created_on { get; set; }
        public object[] __configure_to_order_items { get; set; }
        public string rep_name { get; set; }
        public string rep_code { get; set; }
        public string rep_email { get; set; }
        public DateTime start_ship { get; set; }
        public DateTime end_ship { get; set; }
        public Order_Discounts order_discounts { get; set; }
    }

    public class RetailerNuOrder
    {
        public string _id { get; set; }
        public string retailer_name { get; set; }
        public string retailer_code { get; set; }
        public string buyer_name { get; set; }
        public string buyer_email { get; set; }
    }

    public class Billing_AddressNuOrder
    {
        public string _ref { get; set; }
        public string line_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Shipping_AddressNuOrder
    {
        public string _ref { get; set; }
        public string line_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string country { get; set; }
    }

    public class Order_Discounts2
    {
        public object[] applied { get; set; }
        public string discounted_total { get; set; }
        public string original_total { get; set; }
        public string total_after_line_item_discount { get; set; }
        public string total_applied { get; set; }
        public string surcharge { get; set; }
        public string company_manual_discount { get; set; }
        public string total_line_item_manual_discounts { get; set; }
        public string total_line_item_product_promotions { get; set; }
        public string total_line_items_discount { get; set; }
        public string total_order_level_discount { get; set; }
        public string shipping_final_amount { get; set; }
        public Surcharges surcharges { get; set; }
        public string net_terms_extension { get; set; }
    }

    public class Surcharges
    {
        public Applied[] applied { get; set; }
        public string total_surcharge { get; set; }
    }

    public class Applied
    {
        public string name { get; set; }
        public string price { get; set; }
        public string currency_code { get; set; }
        public string price_code { get; set; }
        public string _id { get; set; }
    }

    public class __Split_Overrides2
    {
        public string key { get; set; }
        public string split_po { get; set; }
        public bool self { get; set; }
        public string _id { get; set; }
    }

    public class Line_Items2
    {
        public string id { get; set; }
        public Product2 product { get; set; }
        public DateTime ship_start { get; set; }
        public DateTime ship_end { get; set; }
        public string total_applied { get; set; }
        public string effective_subtotal { get; set; }
        public string original_total { get; set; }
        public string discounted_total { get; set; }
        public string item_price_after_discount { get; set; }
        public string adjusted_wholesale_string { get; set; }
        public string retail_string { get; set; }
        public string total_adjustment { get; set; }
        public string company_manual_discount_on_item { get; set; }
        public string item_manual_discount_on_item { get; set; }
        public object[] applied { get; set; }
        public Size[] sizes { get; set; }
        public Surcharges1 surcharges { get; set; }
        public string warehouse { get; set; }
        public bool prebook { get; set; }
    }

    public class Product2
    {
        public string _id { get; set; }
        public string style_number { get; set; }
        public string color { get; set; }
        public string color_code { get; set; }
        public string brand_id { get; set; }
        public string season { get; set; }
    }

    public class Surcharges1
    {
        public object[] applied { get; set; }
        public string total_surcharge { get; set; }
    }

    public class Size3
    {
        public string size { get; set; }
        public int quantity { get; set; }
        public float price { get; set; }
        public string price_precise { get; set; }
        public float original_price { get; set; }
        public int units_per_pack { get; set; }
        public string upc { get; set; }
        public Discounts discounts { get; set; }
        public Surcharges2 surcharges { get; set; }
    }

    public class Discounts
    {
        public int total_discount { get; set; }
        public object[] applied { get; set; }
        public string _id { get; set; }
    }

    public class Surcharges2
    {
        public object[] applied { get; set; }
        public string total_surcharge { get; set; }
    }

}
