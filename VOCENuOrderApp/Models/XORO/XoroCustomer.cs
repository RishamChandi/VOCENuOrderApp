namespace VOCENuOrderApp.Models.XORO
{
    public class XoroCustomer
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public CustomerData Data { get; set; }
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

    public class CustomerData
    {
        public object Account3PLCode { get; set; }
        public int Account3PLId { get; set; }
        public string AccountNumber { get; set; }
        public object ACHStatusId { get; set; }
        public object ACHStatusName { get; set; }
        public bool ActiveFlag { get; set; }
        public object ActiveFlagStr { get; set; }
        public string AlertNote { get; set; }
        public object AlertNoteActiveFlag { get; set; }
        public object AllAddressList { get; set; }
        public object AuthorizationDetailList { get; set; }
        public bool AutoCreateCarrierFlag { get; set; }
        public bool AutoProcessPayment { get; set; }
        public object AutoProcessPaymentId { get; set; }
        public object AutoProcessPaymentMethodName { get; set; }
        public float AvailableCredit { get; set; }
        public object AverageDaysToPay { get; set; }
        public object BillingMethodName { get; set; }
        public string BillToAddr { get; set; }
        public string BillToAddr2 { get; set; }
        public Billtoaddrlist[] BillToAddrList { get; set; }
        public object BillToCity { get; set; }
        public object BillToCompanyName { get; set; }
        public object BillToCountry { get; set; }
        public object BillToEmail { get; set; }
        public object BillToFirstName { get; set; }
        public object BillToLastName { get; set; }
        public object BillToName { get; set; }
        public string? BillToPhoneNumber { get; set; }
        public object BillToPostalZipCode { get; set; }
        public object BillToState { get; set; }
        public object Brands { get; set; }
        public string BusinessNumber { get; set; }
        public object CardsOnFile { get; set; }
        public object CartonBreakRuleId { get; set; }
        public string CompanyName { get; set; }
        public object CountryId { get; set; }
        public object CreditCardList { get; set; }
        public object CreditLimit { get; set; }
        public object CsrId { get; set; }
        public string CurrencyCode { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public object Customer3plTypeName { get; set; }
        public object CustomerEmail { get; set; }
        public object CustomerGroupId { get; set; }
        public object CustomerGroupName { get; set; }
        public int? CustomerId { get; set; }
        public int CustomerIdentifierCode { get; set; }
        public bool? CustomerJobFlag { get; set; }
        public object CustomerJobFlagStr { get; set; }
        public string CustomerName { get; set; }
        public object CustomerNumber { get; set; }
        public string? CustomerPhone { get; set; }
        public object CustomerSinceDate { get; set; }
        public object CustomerStatementEmailSentDttm { get; set; }
        public object CustomerStatusId { get; set; }
        public int? CustomerTypeId { get; set; }
        public object CustomerTypeId3PL { get; set; }
        public object CustomerTypeName { get; set; }
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
        public string DefaultAccountReceivableId { get; set; }
        public object DefaultAccountReceivableName { get; set; }
        public string DefaultBillingEmail { get; set; }
        public int? DefaultBillingMethodTypeId { get; set; }
        public object DefaultBillingMethodTypeName { get; set; }
        public object DefaultBillToAddr { get; set; }
        public int? DefaultCarrierId { get; set; }
        public object DefaultCarrierName { get; set; }
        public object DefaultCartonBreakRule { get; set; }
        public object DefaultCustomerMessage { get; set; }
        public int DefaultDeliveryMethodId { get; set; }
        public object DefaultDeliveryMethodName { get; set; }
        public object DefaultDepositAccountId { get; set; }
        public object DefaultDepositAccountName { get; set; }
        public object DefaultFOBId { get; set; }
        public object DefaultFOBName { get; set; }
        public object DefaultIncomeAccntId { get; set; }
        public object DefaultIncomeAccntName { get; set; }
        public object DefaultIncomeReturnAccntId { get; set; }
        public object DefaultIncomeReturnAccntName { get; set; }
        public object DefaultLiabilityAccountId { get; set; }
        public object DefaultLiabilityAccountName { get; set; }
        public object DefaultOrderTypeId { get; set; }
        public object DefaultOrderTypeName { get; set; }
        public int? DefaultPaymentMethodId { get; set; }
        public object DefaultPaymentMethodName { get; set; }
        public object DefaultPaymentMethodTypeId { get; set; }
        public object DefaultPaymentTermId { get; set; }
        public object DefaultPaymentTermName { get; set; }
        public object DefaultProjectClassId { get; set; }
        public object DefaultProjectClassName { get; set; }
        public string DefaultShipmentUpdatesEmail { get; set; }
        public object DefaultShippingTermId { get; set; }
        public object DefaultShippingTermName { get; set; }
        public int? DefaultShipServiceId { get; set; }
        public object DefaultShipServiceName { get; set; }
        public object DefaultShipToAddr { get; set; }
        public object DefaultWaveTemplate { get; set; }
        public float DepositAvailable { get; set; }
        public object DepositRequiredAmount { get; set; }
        public object DepositRequiredType { get; set; }
        public object DepositRequiredTypeName { get; set; }
        public float DepositTotal { get; set; }
        public bool DisableSellPackageAllocation { get; set; }
        public object DisableSellPackageAllocationStr { get; set; }
        public bool DoNotUpDateCustomer { get; set; }
        public object DueReminderDays { get; set; }
        public string EmailCc { get; set; }
        public string EmailMain { get; set; }
        public bool EnableCustomerStatementEmailFlag { get; set; }
        public object EntityUseCode { get; set; }
        public object ExchangeRate { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public bool ForceCustomerTaxCode { get; set; }
        public object ForceCustomerTaxCodeStr { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public object ImageList { get; set; }
        public object ImportError { get; set; }
        public string IntAccntId { get; set; }
        public float InvoiceBalance { get; set; }
        public bool IsBcPstTaxExempt { get; set; }
        public bool IsMbPstTaxExempt { get; set; }
        public bool IsPackandHoldRequired { get; set; }
        public bool IsPortalActive { get; set; }
        public bool IsPortalUser { get; set; }
        public bool IsQbPstTaxExempt { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsSkPstTaxExempt { get; set; }
        public bool IsTaxableFlag { get; set; }
        public object IsTaxableFlagStr { get; set; }
        public bool IsVASRequired { get; set; }
        public object[] ItemBrandList { get; set; }
        public int JobDepth { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public object LinkedPaymentGatewayId { get; set; }
        public int LinkedPaymentGatewayTypeId { get; set; }
        public string MainPhone { get; set; }
        public string MobilePhone { get; set; }
        public float NetBalance { get; set; }
        public float NetTotal { get; set; }
        public bool OnHold { get; set; }
        public string OnHoldMessage { get; set; }
        public object OpeningBalance { get; set; }
        public object OpeningBalanceDate { get; set; }
        public string OtherContactInfo { get; set; }
        public object ParentId { get; set; }
        public object ParentName { get; set; }
        public object PermissionGroup { get; set; }
        public object PermissionGroupId { get; set; }
        public string PortalAccountNumber { get; set; }
        public object PortalUrl { get; set; }
        public object PreviousName { get; set; }
        public bool ProcessCardAsDefault { get; set; }
        public float ReceivableTotal { get; set; }
        public object RefId { get; set; }
        public string RepId { get; set; }
        public object ReportDataObj { get; set; }
        public bool ReqFromCustomerPortal { get; set; }
        public bool RestrictDailyInvoiceEmailFlag { get; set; }
        public bool RestrictDueInvoiceEmailFlag { get; set; }
        public bool RestrictNewCustomerForSameEmail { get; set; }
        public bool RestrictOverdueInvoiceEmailFlag { get; set; }
        public bool RestrictReleaseSoEmailFlag { get; set; }
        public bool RestrictShipConfirmationEmailFlag { get; set; }
        public object SalesRepEmail { get; set; }
        public object SalesRepId { get; set; }
        public object SalesRepName { get; set; }
        public string ShippingAccountNumber { get; set; }
        public object ShippingNotes { get; set; }
        public string ShipToAddr { get; set; }
        public string ShipToAddr2 { get; set; }
        public Shiptoaddrlist[] ShipToAddrList { get; set; }
        public object ShipToCity { get; set; }
        public object ShipToCompanyName { get; set; }
        public object ShipToCountry { get; set; }
        public object ShipToEmail { get; set; }
        public object ShipToFirstName { get; set; }
        public object ShipToLastName { get; set; }
        public object ShipToName { get; set; }
        public string? ShipToPhoneNumber { get; set; }
        public object ShipToPostalZipCode { get; set; }
        public object ShipToState { get; set; }
        public bool ShowInvoiceCreditDeposit { get; set; }
        public bool ShowPayBalance { get; set; }
        public int SsccSeqCounter { get; set; }
        public int SsccSeqCounterExtension { get; set; }
        public object StoreCode { get; set; }
        public object StoreId { get; set; }
        public object StoreName { get; set; }
        public string Tags { get; set; }
        public object TaxableFlagStr { get; set; }
        public object TaxCodeChar { get; set; }
        public object TaxCodeId { get; set; }
        public object TaxExemptMap { get; set; }
        public string TaxNumber1 { get; set; }
        public string TaxNumber2 { get; set; }
        public object ThirdPartyDisplayName { get; set; }
        public object ThirdPartyIconUrl { get; set; }
        public object ThirdPartyRefNo { get; set; }
        public object ThirdPartyRequestRawData { get; set; }
        public object ThirdPartySource { get; set; }
        public object TxnTypeId { get; set; }
        public string UpcCompanyPrefix { get; set; }
        public object VASInstruction { get; set; }
        public int WaveTemplateId { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
    }

    public class Billtoaddrlist
    {
        public string Addr { get; set; }
        public string Addr2 { get; set; }
        public object AddressId { get; set; }
        public int AddressInstructionId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public object AddressVerificationError { get; set; }
        public object AddressVerificationWarning { get; set; }
        public string City { get; set; }
        public object Company { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public object CountryCode { get; set; }
        public int? CustomerId { get; set; }
        public object CustomerName { get; set; }
        public bool DefaultFlag { get; set; }
        public bool DeleteFlag { get; set; }
        public bool EditState { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public object ImportError { get; set; }
        public string IntAccountId { get; set; }
        public bool IsUpdateDefaultBillToAddr { get; set; }
        public bool IsUpdateDefaultShipToAddr { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalZipCode { get; set; }
        public bool ResidentialFlag { get; set; }
        public string ShippingAccountNumber { get; set; }
        public string State { get; set; }
        public object StateCode { get; set; }
    }

    public class Shiptoaddrlist
    {
        public string Addr { get; set; }
        public string Addr2 { get; set; }
        public object AddressId { get; set; }
        public int AddressInstructionId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public object AddressVerificationError { get; set; }
        public object AddressVerificationWarning { get; set; }
        public string City { get; set; }
        public object Company { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public object CountryCode { get; set; }
        public int? CustomerId { get; set; }
        public object CustomerName { get; set; }
        public bool DefaultFlag { get; set; }
        public bool DeleteFlag { get; set; }
        public bool EditState { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public object ImportError { get; set; }
        public string IntAccountId { get; set; }
        public bool IsUpdateDefaultBillToAddr { get; set; }
        public bool IsUpdateDefaultShipToAddr { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalZipCode { get; set; }
        public bool ResidentialFlag { get; set; }
        public string ShippingAccountNumber { get; set; }
        public string State { get; set; }
        public object StateCode { get; set; }
    }


    // New Customer Object - may 31 2024

    public class CustomerRootobject
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public List<CustomerDataNew> Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public int Page { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
        public int TotalPages { get; set; }
    }

    public class CustomerDataNew
    {
        //public object Account3PLCode { get; set; }
        //public int? Account3PLId { get; set; }
        public string AccountNumber { get; set; }
        public object ACHStatusId { get; set; }
        public object ACHStatusName { get; set; }
        public bool ActiveFlag { get; set; }
        public object ActiveFlagStr { get; set; }
        public object AddressShippingAccountNumber { get; set; }
        public string AlertNote { get; set; }
        public object AlertNoteActiveFlag { get; set; }
        public object AllAddressList { get; set; }
        public object AuthorizationDetailList { get; set; }
        public bool AutoCreateCarrierFlag { get; set; }
        public bool AutoProcessPayment { get; set; }
        public object AutoProcessPaymentId { get; set; }
        public object AutoProcessPaymentMethodName { get; set; }
        public float AvailableCredit { get; set; }
        public object AverageDaysToPay { get; set; }
        public object BillingMethodName { get; set; }
        public string? BillToAddr { get; set; }
        public string? BillToAddr2 { get; set; }
        public List<Billtoaddrlist> BillToAddrList { get; set; }
        public object BillToCity { get; set; }
        public object BillToCompanyName { get; set; }
        public object BillToCountry { get; set; }
        public object BillToEmail { get; set; }
        public object BillToFirstName { get; set; }
        public object BillToLastName { get; set; }
        public object BillToName { get; set; }
        public string? BillToPhoneNumber { get; set; }
        public object BillToPostalZipCode { get; set; }
        public object BillToState { get; set; }
        public object Brands { get; set; }
        public string BusinessNumber { get; set; }
        public object CardsOnFile { get; set; }
        public object CartonBreakRuleId { get; set; }
        public string CompanyName { get; set; }
        public object CountryId { get; set; }
        public object CreditCardList { get; set; }
        public object CreditLimit { get; set; }
        public object CsrId { get; set; }
        public string CurrencyCode { get; set; }
        public object CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public object Customer3plTypeName { get; set; }
        public object CustomerEmail { get; set; }
        public object CustomerGroupId { get; set; }
        public object CustomerGroupName { get; set; }
        public int? CustomerId { get; set; }
        public int CustomerIdentifierCode { get; set; }
        public bool? CustomerJobFlag { get; set; }
        public object CustomerJobFlagStr { get; set; }
        public string CustomerName { get; set; }
        public object CustomerNumber { get; set; }
        public string? CustomerPhone { get; set; }
        public object CustomerSinceDate { get; set; }
        public object CustomerStatementEmailSentDttm { get; set; }
        public object CustomerStatusId { get; set; }
        public int? CustomerTypeId { get; set; }
        public object CustomerTypeId3PL { get; set; }
        public object CustomerTypeName { get; set; }
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
        public object DCAddrId { get; set; }
        public string DefaultAccountReceivableId { get; set; }
        public object DefaultAccountReceivableName { get; set; }
        public object DefaultBillingEmail { get; set; }
        public int? DefaultBillingMethodTypeId { get; set; }
        public object DefaultBillingMethodTypeName { get; set; }
        public object DefaultBillToAddr { get; set; }
        public int? DefaultCarrierId { get; set; }
        public object DefaultCarrierName { get; set; }
        public object DefaultCartonBreakRule { get; set; }
        public string DefaultCustomerMessage { get; set; }
        public int DefaultDeliveryMethodId { get; set; }
        public object DefaultDeliveryMethodName { get; set; }
        public object DefaultDepositAccountId { get; set; }
        public object DefaultDepositAccountName { get; set; }
        public object DefaultFOBId { get; set; }
        public object DefaultFOBName { get; set; }
        public object DefaultIncomeAccntId { get; set; }
        public object DefaultIncomeAccntName { get; set; }
        public object DefaultIncomeReturnAccntId { get; set; }
        public object DefaultIncomeReturnAccntName { get; set; }
        public object DefaultLiabilityAccountId { get; set; }
        public object DefaultLiabilityAccountName { get; set; }
        public object DefaultOrderTypeId { get; set; }
        public object DefaultOrderTypeName { get; set; }
        public int? DefaultPaymentMethodId { get; set; }
        public object DefaultPaymentMethodName { get; set; }
        public object DefaultPaymentMethodTypeId { get; set; }
        public object DefaultPaymentTermId { get; set; }
        public object DefaultPaymentTermName { get; set; }
        public object DefaultProjectClassId { get; set; }
        public object DefaultProjectClassName { get; set; }
        public object DefaultShipmentUpdatesEmail { get; set; }
        public object DefaultShippingTermId { get; set; }
        public object DefaultShippingTermName { get; set; }
        public int? DefaultShipServiceId { get; set; }
        public object DefaultShipServiceName { get; set; }
        public object DefaultShipToAddr { get; set; }
        public object DefaultWaveTemplate { get; set; }
        public float DepositAvailable { get; set; }
        public object DepositRequiredAmount { get; set; }
        public object DepositRequiredType { get; set; }
        public object DepositRequiredTypeName { get; set; }
        public float DepositTotal { get; set; }
        public bool DisableSellPackageAllocation { get; set; }
        public object DisableSellPackageAllocationStr { get; set; }
        public bool DoNotUpDateCustomer { get; set; }
        public object DueReminderDays { get; set; }
        public string EmailCc { get; set; }
        public string EmailMain { get; set; }
        public bool EnableCustomerStatementEmailFlag { get; set; }
        public object EntityUseCode { get; set; }
        public object ExchangeRate { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public bool ForceCustomerTaxCode { get; set; }
        public object ForceCustomerTaxCodeStr { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public object ImageList { get; set; }
        public object ImportError { get; set; }
        public string IntAccntId { get; set; }
        public float InvoiceBalance { get; set; }
        public bool IsBcPstTaxExempt { get; set; }
        public bool IsMbPstTaxExempt { get; set; }
        public bool IsPackandHoldRequired { get; set; }
        public bool IsPortalActive { get; set; }
        public bool IsPortalUser { get; set; }
        public bool IsQbPstTaxExempt { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsSkPstTaxExempt { get; set; }
        public bool IsTaxableFlag { get; set; }
        public object IsTaxableFlagStr { get; set; }
        public bool IsVASRequired { get; set; }
        public object[] ItemBrandList { get; set; }
        public int JobDepth { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public object LinkedPaymentGatewayId { get; set; }
        public int LinkedPaymentGatewayTypeId { get; set; }
        public string MainPhone { get; set; }
        public string MobilePhone { get; set; }
        public float NetBalance { get; set; }
        public float NetTotal { get; set; }
        public bool OnHold { get; set; }
        public string OnHoldMessage { get; set; }
        public object OpeningBalance { get; set; }
        public object OpeningBalanceDate { get; set; }
        public string OtherContactInfo { get; set; }
        public object ParentId { get; set; }
        public object ParentName { get; set; }
        public object PermissionGroup { get; set; }
        public object PermissionGroupId { get; set; }
        public string PortalAccountNumber { get; set; }
        public object PortalUrl { get; set; }
        public object PreviousName { get; set; }
        public bool ProcessCardAsDefault { get; set; }
        public float ReceivableTotal { get; set; }
        public object RefId { get; set; }
        public string RepId { get; set; }
        public object ReportDataObj { get; set; }
        public bool ReqFromCustomerPortal { get; set; }
        public bool RestrictDailyInvoiceEmailFlag { get; set; }
        public bool RestrictDueInvoiceEmailFlag { get; set; }
        public bool RestrictNewCustomerForSameEmail { get; set; }
        public bool RestrictOverdueInvoiceEmailFlag { get; set; }
        public bool RestrictReleaseSoEmailFlag { get; set; }
        public bool RestrictShipConfirmationEmailFlag { get; set; }
        public object SalesRepEmail { get; set; }
        public object SalesRepId { get; set; }
        public object SalesRepName { get; set; }
        public string ShippingAccountNumber { get; set; }
        public object ShippingNotes { get; set; }
        public string? ShipToAddr { get; set; }
        public string? ShipToAddr2 { get; set; }
        public List<Shiptoaddrlist> ShipToAddrList { get; set; }
        public object ShipToCity { get; set; }
        public object ShipToCompanyName { get; set; }
        public object ShipToCountry { get; set; }
        public object ShipToEmail { get; set; }
        public object ShipToFirstName { get; set; }
        public object ShipToLastName { get; set; }
        public object ShipToName { get; set; }
        public string? ShipToPhoneNumber { get; set; }
        public object ShipToPostalZipCode { get; set; }
        public object ShipToState { get; set; }
        public bool ShowInvoiceCreditDeposit { get; set; }
        public bool ShowPayBalance { get; set; }
        public int SsccSeqCounter { get; set; }
        public int SsccSeqCounterExtension { get; set; }
        public object StoreCode { get; set; }
        public object StoreId { get; set; }
        public object StoreName { get; set; }
        public bool SyncThirdPartyCustomerAndCompanyDataInXoro { get; set; }
        public string Tags { get; set; }
        public object TaxableFlagStr { get; set; }
        public object TaxCodeChar { get; set; }
        public object TaxCodeId { get; set; }
        public object TaxExemptMap { get; set; }
        public string TaxNumber1 { get; set; }
        public string TaxNumber2 { get; set; }
        public object ThirdPartyCompanyId { get; set; }
        public object ThirdPartyCustomerId { get; set; }
        public object ThirdPartyDisplayName { get; set; }
        public object ThirdPartyIconUrl { get; set; }
        public object ThirdPartyRefNo { get; set; }
        public object ThirdPartyRequestRawData { get; set; }
        public object ThirdPartySource { get; set; }
        public object TxnTypeId { get; set; }
        public string UpcCompanyPrefix { get; set; }
        public object VASInstruction { get; set; }
        public int WaveTemplateId { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
    }

    //public class Billtoaddrlist
    //{
    //    public string Addr { get; set; }
    //    public object Addr2 { get; set; }
    //    public object AddressId { get; set; }
    //    public int AddressInstructionId { get; set; }
    //    public int AddressTypeId { get; set; }
    //    public string AddressTypeName { get; set; }
    //    public object AddressVerificationError { get; set; }
    //    public object AddressVerificationWarning { get; set; }
    //    public string City { get; set; }
    //    public object Company { get; set; }
    //    public string CompanyName { get; set; }
    //    public string Country { get; set; }
    //    public object CountryCode { get; set; }
    //    public object CustomerId { get; set; }
    //    public object CustomerName { get; set; }
    //    public object DCAddressName { get; set; }
    //    public object DCAddrId { get; set; }
    //    public bool DefaultFlag { get; set; }
    //    public bool DeleteFlag { get; set; }
    //    public bool EditState { get; set; }
    //    public string Email { get; set; }
    //    public string FirstName { get; set; }
    //    public int Id { get; set; }
    //    public object ImportError { get; set; }
    //    public string IntAccountId { get; set; }
    //    public bool IsUpdateDefaultBillToAddr { get; set; }
    //    public bool IsUpdateDefaultShipToAddr { get; set; }
    //    public string LastName { get; set; }
    //    public string Name { get; set; }
    //    public object Phone { get; set; }
    //    public string PhoneNumber { get; set; }
    //    public string PostalZipCode { get; set; }
    //    public bool ResidentialFlag { get; set; }
    //    public object ShippingAccountNumber { get; set; }
    //    public string State { get; set; }
    //    public object StateCode { get; set; }
    //}

    //public class Shiptoaddrlist
    //{
    //    public string Addr { get; set; }
    //    public object Addr2 { get; set; }
    //    public object AddressId { get; set; }
    //    public int AddressInstructionId { get; set; }
    //    public int AddressTypeId { get; set; }
    //    public string AddressTypeName { get; set; }
    //    public object AddressVerificationError { get; set; }
    //    public object AddressVerificationWarning { get; set; }
    //    public string City { get; set; }
    //    public object Company { get; set; }
    //    public string CompanyName { get; set; }
    //    public string Country { get; set; }
    //    public object CountryCode { get; set; }
    //    public object CustomerId { get; set; }
    //    public object CustomerName { get; set; }
    //    public object DCAddressName { get; set; }
    //    public object DCAddrId { get; set; }
    //    public bool DefaultFlag { get; set; }
    //    public bool DeleteFlag { get; set; }
    //    public bool EditState { get; set; }
    //    public string Email { get; set; }
    //    public string FirstName { get; set; }
    //    public int Id { get; set; }
    //    public object ImportError { get; set; }
    //    public string IntAccountId { get; set; }
    //    public bool IsUpdateDefaultBillToAddr { get; set; }
    //    public bool IsUpdateDefaultShipToAddr { get; set; }
    //    public string LastName { get; set; }
    //    public string Name { get; set; }
    //    public object Phone { get; set; }
    //    public string PhoneNumber { get; set; }
    //    public string PostalZipCode { get; set; }
    //    public bool ResidentialFlag { get; set; }
    //    public object ShippingAccountNumber { get; set; }
    //    public string State { get; set; }
    //    public object StateCode { get; set; }
    //}


    // New Get Customer List Object - Jan 2025

    public class GetCustomerListRootobject
    {
        public bool ConfirmFlag { get; set; }
        public string ConfirmValue { get; set; }
        public string ConfirmValueData { get; set; }
        public List<CustomerDatum> Data { get; set; } = new();
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public string MessageTitle { get; set; }
        public string MetaData { get; set; }
        public int Page { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
        public int TotalPages { get; set; }
    }

    public class CustomerDatum
    {
        public string AccountNumber { get; set; }
        public int? Account3PLId { get; set; }
        public string Account3PLName { get; set; }
        public bool ActiveFlag { get; set; }
        public string AlertNote { get; set; }
        public bool AutoCreateCarrierFlag { get; set; }
        public bool AutoProcessPayment { get; set; }
        public int? AutoProcessPaymentId { get; set; }
        public string AutoProcessPaymentMethodName { get; set; }
        public decimal AvailableCredit { get; set; }
        public int? AverageDaysToPay { get; set; }
        public string BillingMethodName { get; set; }
        public CustomerAddress BillToAddress { get; set; }
        public List<CustomerAddress> BillToAddrList { get; set; } = new();
        public string BusinessNumber { get; set; }
        public string CompanyName { get; set; }
        public int? CountryId { get; set; }
        public decimal? CreditLimit { get; set; }
        public int? CsrId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public string CustomerEmail { get; set; }
        public int CustomerIdentifierCode { get; set; }
        public bool? CustomerJobFlag { get; set; }
        public string CustomerName { get; set; }
        public string CustomerNumber { get; set; }
        public string CustomerPhone { get; set; }
        public DateTime? CustomerSinceDate { get; set; }
        public DateTime? CustomerStatementEmailSentDttm { get; set; }
        public int? CustomerStatusId { get; set; }
        public int? CustomerTypeId { get; set; }
        public string CustomerTypeName { get; set; }
        public List<string> CustomFields { get; set; } = new();
        public decimal DepositAvailable { get; set; }
        public decimal DepositTotal { get; set; }
        public bool DisableSellPackageAllocation { get; set; }
        public string EmailCc { get; set; }
        public string EmailMain { get; set; }
        public bool EnableCustomerStatementEmailFlag { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public bool ForceCustomerTaxCode { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public string IntAccntId { get; set; }
        public decimal InvoiceBalance { get; set; }
        public bool IsPortalActive { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsTaxableFlag { get; set; }
        public List<string> ItemBrandList { get; set; } = new();
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public string MainPhone { get; set; }
        public string MobilePhone { get; set; }
        public decimal NetBalance { get; set; }
        public decimal NetTotal { get; set; }
        public bool OnHold { get; set; }
        public string OnHoldMessage { get; set; }
        public string DefaultPaymentTermName { get; set; }
        public string PortalAccountNumber { get; set; }
        public bool ProcessCardAsDefault { get; set; }
        public decimal ReceivableTotal { get; set; }
        public string RepId { get; set; }
        public bool ReqFromCustomerPortal { get; set; }
        public bool RestrictNewCustomerForSameEmail { get; set; }
        public bool RestrictOverdueInvoiceEmailFlag { get; set; }
        public List<CustomerAddress> ShipToAddrList { get; set; } = new();
        public string ShippingAccountNumber { get; set; }
        public string Tags { get; set; }
        public string TaxNumber1 { get; set; }
        public string TaxNumber2 { get; set; }
        public int WaveTemplateId { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
    }

    public class CustomerAddress
    {
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public int Id { get; set; }
        public string LastName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalZipCode { get; set; }
        public bool ResidentialFlag { get; set; }
        public string ShippingAccountNumber { get; set; }
        public string State { get; set; }
        public bool DefaultFlag { get; set; }
    }




    // XORO CUSTOMER FOR NUORDER COMPANY

    public class XOROCustRootobject
    {
        public bool ConfirmFlag { get; set; }
        public object ConfirmValue { get; set; }
        public object ConfirmValueData { get; set; }
        public List<XOROCustDatum> Data { get; set; }
        public bool EndRequest { get; set; }
        public int ErrorCode { get; set; }
        public bool IsForwardRequest { get; set; }
        public bool LogFlag { get; set; }
        public string Message { get; set; }
        public object MessageTitle { get; set; }
        public string MetaData { get; set; }
        public int Page { get; set; }
        public bool Result { get; set; }
        public int SelectorErrorCode { get; set; }
        public int TotalPages { get; set; }
    }

    public class XOROCustDatum
    {
        public object Account3PLCode { get; set; }
        public object Account3PLId { get; set; }
        public object Account3PLName { get; set; }
        public string AccountNumber { get; set; }
        public object ACHStatusId { get; set; }
        public object ACHStatusName { get; set; }
        public bool ActiveFlag { get; set; }
        public object ActiveFlagStr { get; set; }
        public object AddressShippingAccountNumber { get; set; }
        public string AlertNote { get; set; }
        public object AlertNoteActiveFlag { get; set; }
        public object AllAddressList { get; set; }
        public object AuthorizationDetailList { get; set; }
        public bool AutoCreateCarrierFlag { get; set; }
        public bool AutoProcessPayment { get; set; }
        public object AutoProcessPaymentId { get; set; }
        public object AutoProcessPaymentMethodName { get; set; }
        public float AvailableCredit { get; set; }
        public object AverageDaysToPay { get; set; }
        public object BillingMethodName { get; set; }
        public string? BillToAddr { get; set; }
        public string? BillToAddr2 { get; set; }
        public List<Billtoaddrlist1> BillToAddrList { get; set; }
        public object BillToCity { get; set; }
        public object BillToCompanyName { get; set; }
        public object BillToCountry { get; set; }
        public object BillToEmail { get; set; }
        public object BillToFirstName { get; set; }
        public object BillToLastName { get; set; }
        public object BillToName { get; set; }
        public string? BillToPhoneNumber { get; set; }
        public object BillToPostalZipCode { get; set; }
        public object BillToState { get; set; }
        public object Brands { get; set; }
        public string BusinessNumber { get; set; }
        public object CardsOnFile { get; set; }
        public object CarrierShippingAccntNumber { get; set; }
        public object CartonBreakRuleId { get; set; }
        public string CompanyName { get; set; }
        public object CountryId { get; set; }
        public object CreditCardList { get; set; }
        public object CreditLimit { get; set; }
        public object CsrId { get; set; }
        public string CurrencyCode { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencySymbol { get; set; }
        public object Customer3plTypeName { get; set; }
        public object CustomerEmail { get; set; }
        public object CustomerGroupId { get; set; }
        public string CustomerGroupName { get; set; }
        public int? CustomerId { get; set; }
        public int CustomerIdentifierCode { get; set; }
        public bool CustomerJobFlag { get; set; }
        public object CustomerJobFlagStr { get; set; }
        public string CustomerName { get; set; }
        public object CustomerNumber { get; set; }
        public string? CustomerPhone { get; set; }
        public object CustomerSinceDate { get; set; }
        public object CustomerStatementEmailSentDttm { get; set; }
        public object CustomerStatusId { get; set; }
        public int? CustomerTypeId { get; set; }
        public object CustomerTypeId3PL { get; set; }
        public object CustomerTypeName { get; set; }
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
        public object DCAddrId { get; set; }
        public string DefaultAccountReceivableId { get; set; }
        public object DefaultAccountReceivableName { get; set; }
        public object DefaultBillingEmail { get; set; }
        public int? DefaultBillingMethodTypeId { get; set; }
        public object DefaultBillingMethodTypeName { get; set; }
        public object DefaultBillToAddr { get; set; }
        public object DefaultCarrierId { get; set; }
        public object DefaultCarrierName { get; set; }
        public object DefaultCartonBreakRule { get; set; }
        public string DefaultCustomerMessage { get; set; }
        public int DefaultDeliveryMethodId { get; set; }
        public object DefaultDeliveryMethodName { get; set; }
        public object DefaultDepositAccountId { get; set; }
        public object DefaultDepositAccountName { get; set; }
        public object DefaultFOBId { get; set; }
        public object DefaultFOBName { get; set; }
        public object DefaultIncomeAccntId { get; set; }
        public object DefaultIncomeAccntName { get; set; }
        public object DefaultIncomeReturnAccntId { get; set; }
        public object DefaultIncomeReturnAccntName { get; set; }
        public object DefaultLiabilityAccountId { get; set; }
        public object DefaultLiabilityAccountName { get; set; }
        public object DefaultOrderTypeId { get; set; }
        public object DefaultOrderTypeName { get; set; }
        public int? DefaultPaymentMethodId { get; set; }
        public object DefaultPaymentMethodName { get; set; }
        public object DefaultPaymentMethodTypeId { get; set; }
        public object DefaultPaymentTermId { get; set; }
        public string DefaultPaymentTermName { get; set; }
        public object DefaultProjectClassId { get; set; }
        public object DefaultProjectClassName { get; set; }
        public object DefaultShipmentUpdatesEmail { get; set; }
        public object DefaultShippingTermId { get; set; }
        public object DefaultShippingTermName { get; set; }
        public object DefaultShipServiceId { get; set; }
        public object DefaultShipServiceName { get; set; }
        public object DefaultShipToAddr { get; set; }
        public object DefaultWaveTemplate { get; set; }
        public float DepositAvailable { get; set; }
        public object DepositRequiredAmount { get; set; }
        public object DepositRequiredType { get; set; }
        public object DepositRequiredTypeName { get; set; }
        public float DepositTotal { get; set; }
        public bool DisableSellPackageAllocation { get; set; }
        public object DisableSellPackageAllocationStr { get; set; }
        public bool DoNotUpDateCustomer { get; set; }
        public object DueReminderDays { get; set; }
        public string EmailCc { get; set; }
        public string EmailMain { get; set; }
        public bool EnableCustomerStatementEmailFlag { get; set; }
        public object EntityUseCode { get; set; }
        public object ExchangeRate { get; set; }
        public string Fax { get; set; }
        public string FirstName { get; set; }
        public bool ForceCustomerTaxCode { get; set; }
        public object ForceCustomerTaxCodeStr { get; set; }
        public string FullName { get; set; }
        public int Id { get; set; }
        public object[] ImageList { get; set; }
        public object ImageUrls { get; set; }
        public object ImportError { get; set; }
        public string IntAccntId { get; set; }
        public float InvoiceBalance { get; set; }
        public bool IsBcPstTaxExempt { get; set; }
        public bool IsMbPstTaxExempt { get; set; }
        public bool IsPackandHoldRequired { get; set; }
        public bool IsPortalActive { get; set; }
        public bool IsPortalUser { get; set; }
        public bool IsQbPstTaxExempt { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsSkPstTaxExempt { get; set; }
        public bool IsTaxableFlag { get; set; }
        public object IsTaxableFlagStr { get; set; }
        public bool IsVASRequired { get; set; }
        public object[] ItemBrandList { get; set; }
        public int JobDepth { get; set; }
        public string JobTitle { get; set; }
        public string LastName { get; set; }
        public object LinkedPaymentGatewayId { get; set; }
        public int LinkedPaymentGatewayTypeId { get; set; }
        public string MainPhone { get; set; }
        public string MobilePhone { get; set; }
        public float NetBalance { get; set; }
        public float NetTotal { get; set; }
        public bool OnHold { get; set; }
        public string OnHoldMessage { get; set; }
        public object OpeningBalance { get; set; }
        public object OpeningBalanceDate { get; set; }
        public string OtherContactInfo { get; set; }
        public object ParentId { get; set; }
        public object ParentName { get; set; }
        public object PermissionGroup { get; set; }
        public object PermissionGroupId { get; set; }
        public string PortalAccountNumber { get; set; }
        public object PortalUrl { get; set; }
        public object PreviousName { get; set; }
        public bool ProcessCardAsDefault { get; set; }
        public float ReceivableTotal { get; set; }
        public object RefId { get; set; }
        public string RepId { get; set; }
        public object ReportDataObj { get; set; }
        public bool ReqFromCustomerPortal { get; set; }
        public bool RestrictDailyInvoiceEmailFlag { get; set; }
        public bool RestrictDueInvoiceEmailFlag { get; set; }
        public bool RestrictNewCustomerForSameEmail { get; set; }
        public bool RestrictOverdueInvoiceEmailFlag { get; set; }
        public bool RestrictReleaseSoEmailFlag { get; set; }
        public bool RestrictShipConfirmationEmailFlag { get; set; }
        public string? SalesRepEmail { get; set; }
        public string? SalesRepId { get; set; }
        public string? SalesRepName { get; set; }
        public string ShippingAccountNumber { get; set; }
        public object ShippingNotes { get; set; }
        public string? ShipToAddr { get; set; }
        public string? ShipToAddr2 { get; set; }
        public List<Shiptoaddrlist1> ShipToAddrList { get; set; }
        public object ShipToCity { get; set; }
        public object ShipToCompanyName { get; set; }
        public object ShipToCountry { get; set; }
        public object ShipToEmail { get; set; }
        public object ShipToFirstName { get; set; }
        public object ShipToLastName { get; set; }
        public object ShipToName { get; set; }
        public string? ShipToPhoneNumber { get; set; }
        public object ShipToPostalZipCode { get; set; }
        public object ShipToState { get; set; }
        public bool ShowInvoiceCreditDeposit { get; set; }
        public bool ShowPayBalance { get; set; }
        public int SsccSeqCounter { get; set; }
        public int SsccSeqCounterExtension { get; set; }
        public object StoreCode { get; set; }
        public object StoreId { get; set; }
        public object StoreName { get; set; }
        public bool SyncThirdPartyCustomerAndCompanyDataInXoro { get; set; }
        public string Tags { get; set; }
        public object TaxableFlagStr { get; set; }
        public object TaxCodeChar { get; set; }
        public object TaxCodeId { get; set; }
        public object TaxExemptMap { get; set; }
        public string TaxNumber1 { get; set; }
        public string TaxNumber2 { get; set; }
        public object ThirdPartyCompanyId { get; set; }
        public object ThirdPartyCustomerId { get; set; }
        public object ThirdPartyDisplayName { get; set; }
        public object ThirdPartyIconUrl { get; set; }
        public object ThirdPartyRefNo { get; set; }
        public object ThirdPartyRequestRawData { get; set; }
        public object ThirdPartySource { get; set; }
        public object TxnTypeId { get; set; }
        public string UpcCompanyPrefix { get; set; }
        public object VASInstruction { get; set; }
        public int WaveTemplateId { get; set; }
        public string Website { get; set; }
        public string WorkPhone { get; set; }
    }

    public class Billtoaddrlist1
    {
        public string Addr { get; set; }
        public string? Addr2 { get; set; }
        public object AddressId { get; set; }
        public int AddressInstructionId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public object AddressVerificationError { get; set; }
        public object AddressVerificationWarning { get; set; }
        public string City { get; set; }
        public object Company { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public object CountryCode { get; set; }
        public int? CustomerId { get; set; }
        public object CustomerName { get; set; }
        public object DCAddressName { get; set; }
        public object DCAddrId { get; set; }
        public bool DefaultFlag { get; set; }
        public bool DeleteFlag { get; set; }
        public bool EditState { get; set; }
        public string Email { get; set; }
        public object FirstName { get; set; }
        public int Id { get; set; }
        public object ImportError { get; set; }
        public string IntAccountId { get; set; }
        public bool IsUpdateDefaultBillToAddr { get; set; }
        public bool IsUpdateDefaultShipToAddr { get; set; }
        public object LastName { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalZipCode { get; set; }
        public bool ResidentialFlag { get; set; }
        public object ShippingAccountNumber { get; set; }
        public string State { get; set; }
        public object StateCode { get; set; }
    }

    public class Shiptoaddrlist1
    {
        public string Addr { get; set; }
        public string? Addr2 { get; set; }
        public object AddressId { get; set; }
        public int AddressInstructionId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressTypeName { get; set; }
        public object AddressVerificationError { get; set; }
        public object AddressVerificationWarning { get; set; }
        public string City { get; set; }
        public object Company { get; set; }
        public string CompanyName { get; set; }
        public string Country { get; set; }
        public object CountryCode { get; set; }
        public int? CustomerId { get; set; }
        public object CustomerName { get; set; }
        public object DCAddressName { get; set; }
        public object DCAddrId { get; set; }
        public bool DefaultFlag { get; set; }
        public bool DeleteFlag { get; set; }
        public bool EditState { get; set; }
        public string Email { get; set; }
        public object FirstName { get; set; }
        public int Id { get; set; }
        public object ImportError { get; set; }
        public string IntAccountId { get; set; }
        public bool IsUpdateDefaultBillToAddr { get; set; }
        public bool IsUpdateDefaultShipToAddr { get; set; }
        public object LastName { get; set; }
        public string Name { get; set; }
        public string? Phone { get; set; }
        public string PhoneNumber { get; set; }
        public string PostalZipCode { get; set; }
        public bool ResidentialFlag { get; set; }
        public object ShippingAccountNumber { get; set; }
        public string State { get; set; }
        public object StateCode { get; set; }
    }

}
