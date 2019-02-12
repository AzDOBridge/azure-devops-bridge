using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;
using Alexa.NET.Request;
using AzDoBridgeUnitTests.Utilities;
using Alexa.NET.Request.Type;
using AzDoBridge.Models;

namespace AzDoBridgeUnitTests.ModelsTests
{
    public class ModelsTests
    {
        const string testFilePath = "TestInputs";
        const string testFile = "IntentRequest.json";
        
        [Theory]
        [InlineData("itemid")]
        [InlineData("itemstate")]
        [InlineData("Fullname")]
        public void Can_Get_SlotValue_For_Intent(string slotName)
        {
            SkillRequest skillRequest = TestUtilities.EntityFromFile<SkillRequest>(testFilePath, testFile);
            var intentRequest = (IntentRequest)skillRequest.Request;
            var ItemID = IntentRequestExtensions.GetSlotValue(intentRequest, slotName);
            var success = intentRequest.Intent.Slots.TryGetValue(slotName, out Slot slot);
            ItemID.Should().Equals(slot.Value);
        }
        [Theory]
        [InlineData(RequestType.IntentRequest)]
        public void Can_Determine_Request_Type(string requestType)
        {
            //IntentRequest File
            SkillRequest skillRequest = TestUtilities.EntityFromFile<SkillRequest>(testFilePath, testFile);
            var intentRequest = (IntentRequest)skillRequest.Request;
            var input = intentRequest.Type;
            input.Should().Equals(requestType);
        }
        [Theory]
        [InlineData("itemid")]
        [InlineData("itemstate")]
        [InlineData("Fullname")]
        public void NotFound_SlotValue_For_Intent(string slotValue)
        {
            SkillRequest skillRequest = TestUtilities.EntityFromFile<SkillRequest>(testFilePath, testFile);
            var intentRequest = (IntentRequest)skillRequest.Request;
            intentRequest.Intent.Slots[slotValue].Value = null;
            var success = IntentRequestExtensions.GetSlotValue(intentRequest, slotValue);
            success.Should().Equals(null);
        }
    }
}
