using System;
using System.Web;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Strings;
using Skybrud.Essentials.Time;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Extensions.PublishedContent {

    /// <summary>
    /// Static class with various extension methods for <see cref="IPublishedContent"/>.
    /// </summary>
    public static partial class PublishedContentExtensions {

        /// <summary>
        /// Returns whether the specified <paramref name="content"/> item should be hidden in navigation (if the
        /// <code>umbracoNaviHide</code> property has been checked in Umbraco).
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/> representing the item.</param>
        public static bool IsHidden(this IPublishedContent content) {
            return content.GetPropertyValue<bool>("umbracoNaviHide");
        }

        /// <summary>
        /// Gets whether the specified <paramref name="content"/> item is a descendant of a node with the
        /// <paramref name="contentId"/>.
        /// </summary>
        /// <param name="content">An instance of <see cref="IPublishedContent"/> representing the
        /// item/descendant.</param>
        /// <param name="contentId">The ID of the ancestor.</param>
        public static bool IsDescendantOf(this IPublishedContent content, int contentId) {
            return content.Path.Contains("," + contentId + ",");
        }

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

        public static float GetFloat(this IPublishedContent content, string propertyAlias) {
            return content == null ? default(float) : content.GetPropertyValue<float>(propertyAlias);
        }

        public static T GetFloat<T>(this IPublishedContent content, string propertyAlias, Func<float, T> func) {
            return content == null ? default(T) : func(content.GetPropertyValue<float>(propertyAlias));
        }

        public static double GetDouble(this IPublishedContent content, string propertyAlias) {
            return content == null ? default(double) : content.GetPropertyValue<double>(propertyAlias);
        }

        public static T GetDouble<T>(this IPublishedContent content, string propertyAlias, Func<double, T> func) {
            return content == null ? default(T) : func(content.GetPropertyValue<double>(propertyAlias));
        }

        public static bool GetBoolean(this IPublishedContent content, string propertyAlias) {
            return content == null ? default(bool) : StringUtils.ParseBoolean(content.GetPropertyValue(propertyAlias) + "");
        }

        public static T GetBoolean<T>(this IPublishedContent content, string propertyAlias, Func<bool, T> func) {
            return content == null ? default(T) : func(StringUtils.ParseBoolean(content.GetPropertyValue(propertyAlias) + ""));
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

        private static IPublishedContent[] ToArray(IPublishedContent content) {
            return content == null ? new IPublishedContent[0] : new [] {content};
        }

    }

}