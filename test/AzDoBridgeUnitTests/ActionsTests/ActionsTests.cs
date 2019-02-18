using AzDoBridge.Actions;
using AzDoBridge.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;
using FluentAssertions;
using Alexa.NET.Response;
using Alexa.NET.Request;

namespace AzDoBridge.UnitTests
{
    public class ActionsTests
    {
        Mock<ILogger> logger;
        public ActionsTests()
        {
            logger = new Mock<ILogger>();
        }
        [Theory]
        [InlineData(AzDoBridgeIntent.AssignTo)]
        [InlineData(AzDoBridgeIntent.ChangeWiStatus)]
        [InlineData(AzDoBridgeIntent.SetPriority)]
        public void Should_Factor_Multiple_Actions(AzDoBridgeIntent azDoBridgeIntent)
        {
            IAzDoBridgeAction action;
            var fac = AzDoBridgeActionFactory.TryGetAction(azDoBridgeIntent.ToString(), logger.Object, out action);
            fac.Should().BeTrue();
            action.GetType().Should().Equals(typeof(AzDoBridgeIntent));            
        }
    }
}
