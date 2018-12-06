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
    }
}
