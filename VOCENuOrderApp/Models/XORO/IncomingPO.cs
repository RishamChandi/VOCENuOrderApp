namespace VOCENuOrderApp.Models.XORO
{
    public class IncomingPO
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public IncomingPODatum[] Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
    }

    public class IncomingPODatum
    {
        public float AvailablePOLineQty { get; set; }
        public object CustomFieldH1 { get; set; }
        public object CustomFieldH10 { get; set; }
        public object CustomFieldH11 { get; set; }
        public object CustomFieldH12 { get; set; }
        public object CustomFieldH13 { get; set; }
        public object CustomFieldH14 { get; set; }
        public object CustomFieldH15 { get; set; }
        public object CustomFieldH16 { get; set; }
        public object CustomFieldH17 { get; set; }
        public object CustomFieldH18 { get; set; }
        public object CustomFieldH19 { get; set; }
        public object CustomFieldH2 { get; set; }
        public object CustomFieldH20 { get; set; }
        public object CustomFieldH21 { get; set; }
        public object CustomFieldH22 { get; set; }
        public object CustomFieldH23 { get; set; }
        public object CustomFieldH24 { get; set; }
        public object CustomFieldH25 { get; set; }
        public object CustomFieldH26 { get; set; }
        public object CustomFieldH27 { get; set; }
        public object CustomFieldH28 { get; set; }
        public object CustomFieldH29 { get; set; }
        public object CustomFieldH3 { get; set; }
        public object CustomFieldH30 { get; set; }
        public object CustomFieldH31 { get; set; }
        public object CustomFieldH32 { get; set; }
        public object CustomFieldH33 { get; set; }
        public object CustomFieldH34 { get; set; }
        public object CustomFieldH35 { get; set; }
        public object CustomFieldH36 { get; set; }
        public object CustomFieldH37 { get; set; }
        public object CustomFieldH38 { get; set; }
        public object CustomFieldH39 { get; set; }
        public object CustomFieldH4 { get; set; }
        public object CustomFieldH40 { get; set; }
        public object CustomFieldH41 { get; set; }
        public object CustomFieldH42 { get; set; }
        public object CustomFieldH43 { get; set; }
        public object CustomFieldH44 { get; set; }
        public object CustomFieldH45 { get; set; }
        public object CustomFieldH46 { get; set; }
        public object CustomFieldH47 { get; set; }
        public object CustomFieldH48 { get; set; }
        public object CustomFieldH49 { get; set; }
        public object CustomFieldH5 { get; set; }
        public object CustomFieldH50 { get; set; }
        public object CustomFieldH6 { get; set; }
        public object CustomFieldH7 { get; set; }
        public object CustomFieldH8 { get; set; }
        public object CustomFieldH9 { get; set; }
        public string ExpectedDeliveryDate { get; set; }
        public string ExpectedShipDate { get; set; }
        public bool IsDropshipPO { get; set; }
        public int ItemId { get; set; }
        public string ItemNumber { get; set; }
        public int LineStatusId { get; set; }
        public float LinkedSOQty { get; set; }
        public int POId { get; set; }
        public int PoLineId { get; set; }
        public float POLineQty { get; set; }
        public string PONumber { get; set; }
        public object RefId { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public object TxnTypeId { get; set; }
        public int UomId { get; set; }
        public string UomName { get; set; }
        public int VendorId { get; set; }
        public string VendorName { get; set; }
    }

}
