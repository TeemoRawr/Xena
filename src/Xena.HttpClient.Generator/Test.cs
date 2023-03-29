namespace Test
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    interface ITemplateApiService
    {
        String Template_GetTemplateApi([RequiredAttribute] string templateId);
        String PutTemplateApi([RequiredAttribute] string templateId);
        String GetRawTemplateApi([RequiredAttribute] string templateId);
    }

    interface IMetadataApiService
    {
        String PutMetadataApi([RequiredAttribute] int metadataId);
        String GetMetadataApi([RequiredAttribute] string fileName, [RequiredAttribute] string rootId);
        String GetMetadataQuery(int take, string sortBy, string sortDirection, string fileName, string templateRootId, string merchantId, string fileType, string fileTypeNot, string status, string statusNot, string fieldsNotNull);
        String GetUploadBatches();
        String PutUploadBatchesCompleted();
    }

    interface ITemplateMetadataApiService
    {
        String GetAssetsMetadataApi([RequiredAttribute] string templateRootId);
    }

    interface IRenderPrivacyPageApiService
    {
        String PostRenderPrivacyPage();
    }

    interface IRenderStaticPageApiService
    {
        String PostRenderStaticPage();
    }

    interface IRenderPaymentmethodSelectionPageApiService
    {
        String PostRenderPaymentmethodSelectionPage();
    }

    interface IRenderPaymentmethodFormPageApiService
    {
        String PostRenderPaymentmethodFormPage();
    }

    interface IRenderStatusPageApiService
    {
        String PostRenderStatusPage();
    }

    interface IApiService
    {
        String GetCustomizationApi([RequiredAttribute] int customizationId);
        String DeleteCustomizationApi([RequiredAttribute] int customizationId);
        String GetMerchantCustomizations([RequiredAttribute] string merchantId);
        String PutMerchantCustomization([RequiredAttribute] string merchantId);
        String PostCustomizationPreview([RequiredAttribute] string merchantId);
    }

    public class CustomizationPreview
    {
        ///<summary>optional, the name of an existing customization to retrieve the logo. Only used if field logo is empty</summary>
        public string Name { set; get; }

        ///<summary>the host (domain name) the consumer is on (eg- worldline-solutions.com)</summary>
        public string Host { set; get; }

        ///<summary>the branding to be used in case host is not provided or cannot be used</summary>
        public string Branding { set; get; }

        ///<summary>Language used to render the template. Either use : Id (1 for english, 2 for french, 3 for nl_be...), short code (EN, FR, NL...) or long code (EN_EN, FR_FR, NL_BE...)</summary>
        [RequiredAttribute]
        public LanguageIdOrCode Language { set; get; }

        CustomizationPreviewPage Page { set; get; }

        [RequiredAttribute]
        public CustomizationDetails CustomizationDetails { set; get; }

        public InlineImage Logo { set; get; }
    }

    public class Customization
    {
        [RequiredAttribute]
        public int Id { set; get; }

        [RequiredAttribute]
        public string Name { set; get; }

        public DateTime InsertDate { set; get; }

        [RequiredAttribute]
        public DateTime UpdateDate { set; get; }

        [RequiredAttribute]
        public CustomizationDetails CustomizationDetails { set; get; }

        public InlineImage Logo { set; get; }
    }

    public class PutCustomization
    {
        [RequiredAttribute]
        public string Name { set; get; }

        public InlineImage Logo { set; get; }

        [RequiredAttribute]
        public CustomizationDetails CustomizationDetails { set; get; }
    }

    public class InlineImage
    {
        public string Base64 { set; get; }

        InlineImageFormat Format { set; get; }

        ///<summary>alt attribute of image</summary>
        public string Alt { set; get; }
    }

    public class CustomizationDetails
    {
        public string Title { set; get; }

        public string ContentBackgroundColor { set; get; }

        public string ContentTextColor { set; get; }

        public string BannerBackgroundColor { set; get; }

        public string BannerTextColor { set; get; }

        public string FooterBackgroundColor { set; get; }

        public string FooterTextColor { set; get; }

        public string SubmitButtonBackgroundColor { set; get; }

        public string SubmitButtonTextColor { set; get; }

        public string SubmitButtonBackgroundHoverColor { set; get; }

        public string SubmitButtonTextHoverColor { set; get; }
    }

    public class Customizations
    {
    }

    public class PrivacyPageRenderRequest
    {
        public PrivacyPageData PrivacyPageData { set; get; }

        [RequiredAttribute]
        public TemplateRenderRequest TemplateRenderRequest { set; get; }
    }

    public class StaticPageRenderRequest
    {
        public StaticPageData StaticPageData { set; get; }

        [RequiredAttribute]
        public TemplateRenderRequest TemplateRenderRequest { set; get; }
    }

    public class PaymentmethodSelectionPageRenderRequest
    {
        [RequiredAttribute]
        public PaymentMethodSelectionData PaymentMethodSelectionData { set; get; }

        [RequiredAttribute]
        public TemplateRenderRequest TemplateRenderRequest { set; get; }
    }

    public class PaymentmethodFormPageRenderRequest
    {
        [RequiredAttribute]
        public PaymentMethodFormData PaymentMethodFormData { set; get; }

        [RequiredAttribute]
        public TemplateRenderRequest TemplateRenderRequest { set; get; }
    }

    public class StatusPageRenderRequest
    {
        [RequiredAttribute]
        public StatusPageData StatusPageData { set; get; }

        [RequiredAttribute]
        public TemplateRenderRequest TemplateRenderRequest { set; get; }
    }

    public class PrivacyPageData
    {
        [RequiredAttribute]
        public IReadOnlyList<PrivacyText> PrivacyTexts { set; get; }
    }

    public class StaticPageData
    {
        [RequiredAttribute]
        public int MemoId { set; get; }

        public IReadOnlyList<object> Placeholders { set; get; }
    }

    ///<summary>Display the status of the payment flow</summary>
    public class ConsumerMessageTitle
    {
    }

    ///<summary>Display the status of the payment flow</summary>
    public class ConsumerMessageText
    {
    }

    public class ConsumerMessageButton
    {
    }

    public class StatusPageData
    {
        ///<summary>Display the status of the payment flow</summary>
        public ConsumerMessageTitle Title { set; get; }

        ///<summary>Display the status of the payment flow</summary>
        public ConsumerMessageText Text { set; get; }

        ///<summary>Display the status of the payment flow</summary>
        public ConsumerMessageText GiftCardBalanceText { set; get; }

        public string ErrorCode { set; get; }

        public string ThirdPartyErrorMessage { set; get; }

        public Uri RedirectButtonAction { set; get; }

        public ConsumerMessageButton RedirectButtonText { set; get; }

        public IReadOnlyList<GiftCardBalance> GiftCardBalances { set; get; }

        public int AutoSubmitDelay { set; get; }

        [RequiredAttribute]
        public DateTime IsMethodGet { set; get; }

        public SplitPaymentStatusData SplitPaymentStatusData { set; get; }

        ///<summary>List of extra parameters to be added in forms</summary>
        public StatusPageDataFormParameters FormParameters { set; get; }
    }

    public class GiftCardBalance
    {
        public string Brand { set; get; }

        public string Balance { set; get; }

        public string Currency { set; get; }
    }

    public class PaymentMethodFormData
    {
        public string Brand { set; get; }

        public string BrandVisual { set; get; }

        public string BrandDisplayNameOverride { set; get; }

        public string Acquirer { set; get; }

        ///<summary>The list of MetaDataElement from Ingenico.PaymentEngine.ApiContracts serialized as Json</summary>
        public string MetadataListSerialized { set; get; }

        public PaymentMethodFormDataFormErrors FormErrors { set; get; }

        ///<summary>Uri used for the form submission</summary>
        public Uri ActionFormSubmit { set; get; }

        ///<summary>Default values for form elements. Overrides MetaData default value.</summary>
        public PaymentMethodFormDataFormInputValues FormInputValues { set; get; }

        ///<summary>Render required scripts for a paymenht method</summary>
        public IReadOnlyList<string> RequiredScripts { set; get; }

        ///<summary>Data used to render the alias creation inputs</summary>
        public AliasCreation AliasCreation { set; get; }

        ///<summary>List of extra parameters to be added in forms</summary>
        public PaymentMethodFormDataFormParameters FormParameters { set; get; }

        ///<summary>Uri used for the ChangePaymentMethod link</summary>
        public Uri LinkChangePaymentMethod { set; get; }

        ///<summary>File name of the placeholder image displayed while the brand of the card has not been detected</summary>
        public string BrandPlaceholderImage { set; get; }
    }

    public class PaymentMethodSelectionData
    {
        public IReadOnlyList<PaymentMethodItem> PaymentMethods { set; get; }

        public IReadOnlyList<AliasItem> Aliases { set; get; }

        public Uri ActionPaymentMethodSelect { set; get; }

        public string NoScriptStyleSheet { set; get; }

        public IReadOnlyList<string> RequiredScripts { set; get; }
    }

    public class AliasItem
    {
        public string Alias { set; get; }

        public string Brand { set; get; }

        public string DisplayValue { set; get; }
    }

    public class TemplateRenderRequest
    {
        ///<summary>General data used by the template engine</summary>
        [RequiredAttribute]
        public TemplateData TemplateData { set; get; }

        public IReadOnlyList<LineItem> LineItems { set; get; }

        ///<summary>Data used to render the alias creation inputs</summary>
        public AliasCreation AliasCreation { set; get; }

        ///<summary>Used to display split payment details in payment summary. Only displays something in case of a split payment.</summary>
        public SplitPaymentSummary SplitPaymentSummary { set; get; }

        public Uri ActionCancel { set; get; }

        public DisplayElements DisplayElements { set; get; }

        [RequiredAttribute]
        public DisplayOptions DisplayOptions { set; get; }

        ///<summary>Deprecated: Set PaymentMethodFormData.FormParameters instead</summary>
        public TemplateRenderRequestFormParameters FormParameters { set; get; }

        public Dcc Dcc { set; get; }

        public Surcharge Surcharge { set; get; }
    }

    public class Dcc
    {
        public IReadOnlyList<DccCurrencyTypes> DccCurrencyTypes { set; get; }

        public DccData DccData { set; get; }
    }

    public class DccCurrencyTypes
    {
    }

    public class DccData
    {
        public string CardType { set; get; }

        public string CardholderName { set; get; }

        public string TransactionTotalBaseAmount { set; get; }

        public string TransactionCurrency { set; get; }

        public string TransactionTotalTargetAmount { set; get; }

        public string ExchangeRate { set; get; }

        public string BaseCurrencyCode { set; get; }

        public string TargetCurrencyCode { set; get; }
    }

    public class Surcharge
    {
        ///<summary>Surcharge amount line item to be displayed</summary>
        public string Amount { set; get; }

        ///<summary>New transaction total including surcharge</summary>
        public string TotalAmount { set; get; }

        public SurchargeMode SurchargeMode { set; get; }
    }

    public class SurchargeMode
    {
    }

    ///<summary>General data used by the template engine</summary>
    public class TemplateData
    {
        ///<summary>Used to retrieve the template</summary>
        public string PspId { set; get; }

        ///<summary>Indicates the template requested at transaction time</summary>
        public string RequestedTemplate { set; get; }

        ///<summary>When "Template" field is provided, indicate where to find it</summary>
        public string TemplateRootId { set; get; }

        [RequiredAttribute]
        public ResourcesData ResourcesData { set; get; }

        public LinkData Links { set; get; }

        ///<summary>Payment Zone template definition.</summary>
        public string PaymentZone { set; get; }

        ///<summary>Language used to render the template. Either use : Id (1 for english, 2 for french, 3 for nl_be...), short code (EN, FR, NL...) or long code (EN_EN, FR_FR, NL_BE...)</summary>
        public LanguageIdOrCode Language { set; get; }

        ///<summary>Use the language as part of the search lookup for default template</summary>
        [RequiredAttribute]
        public DateTime UseLanguageForDefaultTemplate { set; get; }

        ///<summary>Content of the default template</summary>
        public string DefaultTemplateContent { set; get; }

        ///<summary>This is the host the consumer is using to access the application. Used to determine the branding.</summary>
        public string Host { set; get; }

        ///<summary>Branding to use in case no branding are found for this host.</summary>
        public string DefaultBranding { set; get; }

        ///<summary>In which store we want to select the template</summary>
        TemplateDataTemplateStore TemplateStore { set; get; }

        ///<summary>Used to target correct branding resource files when in legacy mode</summary>
        [RequiredAttribute]
        public DateTime IsProd { set; get; }

        ///<summary>Mode used to choose the default template (eg: STD, CIM, MAS...)</summary>
        public string Mode { set; get; }

        ///<summary>Value used to choose the default template</summary>
        [RequiredAttribute]
        public DateTime IsUtf8 { set; get; }

        ///<summary>Css to be included inline</summary>
        public string InlineCss { set; get; }

        ///<summary>Js to be included inline</summary>
        public string InlineJs { set; get; }

        ///<summary>Logo to be included inline</summary>
        public string InlineLogo { set; get; }

        ///<summary>List of fields used to customize the page</summary>
        public LegacyCustomizationParameters LegacyCustomizationParameters { set; get; }

        ///<summary>Configuration options for the template engine</summary>
        public ParserConfiguration ParserConfiguration { set; get; }

        ///<summary>The PartialTemplateKeys are used to look for partial templates. Usually this is the PaymentMethod and Brand.</summary>
        public IReadOnlyList<string> PartialTemplateKeys { set; get; }

        ///<summary>CFG7 &amp; 2^10 [1024]</summary>
        [RequiredAttribute]
        public DateTime DisableJavascriptCheck { set; get; }
    }

    public class ResourcesData
    {
        ///<summary>Web URI to the static files hosted by the channel (eg: http://localhost/cdn)</summary>
        public Uri ResourcesUri { set; get; }

        ///<summary>SubResourceIntegrities</summary>
        public ResourcesDataSris Sris { set; get; }

        ///<summary>This value is ignored</summary>
        public Uri TemplateResourcesUri { set; get; }

        ///<summary>Path to Brand Images</summary>
        public Uri BrandImagesBaseUri { set; get; }

        ///<summary>The extension for the brand images (eg: .gif)</summary>
        public string BrandImagesExtension { set; get; }

        ///<summary>This value is ignored</summary>
        public Uri BrandingImagesBaseUri { set; get; }

        ///<summary>This value is ignored</summary>
        public string BrandingImagesExtension { set; get; }

        ///<summary>This value is ignored</summary>
        public Uri MerchantTemplateResourcesUri { set; get; }

        ///<summary>Web URI where the legacy files are hosted (eg: https://webdev.oglan.local/). This URI is only used when rendering inside the legacy $$$PAYMENT ZONE$$$</summary>
        public Uri LegacyResourcesUri { set; get; }

        ///<summary>Render scripts</summary>
        public IReadOnlyList<string> ValidationScripts { set; get; }
    }

    public class LinkData
    {
        public Uri LinksBaseUri { set; get; }

        public Uri ActionPrivacyPolicy { set; get; }

        public Uri ActionAbout { set; get; }

        public Uri ActionSecurity { set; get; }

        public Uri ActionLegalInfo { set; get; }
    }

    ///<summary>Language used to render the template. Either use : Id (1 for english, 2 for french, 3 for nl_be...), short code (EN, FR, NL...) or long code (EN_EN, FR_FR, NL_BE...)</summary>
    public class LanguageIdOrCode
    {
        public string LanguageCode { set; get; }

        public int LanguageId { set; get; }
    }

    ///<summary>List of fields used to customize the page</summary>
    public class LegacyCustomizationParameters
    {
        public string Title { set; get; }

        public string Logo { set; get; }

        public string BgColor { set; get; }

        public string TxtColor { set; get; }

        public string TblBgColor { set; get; }

        public string TblTxtColor { set; get; }

        public string ButtonBgColor { set; get; }

        public string ButtonTxtColor { set; get; }

        public string FontType { set; get; }

        public string HdTblBgColor { set; get; }

        public string HdTblTxtColor { set; get; }

        public string HdFontType { set; get; }

        public string RtlCss { set; get; }
    }

    ///<summary>Configuration options for the template engine</summary>
    public class ParserConfiguration
    {
        public DateTime GenerateErrorMessages { set; get; }

        public DateTime GenerateDivAroundElements { set; get; }
    }

    ///<summary>List of Line Items, only used if needed.</summary>
    public class LineItem
    {
        public string Description { set; get; }

        public string ProductName { set; get; }

        public string ProductPrice { set; get; }

        public string TotalAmount { set; get; }

        public string Quantity { set; get; }

        public string Unit { set; get; }

        public string TaxAmount { set; get; }

        public string ProductType { set; get; }

        public string ProductCode { set; get; }

        public string DiscountAmount { set; get; }
    }

    ///<summary>Data used to render the alias creation inputs</summary>
    public class AliasCreation
    {
        public string AliasUsage { set; get; }

        public DateTime IsChecked { set; get; }
    }

    ///<summary>Used to display split payment details in payment summary. Only displays something in case of a split payment.</summary>
    public class SplitPaymentSummary
    {
        public string RemainingAmount { set; get; }

        public IReadOnlyList<SplitPayment> SplitPayments { set; get; }
    }

    public class SplitPayment
    {
        public string BrandName { set; get; }

        public string TotalAmount { set; get; }

        public string MaskedCardNumber { set; get; }
    }

    public class SplitPaymentStatusData
    {
        public string BrandName { set; get; }

        public IReadOnlyList<PaymentMethodItem> PaymentMethods { set; get; }

        public Uri ActionPaymentMethodSelect { set; get; }

        public IReadOnlyList<string> RequiredScripts { set; get; }
    }

    public class PaymentMethodItem
    {
        public string Brand { set; get; }

        public IReadOnlyList<string> BrandVisuals { set; get; }

        public string BrandDisplayNameOverride { set; get; }

        public string Acquirer { set; get; }
    }

    public class DisplayElements
    {
        ///<summary>Display the total amount when a value is provided</summary>
        public string TotalAmount { set; get; }

        public string Currency { set; get; }

        ///<summary>Display the orderId when a value is provided</summary>
        public string OrderId { set; get; }

        ///<summary>Display the paymentId when a value is provided</summary>
        public string PaymentId { set; get; }

        ///<summary>Display the Beneficiary and its address</summary>
        public IReadOnlyList<string> BeneficiaryLines { set; get; }

        ///<summary>Display the VAT ID when a value is provided</summary>
        public string VatIdentificationNumber { set; get; }

        ///<summary>Display the status of the payment flow</summary>
        public FlowStatus FlowStatus { set; get; }
    }

    ///<summary>Display the status of the payment flow</summary>
    public class FlowStatus
    {
    }

    public class DisplayOptions
    {
        ///<summary>Indicates the target device</summary>
        [RequiredAttribute]
        public DateTime IsForMobile { set; get; }

        [RequiredAttribute]
        public DateTime AllowTemplateWithoutPrivacyPolicy { set; get; }

        [RequiredAttribute]
        public DateTime RenderPaymentMethodsHidden { set; get; }

        public string PaymentMethodsHiddenClassName { set; get; }
    }

    public class PrivacyText
    {
        public string Title { set; get; }

        public string Content { set; get; }
    }

    public class Template
    {
        ///<summary>this is id of the template (filename in DB)</summary>
        [RequiredAttribute]
        public string TemplateId { set; get; }

        public string Content { set; get; }
    }

    public class PutTemplateRequest
    {
        [RequiredAttribute]
        public string Content { set; get; }
    }

    public class Metadata
    {
        ///<summary>this is id</summary>
        [RequiredAttribute]
        public int MetadataId { set; get; }

        public string UniqueId { set; get; }

        public string FileName { set; get; }

        [RequiredAttribute]
        public FileType FileType { set; get; }

        ///<summary>this is the pspid</summary>
        public string MerchantId { set; get; }

        public Status Status { set; get; }

        public string StatusDate { set; get; }

        ///<summary>this is the uniqueid</summary>
        [RequiredAttribute]
        public string TemplateId { set; get; }

        public string TemplateRootId { set; get; }

        public string CdnStatus { set; get; }

        public string CdnSri { set; get; }

        public string CdnVersionTag { set; get; }

        public int TechOption { set; get; }

        public string AssetPath { set; get; }
    }

    public class GetAssetsMetadataApiResponse
    {
        public IReadOnlyList<Metadata> TemplateMetadataList { set; get; }
    }

    public class PutMetadataRequest
    {
        [RequiredAttribute]
        public string CdnStatus { set; get; }
    }

    public class QueryMetadataResponse
    {
        public IReadOnlyList<Metadata> MetadataList { set; get; }
    }

    public class UploadBatchesResponse
    {
        public string TemplateRootId { set; get; }

        public IReadOnlyList<Metadata> MetadataList { set; get; }
    }

    public class UploadBatchesCompletedRequest
    {
        public string TemplateRootId { set; get; }

        public string CdnVersionTag { set; get; }

        public IReadOnlyList<CompletedMetadata> MetadataIdList { set; get; }
    }

    public class CompletedMetadata
    {
        ///<summary>this is id</summary>
        [RequiredAttribute]
        public int MetadataId { set; get; }

        public string CdnSri { set; get; }
    }

    public class FileType
    {
    }

    public class Status
    {
    }

    public class TemplateRenderResponse
    {
        public string Html { set; get; }

        public string Nonce { set; get; }
    }

    public enum CustomizationPreviewPage
    {
        PaymentMethodList,
        PaymentMethodForm
    }

    public enum InlineImageFormat
    {
        Png,
        Jpg,
        Gif
    }

    public class StatusPageDataFormParameters
    {
    }

    public class PaymentMethodFormDataFormErrors
    {
    }

    public class PaymentMethodFormDataFormInputValues
    {
    }

    public class PaymentMethodFormDataFormParameters
    {
    }

    public class TemplateRenderRequestFormParameters
    {
    }

    public enum TemplateDataTemplateStore
    {
        HostedCheckout,
        HostedTokenization
    }

    public class ResourcesDataSris
    {
    }
}