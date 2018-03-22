using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Configuration;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Xml;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Content {
    
    /// <summary>
    /// Static class with various utility methods for working with content.
    /// </summary>
    public static class ContentUtils {

        #region Fields

        /// <summary>
        /// Gets a reference to the internal lookup table of GUIDs.
        /// </summary>
        public static readonly Dictionary<Guid, int> GuidLookupTable = new Dictionary<Guid, int>();

        #endregion

        #region Public methods

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the content cache based on the ID specified by
        /// <paramref name="str"/>.
        /// </summary>
        /// <param name="str">An instance of <see cref="String"/> with the ID of the content item.</param>
        /// <returns>An instance of <see cref="IPublishedContent"/> if found, otherwise <code>null</code>.</returns>
        public static IPublishedContent TypedContent(string str) {

            if (UmbracoContext.Current == null) return null;
            if (String.IsNullOrWhiteSpace(str)) return null;
            
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
            if (UmbracoContext.Current == null) return new IPublishedContent[0];
            
            // Also just return an empty array if the string is either NULL or empty
            if (String.IsNullOrWhiteSpace(str)) return new IPublishedContent[0];

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
            if (UmbracoContext.Current == null) return new T[0];

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
            if (String.IsNullOrWhiteSpace(id)) return null;
            if (Guid.TryParse(id.Replace("umb://document/", ""), out Guid guid)) return TypedDocumentById(guid);
            if (Int32.TryParse(id, out Int32 numeric)) return UmbracoContext.Current.ContentCache.GetById(numeric);
            return null;
        }
        
        /// <see>
        ///     <cref>https://github.com/umbraco/Umbraco-CMS/blob/3d90c2b83f76d398f28500e35cacd5944f5c1971/src/Umbraco.Web/PublishedContentQuery.cs#L235</cref>
        /// </see>
        private static IPublishedContent TypedDocumentById(Guid guid) {

            // Look op the content item by it's ID if in the lookup table
            if (GuidLookupTable.TryGetValue(guid, out int id)) {
                return UmbracoContext.Current.ContentCache.GetById(id);
            }

            // TODO: Fix so we don't use expesnsive XPath queries (Umbraco must support this first)

            var legacyXml = UmbracoConfig.For.UmbracoSettings().Content.UseLegacyXmlSchema;
            var xpath = legacyXml ? "//node [@key=$guid]" : "//* [@key=$guid]";
            var doc = UmbracoContext.Current.ContentCache.GetSingleByXPath(xpath, new XPathVariable("guid", guid.ToString()));

            if (doc == null) return null;
            
            // Add the GUID to the lookup table
            GuidLookupTable[guid] = doc.Id;

            return doc;

        }

        #endregion

    }

}