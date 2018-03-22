using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Xml;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Media {

    /// <summary>
    /// Static class with various utility methods for working with media.
    /// </summary>
    public static class MediaUtils {

        #region Fields

        /// <summary>
        /// Gets a reference to the internal lookup table of GUIDs.
        /// </summary>
        public static readonly Dictionary<Guid, int> GuidLookupTable = new Dictionary<Guid, int>();

        #endregion

        #region Public methods

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified by <paramref name="str"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the media item.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/> if found, otherwise <code>null</code>.</returns>
        public static IPublishedContent TypedMedia(string str) {


            if (UmbracoContext.Current == null) return null;
            if (String.IsNullOrWhiteSpace(str)) return null;

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
        /// <returns>An instance of <typeparamref name="T"/> if found, otherwise <code>null</code>.</returns>
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
            if (UmbracoContext.Current == null) return new IPublishedContent[0];
            
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
            if (UmbracoContext.Current == null) return new T[0];

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
            if (String.IsNullOrWhiteSpace(id)) return null;
            if (Guid.TryParse(id.Replace("umb://media/", ""), out Guid guid)) return TypedDocumentById(guid);
            if (Int32.TryParse(id, out Int32 numeric)) return UmbracoContext.Current.MediaCache.GetById(numeric);
            return null;
        }

        /// <see>
        ///     <cref>https://github.com/umbraco/Umbraco-CMS/blob/7abc6d3d4feea88e824a6fbc3694b92dc37c6ceb/src/Umbraco.Web/UmbracoHelper.cs#L992</cref>
        /// </see>
        private static IPublishedContent TypedDocumentById(Guid guid) {

            // Look op the media item by it's ID if in the lookup table
            if (GuidLookupTable.TryGetValue(guid, out int id)) {
                return UmbracoContext.Current.MediaCache.GetById(id);
            }

            // TODO: Fix so we don't use the database (Umbraco must support this first)
            
            var entityService = ApplicationContext.Current.Services.EntityService;
            var mediaAttempt = entityService.GetIdForKey(guid, UmbracoObjectTypes.Media);
            var doc = mediaAttempt.Success ? UmbracoContext.Current.MediaCache.GetById(mediaAttempt.Result) : null;

            if (doc == null) return null;

            // Add the GUID to the lookup table
            GuidLookupTable[guid] = doc.Id;

            return doc;

        }

        #endregion

    }

}