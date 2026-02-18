namespace VOCENuOrderApp.Models.XORO
{
    // Create Xoro Sales Order
    public class XoroSalesOrder
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public List<XoroOrder> Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
    }

    //Xoro SO Model and Class
    public class XoroOrder // Old name XoroSORootobject
    {
        public Soestimateheader SoEstimateHeader { get; set; }
        public List<Soestimateitemlinearr> SoEstimateItemLineArr { get; set; }
    }

    // XoroOrder Header level details
    public partial class Soestimateheader
    {
        public int Id { get; set; }
        public bool AutoCreateCarrierFlag { get; set; }
        public bool AutoWaveSalesOrder { get; set; }
        public bool AutoReleaseSalesOrder { get; set; }
        public string AutoDepositTotalAmount { get; set; }
        public bool ApplyPricingRule { get; set; }
        public string BrandName { get; set; }
        public string BillToAddr { get; set; }
        public string BillToAddr2 { get; set; }
        public string BillToCity { get; set; }
        public string BillToCountry { get; set; }
        public string BillToState { get; set; }
        public string BillToZpCode { get; set; }
        public string BillToFirstName { get; set; }
        public string BillToLastName { get; set; }
        public string BillToCompanyName { get; set; }
        public string BillToPhoneNumber { get; set; }
        public string BillToEmail { get; set; }
        public string CurrencyCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerMainPhone { get; set; }
        public string CustomerMessage { get; set; }
        public string CustomerFullName { get; set; }
        public string CustomerEmailMain { get; set; }
        public string CustomerPO { get; set; }
        public string CustomerTags { get; set; }
        public string CustomerTypeName { get; set; }
        public string CustomerGroupName { get; set; }
        public string CustomerDefaultOrderTypeName { get; set; }
        public string CustomerDefaultProjectClassName { get; set; }
        public string CarrierName { get; set; }
        public string DateToBeShipped { get; set; }
        public string DateToBeCancelled { get; set; }
        public bool DepositRequiredFlag { get; set; }
        public object DepositRequiredType { get; set; }
        public object DepositRequiredTypeName { get; set; }
        public object DepositRequiredAmount { get; set; }
        public object DepositAmount { get; set; }
        public string DepositAccountName { get; set; }
        public string FobName { get; set; }
        public bool IsCustomerTaxExempt { get; set; }
        public string LiabilityAccountName { get; set; }
        public string Memo { get; set; }
        public string OrderDate { get; set; }
        public string OrderTypeName { get; set; }
        public string OrderClassName { get; set; }
        public string PaymentTermsName { get; set; }
        public string PaymentMethodName { get; set; }
        public string RefNo { get; set; }
        public bool ReCalcTaxesFlag { get; set; }
        public bool ReCalculateTaxes { get; set; }
        public bool ReCalcShippingTaxesFlag { get; set; }
        public string SaleStoreName { get; set; }
        public string StoreName { get; set; }
        public string ShipMethodName { get; set; }
        public string SalesRepId { get; set; }
        public object ShippingCost { get; set; }
        public string ShippingTermsName { get; set; }
        public string ShipServiceName { get; set; }
        public string ShippingNotes { get; set; }
        public string ShipToAddr { get; set; }
        public string ShipToAddr2 { get; set; }
        public string ShipToCity { get; set; }
        public string ShipToCountry { get; set; }
        public string ShipToState { get; set; }
        public string ShipToZpCode { get; set; }
        public string ShipToFirstName { get; set; }
        public string ShipToLastName { get; set; }
        public string ShipToCompanyName { get; set; }
        public string ShipToPhoneNumber { get; set; }
        public string ShipToEmail { get; set; }
        //public Shippingtaxitem[] ShippingTaxItems { get; set; }
        public List<Taxitem> ShippingTaxItems { get; set; }
        public string OrderNumber { get; set; }
        public string SoSubTypeName { get; set; }
        public string ThirdPartyRefNo { get; set; }
        public string ThirdPartyRefName { get; set; }
        public string ThirdPartySource { get; set; }
        public string ThirdPartyDisplayName { get; set; }
        public string ThirdPartyIconUrl { get; set; }
        public string TaxCalcCode { get; set; }
        public bool ReCalcFlag { get; set; }
        public string Tags { get; set; }
        public int? WaveAllocationCode { get; set; }
        public string CustomFieldH1 { get; set; }
        public string CustomFieldH10 { get; set; }
        public string CustomFieldH11 { get; set; }
        public string CustomFieldH12 { get; set; }
        public string CustomFieldH13 { get; set; }
        public string CustomFieldH14 { get; set; }
        public string CustomFieldH15 { get; set; }
        public string CustomFieldH16 { get; set; }
        public string CustomFieldH17 { get; set; }
        public string CustomFieldH18 { get; set; }
        public string CustomFieldH19 { get; set; }
        public string CustomFieldH2 { get; set; }
        public string CustomFieldH20 { get; set; }
        public string CustomFieldH21 { get; set; }
        public string CustomFieldH22 { get; set; }
        public string CustomFieldH23 { get; set; }
        public string CustomFieldH24 { get; set; }
        public string CustomFieldH25 { get; set; }
        public string CustomFieldH26 { get; set; }
        public string CustomFieldH27 { get; set; }
        public string CustomFieldH28 { get; set; }
        public string CustomFieldH29 { get; set; }
        public string CustomFieldH3 { get; set; }
        public string CustomFieldH30 { get; set; }
        public string CustomFieldH31 { get; set; }
        public string CustomFieldH32 { get; set; }
        public string CustomFieldH33 { get; set; }
        public string CustomFieldH34 { get; set; }
        public string CustomFieldH35 { get; set; }
        public string CustomFieldH36 { get; set; }
        public string CustomFieldH37 { get; set; }
        public string CustomFieldH38 { get; set; }
        public string CustomFieldH39 { get; set; }
        public string CustomFieldH4 { get; set; }
        public string CustomFieldH40 { get; set; }
        public string CustomFieldH41 { get; set; }
        public string CustomFieldH42 { get; set; }
        public string CustomFieldH43 { get; set; }
        public string CustomFieldH44 { get; set; }
        public string CustomFieldH45 { get; set; }
        public string CustomFieldH46 { get; set; }
        public string CustomFieldH47 { get; set; }
        public string CustomFieldH48 { get; set; }
        public string CustomFieldH49 { get; set; }
        public string CustomFieldH5 { get; set; }
        public string CustomFieldH50 { get; set; }
        public string CustomFieldH6 { get; set; }
        public string CustomFieldH7 { get; set; }
        public string CustomFieldH8 { get; set; }
        public string CustomFieldH9 { get; set; }
    }

    public partial class Shippingtaxitem
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string RateType { get; set; }
    }

    //XoroOrder Line Level Details
    public partial class Soestimateitemlinearr
    {
        public int Id { get; set; }
        public string ThirdPartyRefNo2 { get; set; }
        public string ItemTypeName { get; set; }
        public string ItemNumber { get; set; }
        public int ItemIdentifierCode { get; set; }
        public string Description { get; set; }
        public double UnitPrice { get; set; }
        public double Qty { get; set; }
        public double CancelQty { get; set; }
        public object Discount { get; set; }
        public string DiscountTypeName { get; set; }
        public string DateToBeShipped { get; set; }
        public string Notes { get; set; }
        public string OrderLineClassName { get; set; }
        public string ProjectClassName { get; set; }
        public string CustomFieldD1 { get; set; }
        public string CustomFieldD10 { get; set; }
        public string CustomFieldD11 { get; set; }
        public string CustomFieldD12 { get; set; }
        public string CustomFieldD13 { get; set; }
        public string CustomFieldD14 { get; set; }
        public string CustomFieldD15 { get; set; }
        public string CustomFieldD16 { get; set; }
        public string CustomFieldD17 { get; set; }
        public string CustomFieldD18 { get; set; }
        public string CustomFieldD19 { get; set; }
        public string CustomFieldD2 { get; set; }
        public string CustomFieldD20 { get; set; }
        public string CustomFieldD21 { get; set; }
        public string CustomFieldD22 { get; set; }
        public string CustomFieldD23 { get; set; }
        public string CustomFieldD24 { get; set; }
        public string CustomFieldD25 { get; set; }
        public string CustomFieldD26 { get; set; }
        public string CustomFieldD27 { get; set; }
        public string CustomFieldD28 { get; set; }
        public string CustomFieldD29 { get; set; }
        public string CustomFieldD3 { get; set; }
        public string CustomFieldD30 { get; set; }
        public string CustomFieldD31 { get; set; }
        public string CustomFieldD32 { get; set; }
        public string CustomFieldD33 { get; set; }
        public string CustomFieldD34 { get; set; }
        public string CustomFieldD35 { get; set; }
        public string CustomFieldD36 { get; set; }
        public string CustomFieldD37 { get; set; }
        public string CustomFieldD38 { get; set; }
        public string CustomFieldD39 { get; set; }
        public string CustomFieldD4 { get; set; }
        public string CustomFieldD40 { get; set; }
        public string CustomFieldD41 { get; set; }
        public string CustomFieldD42 { get; set; }
        public string CustomFieldD43 { get; set; }
        public string CustomFieldD44 { get; set; }
        public string CustomFieldD45 { get; set; }
        public string CustomFieldD46 { get; set; }
        public string CustomFieldD47 { get; set; }
        public string CustomFieldD48 { get; set; }
        public string CustomFieldD49 { get; set; }
        public string CustomFieldD5 { get; set; }
        public string CustomFieldD50 { get; set; }
        public string CustomFieldD6 { get; set; }
        public string CustomFieldD7 { get; set; }
        public string CustomFieldD8 { get; set; }
        public string CustomFieldD9 { get; set; }
        public Taxdata TaxData { get; set; }
        public List<Taxitem> TaxItems { get; set; }
    }

    public partial class Taxitem
    {
        public string Name { get; set; }
        public decimal Rate { get; set; }
        public string RateType { get; set; }
    }

    public class Taxdata
    {
        public List<Taxitem> taxItems { get; set; }
        public float totalAmount { get; set; }
    }









    // Sales Order Create Request Response
    public class XoroSalesOrderCreateResponse
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public Data Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public string? MessageTitle { get; set; }
        public string MetaData { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
    }

    public class Data
    {
        public object SoEstimateEdiLogArr { get; set; }
        public Soestimateheader SoEstimateHeader { get; set; }
        public Soestimateitemlinearr[] SoEstimateItemLineArr { get; set; }
        public object ThirdPartyRequestRawData { get; set; }
    }

    public class Reportdataobj
    {
        public string EntityAccntId { get; set; }
        public string EntityName { get; set; }
        public object EntityObj { get; set; }
        public object linkedTxnRefId { get; set; }
        public int ReportEntityId { get; set; }
        public int TxnRefId { get; set; }
        public string TxnRefNumber { get; set; }
        public int TxnTypeId { get; set; }
    }

    public class Shippingtaxdata
    {
        public Taxitem[] taxItems { get; set; }
        public float totalAmount { get; set; }
    }



}
