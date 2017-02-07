using System;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.UmbracoEssentials.Content;
using Skybrud.UmbracoEssentials.Media;
using Umbraco.Core.Models;

namespace Skybrud.UmbracoEssentials.Extensions.Json {
    
    /// <summary>
    /// Various extensions methods for <see cref="JObject"/>.
    /// </summary>
    public static class JObjectExtensions {

        public static IPublishedContent TypedContent(this JObject obj, string path) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, ContentUtils.TypedContent);
        }

        public static T TypedContent<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, x => ContentUtils.TypedContent(x, func));
        }

        public static IPublishedContent[] TypedCsvContent(this JObject obj, string path) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, ContentUtils.TypedCsvContent);
        }

        public static T[] TypedCsvContent<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {
            return obj.GetString(path, x => ContentUtils.TypedCsvContent(x, func));
        }

        public static IPublishedContent TypedMedia(this JObject obj, string path) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, MediaUtils.TypedMedia);
        }

        public static T TypedMedia<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, x => MediaUtils.TypedMedia(x, func));
        }

        public static IPublishedContent[] TypedCsvMedia(this JObject obj, string path) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, MediaUtils.TypedCsvMedia);
        }

        public static T[] TypedCsvMedia<T>(this JObject obj, string path, Func<IPublishedContent, T> func) {
            if (String.IsNullOrWhiteSpace(path)) throw new ArgumentNullException("path");
            return obj.GetString(path, x => MediaUtils.TypedCsvMedia(x, func));
        }

    }

}