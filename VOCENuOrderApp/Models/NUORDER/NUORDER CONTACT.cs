namespace VOCENuOrderApp.Models.NUORDER
{
    public class NUORDER_CONTACT
    {
        public string name { get; set; }
        public string code { get; set; }
        public List<reps> reps { get; set; }
        public List<Address> addresses { get; set; }
        public int surcharge { get; set; }
        public int discount { get; set; }
        public string currency_code { get; set; }
        public bool active { get; set; }
        public string payment_terms { get; set; }
        public string phone_office { get; set; }
        public string pricing_template { get; set; }
        //Custom Field
        public string sales_rep_id { get; set; }
        public string warehouse { get; set; }
    }

    public class NUORDER_CONTACT_STANDARD
    {
        public string name { get; set; }
        public string code { get; set; }
        public List<reps> reps { get; set; }
        public List<Address> addresses { get; set; }
        public int surcharge { get; set; }
        public int discount { get; set; }
        public string currency_code { get; set; }
        public bool active { get; set; }
        public string payment_terms { get; set; }
        public string phone_office { get; set; }
        public string pricing_template { get; set; }
        //Custom Field
        public string sales_rep_id { get; set; }
        //public string warehouse { get; set; }
    }

    public class reps
    {
        public string name { get; set; }
        public string email { get; set; }
        public string _ref { get; set; }
        public string _id { get; set; }
    }

    public class Address
    {
        public string display_name { get; set; }
        public string line_1 { get; set; }
        public string line_2 { get; set; }
        public string line_3 { get; set; }
        public string line_4 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string shipping_code { get; set; }
        public string billing_code { get; set; }
        public bool default_shipping { get; set; }
        public bool default_billing { get; set; }
        public string type { get; set; }
        public string country { get; set; }
    }





    //NuOrder Customer Error Response

    public class NUORDER_CONTACT_RESPONSE
    {
        public int code { get; set; }
        public string message { get; set; }
        public string _id { get; set; }
    }




    // NuOrder Customer CREATE REQUEST FULL

    public class Rootobject
    {
        public string name { get; set; }
        public string code { get; set; }
        public Rep1[] reps { get; set; }
        public Address1[] addresses { get; set; }
        public bool allow_bulk { get; set; }
        public int surcharge { get; set; }
        public int discount { get; set; }
        public string[] customer_groups { get; set; }
        public string currency_code { get; set; }
        public bool active { get; set; }
        public string payment_terms { get; set; }
        public string credit_status { get; set; }
        public string warehouse { get; set; }
        public string legal_file { get; set; }
        public string _id { get; set; }
        public string __sortable_name { get; set; }
        public string schema_id { get; set; }
        public User_Connections[] user_connections { get; set; }
        public string[] __connected_brand_users { get; set; }
        public string[] __filter_key { get; set; }
        public string[] __search_key { get; set; }
        public DateTime created_on { get; set; }
        public int default_discount { get; set; }
        public int default_surcharge { get; set; }
        public string pricing_template { get; set; }
    }

    public class Rep1
    {
        public string name { get; set; }
        public string email { get; set; }
        public string _ref { get; set; }
        public string _id { get; set; }
    }

    public class Address1
    {
        public string display_name { get; set; }
        public string line_1 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string zip { get; set; }
        public string shipping_code { get; set; }
        public string billing_code { get; set; }
        public bool default_shipping { get; set; }
        public bool default_billing { get; set; }
        public string type { get; set; }
        public string country { get; set; }
    }

    public class User_Connections
    {
        public string name { get; set; }
        public string email { get; set; }
        public Rep1[] reps { get; set; }
        public string title { get; set; }
        public string phone_office { get; set; }
        public string phone_cell { get; set; }
        public string _id { get; set; }
        public string _ref { get; set; }
        public object[] linesheets { get; set; }
        public DateTime last_viewed { get; set; }

    }


}
