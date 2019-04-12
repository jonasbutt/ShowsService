using System;
using Newtonsoft.Json;
using NUnit.Framework;
using ShowsService.Tools.Serialization;

namespace ShowsService.Tools.Tests.Serialization
{
    [TestFixture]
    public class CustomFormatDateTimeConverterTests
    {
        [Test]
        public void CustomFormatDateTimeConverter_SerializesUsingTheGivenFormat()
        {
            // Arrange
            var objectToSerialize = new TestClass
                                    {
                                        DateTime = new DateTime(2018, 4, 3, 17, 10, 5)
                                    };

            // Act
            var json = JsonConvert.SerializeObject(objectToSerialize);

            // Assert
            Assert.That(json, Contains.Substring("\"2018-04-03\""));
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class TestClass
        {
            // ReSharper disable once UnusedAutoPropertyAccessor.Local
            [JsonConverter(typeof(CustomFormatDateTimeConverter), "yyyy-MM-dd")]
            public DateTime DateTime { get; set; }
        }
    }
}