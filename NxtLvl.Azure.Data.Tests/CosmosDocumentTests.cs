using Microsoft.Azure.Documents;
using System;
using Xunit;

namespace NxtLvl.Azure.Data.Tests
{
    public class CosmosDocumentTests
    {
        [Fact]
        public void CosmosDocument_DefaultConstructor()
        {
            var cosmosDocument = new CosmosDocument();

            Assert.Null(cosmosDocument.Id);
            Assert.Null(cosmosDocument.SystemType);
            Assert.Equal("{\"id\":null,\"systemType\":null}", cosmosDocument.ToString());
        }

        [Fact]
        public void CosmosDocument_HydrateMethod()
        {
            var expectedId = Guid.NewGuid().ToString();
            var expectedSystemType = "Test.Type";

            Document document = new Document()
            {
                Id = expectedId
            };

            document.SetPropertyValue("systemType", expectedSystemType);

            var cosmosDocument = new CosmosDocument();
            cosmosDocument.Hydrate(document);

            Assert.Equal(expectedId, cosmosDocument.Id.Value.ToString());
            Assert.Equal(expectedSystemType, cosmosDocument.SystemType);
            Assert.Equal("{\"id\":\"" + expectedId + "\",\"systemType\":\"" + expectedSystemType + "\"}", cosmosDocument.ToString());
        }
    }
}
