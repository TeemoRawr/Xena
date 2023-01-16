namespace MyApplication
{
    using System.Collections.Generic;

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class CustomizationPreview
    {
        public System.String name { set; get; }

        public System.String host { set; get; }

        public System.String branding { set; get; }

        public LanguageIdOrCode language { set; get; }

        public System.String page { set; get; }

        public CustomizationDetails customizationDetails { set; get; }

        public InlineImage logo { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Customization
    {
        public System.Int32 id { set; get; }

        public System.String name { set; get; }

        public System.String insertDate { set; get; }

        public System.String updateDate { set; get; }

        public CustomizationDetails customizationDetails { set; get; }

        public InlineImage logo { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PutCustomization
    {
        public System.String name { set; get; }

        public InlineImage logo { set; get; }

        public CustomizationDetails customizationDetails { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class InlineImage
    {
        public System.String base64 { set; get; }

        public System.String format { set; get; }

        public System.String alt { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class CustomizationDetails
    {
        public System.String title { set; get; }

        public System.String contentBackgroundColor { set; get; }

        public System.String contentTextColor { set; get; }

        public System.String bannerBackgroundColor { set; get; }

        public System.String bannerTextColor { set; get; }

        public System.String footerBackgroundColor { set; get; }

        public System.String footerTextColor { set; get; }

        public System.String submitButtonBackgroundColor { set; get; }

        public System.String submitButtonTextColor { set; get; }

        public System.String submitButtonBackgroundHoverColor { set; get; }

        public System.String submitButtonTextHoverColor { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Customizations
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PrivacyPageRenderRequest
    {
        public PrivacyPageData privacyPageData { set; get; }

        public TemplateRenderRequest templateRenderRequest { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class StaticPageRenderRequest
    {
        public StaticPageData staticPageData { set; get; }

        public TemplateRenderRequest templateRenderRequest { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PaymentmethodSelectionPageRenderRequest
    {
        public PaymentMethodSelectionData paymentMethodSelectionData { set; get; }

        public TemplateRenderRequest templateRenderRequest { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PaymentmethodFormPageRenderRequest
    {
        public PaymentMethodFormData paymentMethodFormData { set; get; }

        public TemplateRenderRequest templateRenderRequest { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class StatusPageRenderRequest
    {
        public StatusPageData statusPageData { set; get; }

        public TemplateRenderRequest templateRenderRequest { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PrivacyPageData
    {
        public IReadOnlyList<PrivacyText> privacyTexts { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class StaticPageData
    {
        public System.Int32 memoId { set; get; }

        public IReadOnlyList<object> placeholders { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class ConsumerMessageTitle
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class ConsumerMessageText
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class ConsumerMessageButton
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class StatusPageData
    {
        public System.String title { set; get; }

        public System.String text { set; get; }

        public System.String giftCardBalanceText { set; get; }

        public System.String errorCode { set; get; }

        public System.String thirdPartyErrorMessage { set; get; }

        public System.String redirectButtonAction { set; get; }

        public System.String redirectButtonText { set; get; }

        public System.Int32 autoSubmitDelay { set; get; }

        public System.Boolean isMethodGet { set; get; }

        public SplitPaymentStatusData splitPaymentStatusData { set; get; }

        public System.Object formParameters { set; get; }

        public IReadOnlyList<GiftCardBalance> giftCardBalances { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class GiftCardBalance
    {
        public System.String brand { set; get; }

        public System.String balance { set; get; }

        public System.String currency { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PaymentMethodFormData
    {
        public System.String brand { set; get; }

        public System.String brandVisual { set; get; }

        public System.String brandDisplayNameOverride { set; get; }

        public System.String acquirer { set; get; }

        public System.String metadataListSerialized { set; get; }

        public System.Object formErrors { set; get; }

        public System.String actionFormSubmit { set; get; }

        public System.Object formInputValues { set; get; }

        public AliasCreation aliasCreation { set; get; }

        public System.Object formParameters { set; get; }

        public System.String linkChangePaymentMethod { set; get; }

        public System.String brandPlaceholderImage { set; get; }

        public IReadOnlyList<string> requiredScripts { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PaymentMethodSelectionData
    {
        public System.String actionPaymentMethodSelect { set; get; }

        public System.String noScriptStyleSheet { set; get; }

        public IReadOnlyList<PaymentMethodItem> paymentMethods { set; get; }

        public IReadOnlyList<AliasItem> aliases { set; get; }

        public IReadOnlyList<string> requiredScripts { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class AliasItem
    {
        public System.String alias { set; get; }

        public System.String brand { set; get; }

        public System.String displayValue { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class TemplateRenderRequest
    {
        public TemplateData templateData { set; get; }

        public AliasCreation aliasCreation { set; get; }

        public SplitPaymentSummary splitPaymentSummary { set; get; }

        public System.String actionCancel { set; get; }

        public DisplayElements displayElements { set; get; }

        public DisplayOptions displayOptions { set; get; }

        public System.Object formParameters { set; get; }

        public Dcc dcc { set; get; }

        public Surcharge surcharge { set; get; }

        public IReadOnlyList<LineItem> lineItems { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Dcc
    {
        public DccData dccData { set; get; }

        public IReadOnlyList<DccCurrencyTypes> dccCurrencyTypes { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class DccCurrencyTypes
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class DccData
    {
        public System.String cardType { set; get; }

        public System.String cardholderName { set; get; }

        public System.String transactionTotalBaseAmount { set; get; }

        public System.String transactionCurrency { set; get; }

        public System.String transactionTotalTargetAmount { set; get; }

        public System.String exchangeRate { set; get; }

        public System.String baseCurrencyCode { set; get; }

        public System.String targetCurrencyCode { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Surcharge
    {
        public System.String amount { set; get; }

        public System.String totalAmount { set; get; }

        public System.String surchargeMode { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class SurchargeMode
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class TemplateData
    {
        public System.String pspId { set; get; }

        public System.String requestedTemplate { set; get; }

        public System.String templateRootId { set; get; }

        public ResourcesData resourcesData { set; get; }

        public LinkData links { set; get; }

        public System.String paymentZone { set; get; }

        public LanguageIdOrCode language { set; get; }

        public System.Boolean useLanguageForDefaultTemplate { set; get; }

        public System.String defaultTemplateContent { set; get; }

        public System.String host { set; get; }

        public System.String defaultBranding { set; get; }

        public System.String templateStore { set; get; }

        public System.Boolean isProd { set; get; }

        public System.String mode { set; get; }

        public System.Boolean isUtf8 { set; get; }

        public System.String inlineCss { set; get; }

        public System.String inlineJs { set; get; }

        public System.String inlineLogo { set; get; }

        public LegacyCustomizationParameters legacyCustomizationParameters { set; get; }

        public ParserConfiguration parserConfiguration { set; get; }

        public System.Boolean disableJavascriptCheck { set; get; }

        public IReadOnlyList<string> partialTemplateKeys { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class ResourcesData
    {
        public System.String resourcesUri { set; get; }

        public System.Object sris { set; get; }

        public System.String templateResourcesUri { set; get; }

        public System.String brandImagesBaseUri { set; get; }

        public System.String brandImagesExtension { set; get; }

        public System.String brandingImagesBaseUri { set; get; }

        public System.String brandingImagesExtension { set; get; }

        public System.String merchantTemplateResourcesUri { set; get; }

        public System.String legacyResourcesUri { set; get; }

        public IReadOnlyList<string> validationScripts { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class LinkData
    {
        public System.String linksBaseUri { set; get; }

        public System.String actionPrivacyPolicy { set; get; }

        public System.String actionAbout { set; get; }

        public System.String actionSecurity { set; get; }

        public System.String actionLegalInfo { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class LanguageIdOrCode
    {
        public System.String languageCode { set; get; }

        public System.Int32 languageId { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class LegacyCustomizationParameters
    {
        public System.String title { set; get; }

        public System.String logo { set; get; }

        public System.String bgColor { set; get; }

        public System.String txtColor { set; get; }

        public System.String tblBgColor { set; get; }

        public System.String tblTxtColor { set; get; }

        public System.String buttonBgColor { set; get; }

        public System.String buttonTxtColor { set; get; }

        public System.String fontType { set; get; }

        public System.String hdTblBgColor { set; get; }

        public System.String hdTblTxtColor { set; get; }

        public System.String hdFontType { set; get; }

        public System.String rtlCss { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class ParserConfiguration
    {
        public System.Boolean generateErrorMessages { set; get; }

        public System.Boolean generateDivAroundElements { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class LineItem
    {
        public System.String description { set; get; }

        public System.String productName { set; get; }

        public System.String productPrice { set; get; }

        public System.String totalAmount { set; get; }

        public System.String quantity { set; get; }

        public System.String unit { set; get; }

        public System.String taxAmount { set; get; }

        public System.String productType { set; get; }

        public System.String productCode { set; get; }

        public System.String discountAmount { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class AliasCreation
    {
        public System.String aliasUsage { set; get; }

        public System.Boolean isChecked { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class SplitPaymentSummary
    {
        public System.String remainingAmount { set; get; }

        public IReadOnlyList<SplitPayment> splitPayments { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class SplitPayment
    {
        public System.String brandName { set; get; }

        public System.String totalAmount { set; get; }

        public System.String maskedCardNumber { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class SplitPaymentStatusData
    {
        public System.String brandName { set; get; }

        public System.String actionPaymentMethodSelect { set; get; }

        public IReadOnlyList<PaymentMethodItem> paymentMethods { set; get; }

        public IReadOnlyList<string> requiredScripts { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PaymentMethodItem
    {
        public System.String brand { set; get; }

        public System.String brandDisplayNameOverride { set; get; }

        public System.String acquirer { set; get; }

        public IReadOnlyList<string> brandVisuals { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class DisplayElements
    {
        public System.String totalAmount { set; get; }

        public System.String currency { set; get; }

        public System.String orderId { set; get; }

        public System.String paymentId { set; get; }

        public System.String vatIdentificationNumber { set; get; }

        public System.String flowStatus { set; get; }

        public IReadOnlyList<string> beneficiaryLines { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class FlowStatus
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class DisplayOptions
    {
        public System.Boolean isForMobile { set; get; }

        public System.Boolean allowTemplateWithoutPrivacyPolicy { set; get; }

        public System.Boolean renderPaymentMethodsHidden { set; get; }

        public System.String paymentMethodsHiddenClassName { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PrivacyText
    {
        public System.String title { set; get; }

        public System.String content { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Template
    {
        public System.String templateId { set; get; }

        public System.String content { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PutTemplateRequest
    {
        public System.String content { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Metadata
    {
        public System.Int32 metadataId { set; get; }

        public System.String uniqueId { set; get; }

        public System.String fileName { set; get; }

        public System.String fileType { set; get; }

        public System.String merchantId { set; get; }

        public System.String status { set; get; }

        public System.String statusDate { set; get; }

        public System.String templateId { set; get; }

        public System.String templateRootId { set; get; }

        public System.String cdnStatus { set; get; }

        public System.String cdnSri { set; get; }

        public System.String cdnVersionTag { set; get; }

        public System.Int32 techOption { set; get; }

        public System.String assetPath { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class GetAssetsMetadataApiResponse
    {
        public IReadOnlyList<Metadata> templateMetadataList { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class PutMetadataRequest
    {
        public System.String cdnStatus { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class QueryMetadataResponse
    {
        public IReadOnlyList<Metadata> metadataList { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class UploadBatchesResponse
    {
        public System.String templateRootId { set; get; }

        public IReadOnlyList<Metadata> metadataList { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class UploadBatchesCompletedRequest
    {
        public System.String templateRootId { set; get; }

        public System.String cdnVersionTag { set; get; }

        public IReadOnlyList<CompletedMetadata> metadataIdList { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class CompletedMetadata
    {
        public System.Int32 metadataId { set; get; }

        public System.String cdnSri { set; get; }
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class FileType
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class Status
    {
    }

    [System.CodeDom.Compiler.GeneratedCodeAttribute("Xena.HttpClient.Generator", "1.0.0")]
    public class TemplateRenderResponse
    {
        public System.String html { set; get; }

        public System.String nonce { set; get; }
    }
}