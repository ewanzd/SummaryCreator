using System;
using System.IO;
using System.Xml.Serialization;

namespace SummaryCreator.Source
{
    /// <summary>
    /// XML im- and export.
    /// </summary>
    internal static class XMLPort
    {
        /// <summary>
        /// The variable help for access to file.
        /// </summary>
        private static object waitForAccess = new object();

        /// <summary>
        /// Save a object in XML.
        /// </summary>
        /// <typeparam name="T">The type of object, that should save.</typeparam>
        /// <param name="toSave">The object, that should save.</param>
        /// <param name="path">The path and name of file, in that will be save the object.</param>
        public static void Save<T>(T toSave, string path) where T : class
        {
            lock(toSave)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                using(FileStream file = new FileStream(path, FileMode.Create))
                    serializer.Serialize(file, toSave);
            }
        }

        /// <summary>
        /// Load a object from XML.
        /// </summary>
        /// <typeparam name="T">The type of object, that should get from XML file.</typeparam>
        /// <param name="path">The path from XML file.</param>
        /// <returns>Return the full object.</returns>
        public static T Load<T>(string path) where T : class
        {
            if(!File.Exists(path))
                throw new FileNotFoundException(path);

            T data = default(T);
            lock(waitForAccess)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using(FileStream file = new FileStream(path, FileMode.Open))
                    data = serializer.Deserialize(file) as T;
            }
            return data;
        }
    }
}
