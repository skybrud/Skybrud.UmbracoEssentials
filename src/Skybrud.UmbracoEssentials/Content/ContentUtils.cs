using System;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Composing;

namespace Skybrud.UmbracoEssentials.Content {
    
    /// <summary>
    /// Static class with various utility methods for working with content.
    /// </summary>
    public static class ContentUtils {

        #region Public methods

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the content cache based on the ID specified by
        /// <paramref name="str"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the content item.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/> if found, otherwise <c>null</c>.</returns>
        public static IPublishedContent TypedContent(string str) {

            if (Current.UmbracoHelper == null) return null;
            if (string.IsNullOrWhiteSpace(str)) return null;
            
            // Iterate through each ID (there may be more than one depending on property type)
            foreach (string id in str.Split(',', ' ', '\r', '\n', '\t')) {
                IPublishedContent content = TypedDocumentById(id);
                if (content != null) return content;
            }

            return null;

        }

        /// <summary>
        /// Returns an instance of <typeparamref name="T"/> from the content cache based on the ID specified by
        /// <paramref name="str"/>.
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the content item.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>An instance of <typeparamref name="T"/> if found, otherwise <code>null</code>.</returns>
        public static T TypedContent<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException(nameof(func));

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
            if (Current.UmbracoHelper == null) return new IPublishedContent[0];
            
            // Also just return an empty array if the string is either NULL or empty
            if (string.IsNullOrWhiteSpace(str)) return new IPublishedContent[0];

            // Look up each ID in the content cache and return the collection as an array
            return (
                from id in str.Split(',', ' ', '\r', '\n', '\t')
                let item = TypedDocumentById(id)
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
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvContent<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException(nameof(func));

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (Current.UmbracoHelper == null) return new T[0];

            // Also just return an empty array if the string is either NULL or empty
            if (string.IsNullOrWhiteSpace(str)) return new T[0];

            // Look up each ID in the content cache and return the collection as an array
            return (
                from id in str.Split(',', ' ', '\r', '\n', '\t')
                let item = TypedDocumentById(id)
                where item != null
                select func(item)
            ).ToArray();

        }

        #endregion

        #region Private helper methods

        private static IPublishedContent TypedDocumentById(string id) {
            if (string.IsNullOrWhiteSpace(id)) return null;
            if (Guid.TryParse(id.Replace("umb://document/", ""), out Guid guid)) return Current.UmbracoHelper.Content(guid);
            if (int.TryParse(id, out int numeric)) return Current.UmbracoHelper.Content(numeric);
            return null;
        }

        #endregion

    }

}