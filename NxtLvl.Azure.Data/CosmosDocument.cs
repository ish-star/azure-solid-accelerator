using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using System;

namespace NxtLvl.Azure.Data
{
    public class CosmosDocument
    {
        public Guid? Id { get; set; }
        
        public string SystemType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
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
