using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Sidewinder.Core
{
    public class SerialisationHelper<T>
    {
        /// <summary>
        /// Serialize the object using the DataContractSerializer
        /// </summary>
        /// <param name="encoding">The encoding to use when serializing.</param>
        /// <param name="entity">The entity to serialize.</param>
        /// <returns></returns>
        public static string DataContractSerialize(Encoding encoding, T entity)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var ms = new MemoryStream())
            {
                serializer.WriteObject(ms, entity);
                return encoding.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// Serialize the object using the DataContractSerializer to the file specified. If the file
        /// exists it will be overwritten and it requires an exclusive write lock on the file.
        /// </summary>
        /// <param name="path">The filename to write the serialization string to</param>
        /// <param name="encoding">The encoding to use when serializing.</param>
        /// <param name="entity">The entity to serialize.</param>
        /// <returns></returns>
        public static void DataContractSerialize(string path, Encoding encoding, T entity)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var filename = SmartLocation.GetLocation(path);

            using (var writer = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                serializer.WriteObject(writer, entity);
            }
        }

        /// <summary>
        /// Serialize the object using the DataContractSerializer, defaults to using UTF8 encoding
        /// </summary>
        /// <param name="entity">The entity to serialize.</param>
        /// <returns></returns>
        public static string DataContractSerialize(T entity)
        {
            return DataContractSerialize(Encoding.UTF8, entity);
        }

        /// <summary>
        /// Serialize the object using the DataContractSerializer to the file specified with UTF8
        /// encoding. If the file exists it will be overwritten and it requires an exclusive write
        /// lock on the file.
        /// </summary>
        /// <param name="path">The filename to write the serialization string to</param>
        /// <param name="entity">The entity to serialize.</param>
        /// <returns></returns>
        public static void DataContractSerialize(string path, T entity)
        {
            DataContractSerialize(path, Encoding.UTF8, entity);
        }

        /// <summary>
        /// Deserialize the object using the DataContractSerializer
        /// </summary>
        /// <param name="encoding">The encoding to use when deserializing.</param>
        /// <param name="entity">The entity to deserialize.</param>
        /// <returns></returns>
        public static T DataContractDeserialize(Encoding encoding, string entity)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var ms = new MemoryStream(encoding.GetBytes(entity)))
            {
                return (T)serializer.ReadObject(ms);
            }
        }

        /// <summary>
        /// Deserialize the object using the DataContractSerializer from the file specified. If the file
        /// does not exist then an exception will be thrown
        /// </summary>
        /// <param name="path">The fully qualified path to the file</param>
        /// <returns></returns>
        public static T DataContractDeserializeFromFile(string path)
        {
            return DataContractDeserializeFromFile(path, Encoding.UTF8);
        }

        /// <summary>
        /// Deserialize the object using the DataContractSerializer from the file specified. If the file
        /// does not exist then an exception will be thrown
        /// </summary>
        /// <param name="path">The fully qualified path to the file</param>
        /// <param name="encoding">The encoding to use when deserializing.</param>
        /// <returns></returns>
        public static T DataContractDeserializeFromFile(string path, Encoding encoding)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var filename = SmartLocation.GetLocation(path);

            using (var reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return (T)serializer.ReadObject(reader);
            }
        }

        /// <summary>
        /// Deserialize the object using the DataContractSerializer, defaults to using UTF8 encoding
        /// </summary>
        /// <param name="entity">The entity to deserialize.</param>
        /// <returns></returns>
        public static T DataContractDeserialize(string entity)
        {
            return DataContractDeserialize(Encoding.UTF8, entity);
        }
    }
}