using System;
using System.Web;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Time;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Extensions.PublishedContent {

    /// <summary>
    /// Static class with various extension methods for <see cref="IPublishedContent"/>.
    /// </summary>
    public static partial class PublishedContentExtensions {

        /// <summary>
        /// Gets a string value of the property with the specified <paramref name="propertyAlias"/>, or
        /// <code>null</code> if not found.
        /// </summary>
        /// <param name="content">The instance of <see cref="IPublishedContent"/> that this method extends.</param>
        /// <param name="propertyAlias">The alias of the property.</param>
        /// <returns>Returns the string value of the property, or <code>null</code> if the property could not be found.</returns>
        public static string GetString(this IPublishedContent content, string propertyAlias) {
            return content == null ? null : content.GetPropertyValue<string>(propertyAlias);
        }

        public static T GetString<T>(this IPublishedContent content, string propertyAlias, Func<string, T> func) {
            return content == null ? default(T) : func(content.GetPropertyValue<string>(propertyAlias));
        }

        public static HtmlString GetHtmlString(this IPublishedContent content, string propertyAlias) {
            return content == null ? null : content.GetPropertyValue<HtmlString>(propertyAlias);
        }


        public static string[] GetStringArray(this IPublishedContent content, string propertyAlias) {
            return GetStringArray(content, propertyAlias, new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetStringArray(this IPublishedContent content, string propertyAlias,
            StringSplitOptions options) {
            return GetStringArray(content, propertyAlias, new[] { ',' }, options);
        }

        public static string[] GetStringArray(this IPublishedContent content, string propertyAlias, char separator, StringSplitOptions options) {
            return GetStringArray(content, propertyAlias, new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static string[] GetStringArray(this IPublishedContent content, string propertyAlias, char[] separator, StringSplitOptions options) {
            return content == null ? new string[0] : (content.GetPropertyValue<string>(propertyAlias) ?? "").Split(separator, options);
        }

        public static int GetInt32(this IPublishedContent content, string propertyAlias) {
            return content == null ? default(int) : content.GetPropertyValue<int>(propertyAlias);
        }

        public static T GetInt32<T>(this IPublishedContent content, string propertyAlias, Func<int, T> func) {
            return content == null ? default(T) : func(content.GetPropertyValue<int>(propertyAlias));
        }

        public static long GetInt64(this IPublishedContent content, string propertyAlias) {
            return content == null ? default(long) : content.GetPropertyValue<long>(propertyAlias);
        }

        public static T GetInt64<T>(this IPublishedContent content, string propertyAlias, Func<long, T> func) {
            return content == null ? default(T) : func(content.GetPropertyValue<long>(propertyAlias));
        }

        public static DateTime GetDateTime(this IPublishedContent content, string propertyAlias) {
            return content == null || !content.HasValue(propertyAlias) ? default(DateTime) : content.GetPropertyValue<DateTime>(propertyAlias);
        }

        public static DateTime GetDateTime(this IPublishedContent content, string propertyAlias, DateTime fallback) {
            return content == null || !content.HasValue(propertyAlias) ? fallback : content.GetPropertyValue<DateTime>(propertyAlias);
        }

        public static EssentialsDateTime GetEssentialsDateTime(this IPublishedContent content, string propertyAlias) {
            return content == null || !content.HasValue(propertyAlias) ? null : new EssentialsDateTime(content.GetPropertyValue<DateTime>(propertyAlias));
        }

        public static JObject GetJObject(this IPublishedContent content, string propertyAlias) {
            return content == null ? null : content.GetPropertyValue<JObject>(propertyAlias);
        }

        public static T GetJObject<T>(this IPublishedContent content, string propertyAlias, Func<JObject, T> function) {
            return content == null ? default(T) : function(content.GetPropertyValue<JObject>(propertyAlias));
        }

        public static JArray GetJArray(this IPublishedContent content, string propertyAlias) {
            return content == null ? null : content.GetPropertyValue<JArray>(propertyAlias);
        }

        public static T GetJArray<T>(this IPublishedContent content, string propertyAlias, Func<JArray, T> function) {
            return content == null ? default(T) : function(content.GetPropertyValue<JArray>(propertyAlias));
        }

    }

}