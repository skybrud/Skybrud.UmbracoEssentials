using System;
using System.Linq;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Media {
    
    public static class MediaUtils {

        public static IPublishedContent TypedMedia(string str) {

            // Get the first ID in a comma separated string
            int contentId = str.CsvToInt().FirstOrDefault();

            // Parse the value and attempt to find the media node in the cache
            return contentId > 0 && UmbracoContext.Current != null ? UmbracoContext.Current.MediaCache.GetById(contentId) : null;

        }

        public static T TypedMedia<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException("func");

            // Find the media using the method overload
            IPublishedContent content = TypedMedia(str);

            // Convert the media (or just return null if not found)
            return content == null ? default(T) : func(content);

        }

        public static IPublishedContent[] TypedCsvMedia(string str) {

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new IPublishedContent[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in str.CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select item
            ).ToArray();

        }

        public static T[] TypedCsvMedia<T>(string str, Func<IPublishedContent, T> func) {

            // A callback must be specified
            if (func == null) throw new ArgumentNullException("func");

            // If the Umbraco context isn't avaiable, we just return an empty array
            if (UmbracoContext.Current == null) return new T[0];

            // Look up each ID in the media cache and return the collection as an array
            return (
                from id in str.CsvToInt()
                let item = UmbracoContext.Current.MediaCache.GetById(id)
                where item != null
                select func(item)
            ).ToArray();

        }

    }

}