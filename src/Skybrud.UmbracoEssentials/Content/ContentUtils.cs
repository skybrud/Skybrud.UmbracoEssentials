using System;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Content {
    
    public static class ContentUtils {

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the content cache based on the ID specified by
        /// <paramref name="str"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the content item.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/> if found, otherwise <code>NULL</code>.</returns>
        public static IPublishedContent TypedContent(string str) {

            // Get the first ID in a comma separated string
            int contentId = str.CsvToInt().FirstOrDefault();

            // Parse the value and attempt to find the content node in the cache
            return contentId > 0 && UmbracoContext.Current != null ? UmbracoContext.Current.ContentCache.GetById(contentId) : null;

        }

        /// <summary>
        /// Returns an instance of <typeparamref name="T"/> from the content cache based on the ID specified by
        /// <paramref name="str"/>.
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the content item.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>An instance of <typeparamref name="T"/> if found, otherwise <code>NULL</code>.</returns>
        public static T TypedContent<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException("func");

            // Find the content using the method overload
            IPublishedContent content = TypedContent(str);

            // Convert the content (or just return null if not found)
            return content == null ? default(T) : func(content);

        }

        /// <summary>
        /// Converts the comma seperated IDs as specified by <paramref name="str"/> into an
        /// array of <see cref="IPublishedContent"/> by using the content cache.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the comma separated IDs of the content items.</param>
        /// <returns>An array of <see cref="IPublishedContent"/>.</returns>
        public static IPublishedContent[] TypedCsvContent(string str) {

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new IPublishedContent[0];

            // Look up each ID in the content cache and return the collection as an array
            return (
                from id in str.CsvToInt()
                let item = UmbracoContext.Current.ContentCache.GetById(id)
                where item != null
                select item
            ).ToArray();

        }

        /// <summary>
        /// Converts the comma seperated IDs as specified by <paramref name="str"/> into an array of
        /// <typeparamref name="T"/> by using the content cache.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the comma separated IDs of the content items.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvContent<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException("func");

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new T[0];

            // Look up each ID in the content cache and return the collection as an array
            return (
                from id in str.CsvToInt()
                let item = UmbracoContext.Current.ContentCache.GetById(id)
                where item != null
                select func(item)
            ).ToArray();

        }

    }

}