using System;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;
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
            
            // Get the first ID in a comma separated string
            int mediaId = (content.GetPropertyValue<string>(propertyAlias, recursive) ?? "").CsvToInt().FirstOrDefault();

            // Parse the value and attempt to find the media node in the cache
            return mediaId > 0 && UmbracoContext.Current != null ? UmbracoContext.Current.MediaCache.GetById(mediaId) : null;
        
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
            
            if (func == null) throw new ArgumentNullException("func");
            
            // Find the media using the method overload
            IPublishedContent media = TypedMedia(content, propertyAlias);

            // Convert the media (or just return null if not found)
            return media == null ? default(T) : func(media);
        
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

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new IPublishedContent[0];
 
            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in content.GetPropertyValue<string>(propertyAlias, recursive).CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select item
            ).ToArray();
        
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

            if (func == null) throw new ArgumentNullException("func");

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new T[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in content.GetPropertyValue<string>(propertyAlias).CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select func(item)
            ).ToArray();
        
        }

    }

}