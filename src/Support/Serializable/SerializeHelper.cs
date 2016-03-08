using System.IO;
using System.Xml.Serialization;

namespace Serializable
{
    public static class SerializeHelper
    {
        /// <summary>
        /// Serialize a serializable object to an XML String.
        /// </summary>
        /// <typeparam name="T">The object type being serialized.</typeparam>
        /// <param name="toSerialize">The object.</param>
        /// <returns></returns>
        public static string SerializeObject<T>(this T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());
            StringWriter textWriter = new StringWriter();

            xmlSerializer.Serialize(textWriter, toSerialize);
            return textWriter.ToString();
        }

        /// <summary>
        /// Deserialize a string to an object.
        /// </summary>
        /// <typeparam name="T">The type of the object being deserialized.</typeparam>
        /// <param name="toDeserialize">The string representation of the xml document.</param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string toDeserialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(toDeserialize);
            return (T)xmlSerializer.Deserialize(textReader);
        }
    }
}
