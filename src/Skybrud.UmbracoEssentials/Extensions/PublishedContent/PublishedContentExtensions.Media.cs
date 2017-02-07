using System;
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
        /// <returns>Instance of <see cref="IPublishedContent"/> if found, otherwise <code>NULL</code>.</returns>
        public static IPublishedContent TypedMedia(this IPublishedContent content, string propertyAlias, bool recursive = false) {
            return MediaUtils.TypedMedia(content.GetPropertyValue<string>(propertyAlias, recursive) ?? "");
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
        /// <returns>Instance of <typeparamref name="T"/> if found, otherwise <code>NULL</code>.</returns>
        public static T TypedMedia<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            return MediaUtils.TypedMedia(content.GetPropertyValue<string>(propertyAlias) ?? "", func);
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
            return MediaUtils.TypedCsvMedia(content.GetPropertyValue<string>(propertyAlias, recursive) ?? "");
        }

        /// <summary>
        /// Converts the comma seperated IDs of the property with the specified <paramref name="propertyAlias"/> into
        /// an array of <code>T</code> by using the media cache. Each media is converted to the type of <code>T</code>
        /// using the specified <paramref name="func"/>.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/>.</param>
        /// <param name="propertyAlias">The alias of the property containing the IDs.</param>
        /// <param name="func">The delegate function to be used for the conversion.</param>
        /// <returns>Array of <typeparamref name="T"/>.</returns>
        public static T[] TypedCsvMedia<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            return MediaUtils.TypedCsvMedia(content.GetPropertyValue<string>(propertyAlias) ?? "", func);
        }

    }

}