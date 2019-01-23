using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace BasicLibrary
{
    /// <summary>
    /// Binary im- and export.
    /// </summary>
    public static class BinaryPort
    {
        /// <summary>
        /// Save a object in Binary.
        /// </summary>
        /// <typeparam name="T">The type of object, that should save.</typeparam>
        /// <param name="toSave">The object, that should save.</param>
        /// <param name="path">The path and name of file, in that will be save the object.</param>
        public static Task Save<T>(T toSave, string path) where T : class
        {
            return Task.Run(() =>
            {
                BinaryFormatter serializer = new BinaryFormatter();
                Directory.CreateDirectory(Path.GetDirectoryName(path));

                lock(toSave)
                    using(FileStream stream = new FileStream(path, FileMode.Create))
                        serializer.Serialize(stream, toSave);
            });
        }

        /// <summary>
        /// Load a object from Binary.
        /// </summary>
        /// <typeparam name="T">The type of object, that should get from Binary file.</typeparam>
        /// <param name="path">The path from Binary file.</param>
        /// <returns>Return the full object.</returns>
        public static Task<T> Load<T>(string path) where T : class
        {
            if(!File.Exists(path))
                throw new FileNotFoundException(path);

            T data = default(T);
            BinaryFormatter serializer = new BinaryFormatter();

            using(FileStream stream = new FileStream(path, FileMode.Open))
                data = serializer.Deserialize(stream) as T;

            return Task.FromResult(data);
        }
    }
}
