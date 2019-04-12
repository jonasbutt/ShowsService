using Newtonsoft.Json;
using NUnit.Framework;
using ShowsService.Tools.Serialization;

namespace ShowsService.Tools.Tests.Serialization
{
    [TestFixture]
    public class JsonSerializerTests
    {
        private IJsonSerializer jsonSerializer;

        [SetUp]
        public void SetUp()
        {
            this.jsonSerializer =  new Tools.Serialization.JsonSerializer();
        }

        [Test]
        public void Serialize_ReturnsCorrectJson()
        {
            // Arrange
            var objectToSerialize = new TestClass
                                    {
                                        PropertyOne = "Value One",
                                        PropertyTwo = default,
                                        PropertyThree = default,
                                        PropertyFour = default
                                    };

            // Act
            var json = this.jsonSerializer.Serialize(objectToSerialize);

            // Assert
            Assert.That(json, Is.Not.Null);
            Assert.That(json, Is.EqualTo("{\r\n  \"propertyOne\": \"Value One\"\r\n}"));
        }

        [Test]
        public void Deserialize_ReturnsCorrectJson()
        {
            // Arrange
            const string jsonToDeserialize = "{\r\n  \"propertyOne\": \"Value One\"\r\n}";

            // Act
            var @object = this.jsonSerializer.Deserialize<TestClass>(jsonToDeserialize);

            // Assert
            Assert.That(@object, Is.Not.Null);
            Assert.That(@object.PropertyOne, Is.EqualTo("Value One"));
            Assert.That(@object.PropertyTwo, Is.Null);
        }

        [Test]
        public void SerializeToDictionary_ReturnsCorrectDictionary()
        {
            // Arrange
            var objectToSerialize = new TestClass
                                    {
                                        PropertyOne = "Value One",
                                        PropertyTwo = default,
                                        PropertyThree = "Some value which should be ignored",
                                        PropertyFour = "Value Four"
                                    };

            // Act
            var dictionary = this.jsonSerializer.SerializeToDictionary(objectToSerialize);

            // Assert
            Assert.That(dictionary, Is.Not.Null);
            Assert.That(dictionary, Does.ContainKey("propertyOne"));
            Assert.That(dictionary, Does.Not.ContainKey("propertyTwo"));
            Assert.That(dictionary, Does.Not.ContainKey("propertyThree"));
            Assert.That(dictionary, Does.Not.ContainKey("propertyFour"));
            Assert.That(dictionary, Does.ContainKey("propertyFourWithAlternativeName"));
            Assert.That(dictionary["propertyOne"], Is.EqualTo("Value One"));
            Assert.That(dictionary["propertyFourWithAlternativeName"], Is.EqualTo("Value Four"));
        }

        private class TestClass
        {
            public string PropertyOne { get; set; }

            public string PropertyTwo { get; set; }

            [JsonIgnore]
            public string PropertyThree { get; set; }

            [JsonProperty("propertyFourWithAlternativeName")]
            public string PropertyFour { get; set; }
        }
    }
}
