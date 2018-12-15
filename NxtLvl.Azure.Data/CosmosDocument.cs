using Microsoft.Azure.Documents;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;

namespace NxtLvl.Azure.Data
{
    /// <summary>
    /// Base class that can be used when defining Entities to use with the ComsosEntityStore class.
    /// </summary>
    public class CosmosDocument
    {
        #region fields

        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        #endregion

        #region properties

        /// <summary>
        /// The unique identifier of the Entity record.
        /// </summary>
        [JsonProperty("id")]
        public Guid? Id { get; set; }
        
        /// <summary>
        /// The fully qualified name of the type of the Entity record.
        /// </summary>
        /// <remarks>
        /// Can be used to know which class to serialize the record into in your application.
        /// </remarks>
        [JsonProperty("systemType")]
        public string SystemType { get; set; }

        #endregion

        #region methods

        /// <summary>
        /// Provides the JSON serialization representation of the Entity record.
        /// </summary>
        /// <returns>JSON serialization representation of the Entity record.</returns>
        public override string ToString() => JsonConvert.SerializeObject(this, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.None
        });

        /// <summary>
        /// Converts the CosmosDB Document object to the strongly typed class representation.
        /// </summary>
        /// <param name="document">The CosmosDB Document that contains the data for the Entity record.</param>
        public virtual void Hydrate(Document document)
        {
            Id = new Guid(document.Id);
            SystemType = document.GetPropertyValue<string>("systemType");
        }

        #endregion

        #region constructor

        /// <summary>
        /// Base class that can be used when defining Entities to use with the ComsosEntityStore class.
        /// </summary>
        public CosmosDocument()
        { }

        #endregion
    }
}
