using System.Collections.Generic;
using NUnit.Framework;

namespace Serializable.Unit_Test
{
    [TestFixture]
    class SerializeUnitTest
    {
        [Test]
        private void Serialize()
        {
            SerializableClass instance = new SerializableClass();
            instance.Age = 30;
            instance.Children = new List<string> { "Tim", "Jim", "Henry" };
            instance.Name = "Bob";
            instance.Time = 1300.356;

            string xmlDoc = SerializeHelper.SerializeObject(instance);

            SerializableClass newInstance = new SerializableClass();
            Assert.Equals(instance, newInstance);
        }
    }
}
