namespace VOCENuOrderApp.Models.XORO
{
    public class XORO_INVENTORY
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public List<XoroInvDatum> Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public object[] Exceptions { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
        public int Page { get; set; }
        public int TotalPages { get; set; }
    }

    public class XoroInvDatum
    {
        public float AllocatedQty { get; set; }
        public float ATSQty { get; set; }
        public float ATSQtyIncPO { get; set; }
        public float AvailableQty { get; set; }
        public string BasePartNumber { get; set; }
        public string ItemDescription { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ItemTags { get; set; }
        public float NetAvailable { get; set; }
        public float NotAvailableQty { get; set; }
        public float OnHandQty { get; set; }
        public int ProductHeaderId { get; set; }
        public float QtyOnASN { get; set; }
        public float QtyOnPO { get; set; }
        public float QtyOnPODraft { get; set; }
        public float QtyOnSO { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }



    // NEW INVENTORY OBJECT

    public class XORO_INVENTORY_NEW
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public Datum_NEW[] Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public object[] Exceptions { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
    }

    public class Datum_NEW
    {
        public float AllocatedQty { get; set; }
        public float ATSQty { get; set; }
        public float ATSQtyIncPO { get; set; }
        public float AvailableQty { get; set; }
        public string BasePartNumber { get; set; }
        public string ItemDescription { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ItemTags { get; set; }
        public float NetAvailable { get; set; }
        public float NotAvailableQty { get; set; }
        public float OnHandQty { get; set; }
        public int ProductHeaderId { get; set; }
        public float QtyOnASN { get; set; }
        public float QtyOnPO { get; set; }
        public float QtyOnPODraft { get; set; }
        public float QtyOnSO { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }




    // Xoro Inventory Webhook model

    public class InventoryWebhookModel
    {
        public List<InventoryATS> Property1 { get; set; }
    }

    public class InventoryATS
    {
        public float AllocatedQty { get; set; }
        public float ATSQty { get; set; }
        public float ATSQtyIncMo { get; set; }
        public float ATSQtyIncPO { get; set; }
        public float ATSQtyIncPoMo { get; set; }
        public float AvailableQty { get; set; }
        public float BomATSQty { get; set; }
        public float BomATSQtyIncMo { get; set; }
        public float BomATSQtyIncPO { get; set; }
        public float BomATSQtyIncPoMo { get; set; }
        public float BomAvailableQty { get; set; }
        public float BomNetAvailable { get; set; }
        public float BomOnHandQty { get; set; }
        public object CustomerId { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public string ItemTags { get; set; }
        public string Memo { get; set; }
        public float NetAvailable { get; set; }
        public float NotAvailableQty { get; set; }
        public float OnHandQty { get; set; }
        public string RefNo { get; set; }
        public int StoreId { get; set; }
        public object StoreName { get; set; }
        public object[] ThirdPartyFieldMappingMetadataObjList { get; set; }
        public string ThirdPartySource { get; set; }
        public float TotalBomATSQty { get; set; }
        public float TotalBomATSQtyIncMo { get; set; }
        public float TotalBomATSQtyIncPO { get; set; }
        public float TotalBomATSQtyIncPoMo { get; set; }
        public float TotalBomAvailableQty { get; set; }
        public float TotalBomNetAvailable { get; set; }
        public float TotalBomOnHandQty { get; set; }
        public int TxnRefId { get; set; }
        public int TxnTypeId { get; set; }
        public string RefMessage { get; set; }
    }



    // Class representing a single inventory item from the Xoro Inventory API.
    public class XoroInventoryData
    {
        public int AllocatedQty { get; set; }
        public int ATSQty { get; set; }
        public int ATSQtyIncPO { get; set; }
        public int AvailableQty { get; set; }
        public string BasePartNumber { get; set; }
        public string ItemDescription { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public int NetAvailable { get; set; }
        public int NotAvailableQty { get; set; }
        public int OnHandQty { get; set; }
        public int ProductHeaderId { get; set; }
        public int QtyOnASN { get; set; }
        public int QtyOnPO { get; set; }
        public int QtyOnPODraft { get; set; }
        public int QtyOnSO { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
    }

    // Class representing the Xoro Inventory API response.
    public class XoroInventory
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public List<XoroInventoryData> Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
    }


}
