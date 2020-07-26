using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace KovaiDotCo.Model
{
    /// <summary>
    /// The below model has been created from the JSON received via EventHub
    /// </summary>
    public class AzureDiagnosticRootModel
    {
        [JsonProperty("records")]
        public List<Record> Records { get; set; }
    }

    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Evidence
    {

        [JsonProperty("role")]
        public string Role { get; set; }

    }

    public class Authorization
    {

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("evidence")]
        public Evidence Evidence { get; set; }

    }

    public class Claims
    {

        [JsonProperty("aud")]
        public string Aud { get; set; }

        [JsonProperty("iss")]
        public string Iss { get; set; }

        [JsonProperty("iat")]
        public string Iat { get; set; }

        [JsonProperty("nbf")]
        public string Nbf { get; set; }

        [JsonProperty("exp")]
        public string Exp { get; set; }

        [JsonProperty("http://schemas.microsoft.com/claims/authnclassreference")]
        public string HttpSchemasMicrosoftComClaimsAuthnclassreference { get; set; }

        [JsonProperty("aio")]
        public string Aio { get; set; }

        [JsonProperty("altsecid")]
        public string Altsecid { get; set; }

        [JsonProperty("http://schemas.microsoft.com/claims/authnmethodsreferences")]
        public string HttpSchemasMicrosoftComClaimsAuthnmethodsreferences { get; set; }

        [JsonProperty("appid")]
        public string Appid { get; set; }

        [JsonProperty("appidacr")]
        public string Appidacr { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")]
        public string HttpSchemasXmlsoapOrgWs200505IdentityClaimsEmailaddress { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/surname")]
        public string HttpSchemasXmlsoapOrgWs200505IdentityClaimsSurname { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/givenname")]
        public string HttpSchemasXmlsoapOrgWs200505IdentityClaimsGivenname { get; set; }

        [JsonProperty("groups")]
        public string Groups { get; set; }

        [JsonProperty("http://schemas.microsoft.com/identity/claims/identityprovider")]
        public string HttpSchemasMicrosoftComIdentityClaimsIdentityprovider { get; set; }

        [JsonProperty("ipaddr")]
        public string Ipaddr { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("http://schemas.microsoft.com/identity/claims/objectidentifier")]
        public string HttpSchemasMicrosoftComIdentityClaimsObjectidentifier { get; set; }

        [JsonProperty("puid")]
        public string Puid { get; set; }

        [JsonProperty("rh")]
        public string Rh { get; set; }

        [JsonProperty("http://schemas.microsoft.com/identity/claims/scope")]
        public string HttpSchemasMicrosoftComIdentityClaimsScope { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")]
        public string HttpSchemasXmlsoapOrgWs200505IdentityClaimsNameidentifier { get; set; }

        [JsonProperty("http://schemas.microsoft.com/identity/claims/tenantid")]
        public string HttpSchemasMicrosoftComIdentityClaimsTenantid { get; set; }

        [JsonProperty("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")]
        public string HttpSchemasXmlsoapOrgWs200505IdentityClaimsName { get; set; }

        [JsonProperty("uti")]
        public string Uti { get; set; }

        [JsonProperty("ver")]
        public string Ver { get; set; }

        [JsonProperty("wids")]
        public string Wids { get; set; }

    }

    public class Identity
    {

        [JsonProperty("authorization")]
        public Authorization Authorization { get; set; }

        [JsonProperty("claims")]
        public Claims Claims { get; set; }

    }

    public class Properties
    {

        [JsonProperty("requestbody")]
        public string Requestbody { get; set; }

        [JsonProperty("eventCategory")]
        public string EventCategory { get; set; }

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("serviceRequestId")]
        public string ServiceRequestId { get; set; }

    }

    public class Record
    {

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("resourceId")]
        public string ResourceId { get; set; }

        [JsonProperty("operationName")]
        public string OperationName { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("resultType")]
        public string ResultType { get; set; }

        [JsonProperty("resultSignature")]
        public string ResultSignature { get; set; }

        [JsonProperty("durationMs")]
        public int DurationMs { get; set; }

        [JsonProperty("callerIpAddress")]
        public string CallerIpAddress { get; set; }

        [JsonProperty("correlationId")]
        public string CorrelationId { get; set; }

        [JsonProperty("identity")]
        public Identity Identity { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("properties")]
        public Properties Properties { get; set; }

    }

    public class Root
    {


    }


}
