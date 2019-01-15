using Microsoft.Azure.Documents;
using Newtonsoft.Json;

namespace NxtLvl.Azure.Data.IntegrationTests.TestObjects
{
    public class TestCosmosEntity : CosmosDocument
    {
        const string StringFieldName = "stringField";

        [JsonProperty(StringFieldName)]
        public string StringField { get; set; }

        public override void Hydrate(Document document)
        {
            base.Hydrate(document);

            StringField = document.GetPropertyValue<string>(StringFieldName);
        }
    }
}
