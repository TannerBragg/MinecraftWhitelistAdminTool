using System.Collections.Generic;
using System.Xml.Serialization;

namespace Serializable
{
    [XmlRoot("SerializableClass")]
    public class SerializableClass
    {
        /// <summary>
        /// The age of the object.
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// The time of the object.
        /// </summary>
        public double Time { get; set; }

        /// <summary>
        /// The name of the object.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The names of the children of the object.
        /// </summary>
        public List<string> Children { get; set; }
    }
}
