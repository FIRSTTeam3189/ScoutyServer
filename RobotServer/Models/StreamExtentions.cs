using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ScoutingServer.Models
{
    public static class StreamExtensions
    {
        /// <summary>
        /// Loads a JArray from a stream object
        /// </summary>
        /// <param name="stream">Stream to load from</param>
        /// <returns>The JArray</returns>
        public static JArray JArrayFromStream(this Stream stream)
        {
            // Assert that the stream is not null
            stream.IsNotNull().IsReadableStream();

            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            using (var reader = new JsonTextReader(streamReader))
                return JArray.Load(reader);

        }

        /// <summary>
        /// Loads a JArray from a stream object
        /// </summary>
        /// <param name="stream">Stream to load from</param>
        /// <param name="encoding">Encoding to use</param>
        /// <returns>The JArray</returns>
        public static JArray JArrayFromStream(this Stream stream, Encoding encoding)
        {
            // Assert that the stream is not null and readable
            stream.IsNotNull().IsReadableStream();

            using (var streamReader = new StreamReader(stream, encoding))
            using (var reader = new JsonTextReader(streamReader))
                return JArray.Load(reader);
        }

        public static JToken JTokenFromStream(this Stream stream, Encoding encoding)
        {
            stream.IsNotNull().IsReadableStream();

            using (var streamReader = new StreamReader(stream, encoding))
            using (var reader = new JsonTextReader(streamReader))
                return JToken.Load(reader);
        }

        public static JToken JTokenFromStream(this Stream stream)
        {
            stream.IsNotNull().IsReadableStream();

            using (var streamReader = new StreamReader(stream, Encoding.UTF8))
            using (var reader = new JsonTextReader(streamReader))
                return JToken.Load(reader);
        }

        /// <summary>
        /// Assertion test to check if an object is not null
        /// </summary>
        /// <param name="obj">Object to test</param>
        /// <returns>Object its self</returns>
        public static object IsNotNull(this object obj)
        {
            Contract.Requires(obj != null);
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Contract.EndContractBlock();

            return obj;
        }

        /// <summary>
        /// Assertion test to check if an object of type T is not null
        /// </summary>
        /// <typeparam name="T">Type of object</typeparam>
        /// <param name="obj">Object to test</param>
        /// <returns>The object its self</returns>
        public static T IsNotNull<T>(this T obj) where T : class
        {
            Contract.Requires(obj != null);
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));
            Contract.EndContractBlock();

            return obj;
        }

        /// <summary>
        /// Ensures that the stream is readable
        /// </summary>
        /// <typeparam name="T">Type of stream</typeparam>
        /// <param name="obj">Stream to test</param>
        /// <returns>Stream its self</returns>
        public static T IsReadableStream<T>(this T obj) where T : Stream
        {
            Contract.Requires(obj.CanRead);
            if (!obj.CanRead)
                throw new ArgumentException("Cannot read from stream", nameof(obj));
            Contract.EndContractBlock();

            return obj;
        }

        /// <summary>
        /// Ensures that the stream is writable
        /// </summary>
        /// <typeparam name="T">Type of stream</typeparam>
        /// <param name="obj">Stream to test</param>
        /// <returns>Stream its self</returns>
        public static T IsWritableStream<T>(this T obj) where T : Stream
        {
            Contract.Requires(obj.CanWrite);
            if (!obj.CanWrite)
                throw new ArgumentException("Cannot write to stream", nameof(obj));
            Contract.EndContractBlock();

            return obj;
        }
    }
}
