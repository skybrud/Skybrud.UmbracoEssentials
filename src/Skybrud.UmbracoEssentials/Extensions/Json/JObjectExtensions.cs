using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Extensions.Json {
    
    /// <summary>
    /// Various extensions methods for <see cref="JObject"/>.
    /// </summary>
    public static class JObjectExtensions {

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified in
        /// the property matching the specified <paramref name="path"/>.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="String"/> that contains a JPath expression.</param>
        /// <returns>Instance of <see cref="IPublishedContent"/> if found, otherwise <code>NULL</code>.</returns>
        public static IPublishedContent TypedMedia(this JObject obj, string path) {

            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");

            // Get the first ID in a comma separated string
            int mediaId = obj.GetString(path).CsvToInt().FirstOrDefault();

            // Parse the value and attempt to find the media node in the cache
            return mediaId > 0 && UmbracoContext.Current != null ? UmbracoContext.Current.MediaCache.GetById(mediaId) : null;

        }

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified in
        /// the property matching the specified <paramref name="path"/>. If found, the
        /// <see cref="IPublishedContent"/> is converted to the type of <typeparamref name="T"/> using the specified
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="String"/> that contains a JPath expression.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Instance of <typeparamref name="T"/> if found, otherwise <code>NULL</code>.</returns>
        public static T TypedMedia<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {

            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            if (func == null) throw new ArgumentNullException("func");

            // Find the media using the method overload
            IPublishedContent media = TypedMedia(obj, path);

            // Convert the media (or just return null if not found)
            return media == null ? default(T) : func(media);

        }

        /// <summary>
        /// Converts the comma seperated IDs of the property matching the specified <paramref name="path"/> into an
        /// array of <see cref="IPublishedContent"/> by using the media cache.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="String"/> that contains a JPath expression.</param>
        /// <returns>Array of <see cref="IPublishedContent"/>.</returns>
        public static IPublishedContent[] TypedCsvMedia(this JObject obj, string path) {

            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new IPublishedContent[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in obj.GetString(path).CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select item
            ).ToArray();

        }

        /// <summary>
        /// Converts the comma seperated IDs of the property matching the specified <paramref name="path"/> into an
        /// array of <see cref="IPublishedContent"/> by using the media cache. Each media is converted to the type of
        /// <typeparamref name="T"/> using the specified <paramref name="func"/>.
        /// </summary>
        /// <param name="obj">An instance of <see cref="JObject"/>.</param>
        /// <param name="path">A <see cref="String"/> that contains a JPath expression.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvMedia<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {

            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            if (func == null) throw new ArgumentNullException("func");

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new T[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in obj.GetString(path).CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select func(item)
            ).ToArray();

        }

    }

}