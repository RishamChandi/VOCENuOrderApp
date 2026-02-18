namespace VOCENuOrderApp.Models.NUORDER
{
    public class NUORDER_INVENTORY
    {
        public List<Inventory> inventory { get; set; }
    }

    public class Inventory
    {
        public string warehouse { get; set; }
        public bool prebook { get; set; }
        public List<Wip> wip { get; set; }
    }

    public class Wip
    {
        public string name { get; set; }
        public List<SizeA> sizes { get; set; }
        public List<SizeB> size { get; set; }
    }

    public class SizeA
    {
        public string size { get; set; }
        public string upc { get; set; }
        public int quantity { get; set; }
    }

    public class SizeB
    {
        public string size { get; set; }
        public string upc { get; set; }
        public int quantity { get; set; }
    }




    //NUORDER INVENTORY POST RESPONSE
    public class NUORDER_INVENTORY_RESPONSE
    {
        public List<Class1> Property1 { get; set; }
    }

    public class Class1
    {
        public string warehouse { get; set; }
        public bool prebook { get; set; }
        public List<Wip> wip { get; set; }
    }



    //INVENTORY NULL RECORD
    public class NUORDER_INVENTORY_blank
    {
        public List<object> inventory { get; set; }
    }



}
