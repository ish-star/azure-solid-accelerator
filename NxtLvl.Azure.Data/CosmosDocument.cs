using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace NxtLvl.Azure.Data
{
    public class CosmosDocument
    {
        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        [JsonProperty("id")]
        public Guid? Id { get; set; }
        
        [JsonProperty("systemType")]
        public string SystemType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
                Formatting = Formatting.None
            });
        }

        public virtual void Hydrate(Document document)
        {
            Id = new Guid(document.Id);
            SystemType = document.GetPropertyValue<string>("systemType");
        }

        public CosmosDocument()
        { }
    }
}
