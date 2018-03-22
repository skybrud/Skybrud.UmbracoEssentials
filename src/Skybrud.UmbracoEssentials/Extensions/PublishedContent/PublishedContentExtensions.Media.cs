using System;
using System.Collections.Generic;
using System.Linq;
using Skybrud.UmbracoEssentials.Media;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Extensions.PublishedContent {
    
    public static partial class PublishedContentExtensions {

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified in
        /// the property with the specified <paramref name="propertyAlias"/>.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/>.</param>
        /// <param name="propertyAlias">The alias of the property containing the ID.</param>
        /// <param name="recursive">A value indicating whether to recurse.</param>
        /// <returns>Instance of <see cref="IPublishedContent"/> if found, otherwise <code>null</code>.</returns>
        public static IPublishedContent TypedMedia(this IPublishedContent content, string propertyAlias, bool recursive = false) {

            // Get the property value
            object propertyValue = content?.GetPropertyValue(propertyAlias, recursive);
            if (propertyValue == null) return null;

            // Handle various value types
            switch (propertyValue) {

                case IPublishedContent pc:
                    return pc;

                case List<IPublishedContent> lc:
                    return lc.FirstOrDefault();

                case string str:
                    return MediaUtils.TypedMedia(str);

                default:
                    return null;

            }
            
        }

        /// <summary>
        /// Returns an instance of <see cref="IPublishedContent"/> from the media cache based on the ID specified in
        /// the property with the specified <paramref name="propertyAlias"/>. If found, the
        /// <see cref="IPublishedContent"/> is converted to the type of <typeparamref name="T"/> using the specified
        /// <paramref name="func"/>.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/>.</param>
        /// <param name="propertyAlias">The alias of the property containing the ID.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Instance of <typeparamref name="T"/> if found, otherwise <code>default(T)</code>.</returns>
        public static T TypedMedia<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            IPublishedContent item = TypedMedia(content, propertyAlias);
            return item == null ? default(T) : func(item);
        }

        /// <summary>
        /// Converts the comma seperated IDs of the property with the specified <paramref name="propertyAlias"/> into
        /// an array of <see cref="IPublishedContent"/> by using the media cache.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/>.</param>
        /// <param name="propertyAlias">The alias of the property containing the IDs.</param>
        /// <param name="recursive">A value indicating whether to recurse.</param>
        /// <returns>Array of <see cref="IPublishedContent"/>.</returns>
        public static IPublishedContent[] TypedCsvMedia(this IPublishedContent content, string propertyAlias, bool recursive = false) {

            // Get the property value
            object propertyValue = content?.GetPropertyValue(propertyAlias, recursive);
            if (propertyValue == null) return null;

            // Handle various value types
            switch (propertyValue) {

                case IPublishedContent pc:
                    return new []{ pc };

                case List<IPublishedContent> lc:
                    return lc.ToArray();

                case string str:
                    return MediaUtils.TypedCsvMedia(str);

                default:
                    return new IPublishedContent[0];

            }

        }

        /// <summary>
        /// Converts the comma seperated IDs of the property with the specified <paramref name="propertyAlias"/> into
        /// an array of <typeparamref name="T"/> by using the media cache. Each media is converted to the type of
        /// <typeparamref name="T"/> using the specified <paramref name="func"/>.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/>.</param>
        /// <param name="propertyAlias">The alias of the property containing the IDs.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvMedia<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            return TypedCsvMedia(content, propertyAlias).Select(func).ToArray();
        }

    }

}