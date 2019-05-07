using System;
using System.Linq;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Composing;

namespace Skybrud.UmbracoEssentials.Media {

    /// <summary>
    /// Static class with various utility methods for working with media.
    /// </summary>
    public static class MediaUtils {

        #region Public methods

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified by <paramref name="str"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the media item.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/> if found, otherwise <c>null</c>.</returns>
        public static IPublishedContent TypedMedia(string str) {

            if (Current.UmbracoHelper == null) return null;
            if (string.IsNullOrWhiteSpace(str)) return null;

            // Iterate through each ID (there may be more than one depending on property type)
            foreach (string id in str.Split(',', ' ', '\r', '\n', '\t')) {
                IPublishedContent item = TypedDocumentById(id);
                if (item != null) return item;
            }

            return null;

        }

        /// <summary>
        /// Returns an instance of <typeparamref name="T"/> from the media cache based on the ID specified by <paramref name="str"/>.
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the media item.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>An instance of <typeparamref name="T"/> if found, otherwise <c>null</c>.</returns>
        public static T TypedMedia<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException(nameof(func));

            // Find the content using the method overload
            IPublishedContent item = TypedMedia(str);

            // Convert the media item (or just return null if not found)
            return item == null ? default(T) : func(item);

        }
        
        /// <summary>
        /// Converts the comma seperated IDs as specified by <paramref name="str"/> into an
        /// array of <see cref="IPublishedContent"/> by using the media cache.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the comma separated IDs of the media items.</param>
        /// <returns>An array of <see cref="IPublishedContent"/>.</returns>
        public static IPublishedContent[] TypedCsvMedia(string str) {

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (Current.UmbracoHelper == null) return new IPublishedContent[0];
            
            // Also just return an empty array if the string is either NULL or empty
            if (String.IsNullOrWhiteSpace(str)) return new IPublishedContent[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in str.Split(',', ' ', '\r', '\n', '\t')
                let item = TypedDocumentById(id)
                where item != null
                select item
            ).ToArray();

        }

        /// <summary>
        /// Converts the comma seperated IDs as specified by <paramref name="str"/> into an array of
        /// <typeparamref name="T"/> by using the media cache.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the comma separated IDs of the media items.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>An array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvMedia<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException(nameof(func));

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (Current.UmbracoHelper == null) return new T[0];

            // Look up each ID in the media cache and return the collection as an array
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
            if (Guid.TryParse(id.Replace("umb://media/", string.Empty), out Guid guid)) return Current.UmbracoContext.MediaCache.GetById(guid);
            if (int.TryParse(id, out int numeric)) return Current.UmbracoContext.MediaCache.GetById(numeric);
            return null;
        }

        #endregion

    }

}