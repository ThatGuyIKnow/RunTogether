using System;
using System.Collections.Generic;
using System.Text;
using Bunit;
using RunTogether.Areas.Identity.Data;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace RunTogetherTests
{
    public class OrganiserKeyTests : TestContext
    {
        [Fact]
        public void OrganiserCreationKey_CreationRandomKey_KeyExists()
        {
            OrganiserCreationKey key;

            key = new OrganiserCreationKey("TestOrganiserId");
            
            Assert.NotEmpty(key.Key);
        }

        [Fact]
        public void OrganiserCreationKey_CreationExpirationDatetime_OneDayDifference()
        {
            DateTime expectedExpirationLow = DateTime.Now.AddDays(1).AddSeconds(-10);
            DateTime expectedExpirationHigh = DateTime.Now.AddDays(1).AddSeconds(10);
            OrganiserCreationKey key;

            key = new OrganiserCreationKey("TestOrganiserId");

            Assert.InRange(key.ExpirationDatetime, expectedExpirationLow, expectedExpirationHigh);
        }
        [Fact]
        public void OrganiserCreationKey_CreationKey_NotIdentical()
        {
            OrganiserCreationKey key1;
            OrganiserCreationKey key2;

            key1 = new OrganiserCreationKey("TestOrganiserId1");
            key2 = new OrganiserCreationKey("TestOrganiserId2");

            Assert.NotEqual(key1, key2);
        }
    }
}
