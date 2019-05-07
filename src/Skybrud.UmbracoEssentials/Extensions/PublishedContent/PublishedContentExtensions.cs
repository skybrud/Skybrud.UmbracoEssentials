using System;
using System.Web;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Strings;
using Skybrud.Essentials.Time;
using Umbraco.Core;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web;

// ReSharper disable RedundantTypeSpecificationInDefaultExpression

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
            return content.Value<bool>(Constants.Conventions.Content.NaviHide);
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
            return content?.Value<string>(propertyAlias);
        }

        public static T GetString<T>(this IPublishedContent content, string propertyAlias, Func<string, T> func) {
            return content == null ? default(T) : func(content.Value<string>(propertyAlias));
        }

        public static HtmlString GetHtmlString(this IPublishedContent content, string propertyAlias) {
            return content?.Value<HtmlString>(propertyAlias);
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
            return content == null ? new string[0] : (content.Value<string>(propertyAlias) ?? "").Split(separator, options);
        }

        public static int GetInt32(this IPublishedContent content, string propertyAlias) {
            return content?.Value<int>(propertyAlias) ?? default(int);
        }

        public static T GetInt32<T>(this IPublishedContent content, string propertyAlias, Func<int, T> func) {
            return content == null ? default(T) : func(content.Value<int>(propertyAlias));
        }

        public static long GetInt64(this IPublishedContent content, string propertyAlias) {
            return content?.Value<long>(propertyAlias) ?? default(long);
        }

        public static T GetInt64<T>(this IPublishedContent content, string propertyAlias, Func<long, T> func) {
            return content == null ? default(T) : func(content.Value<long>(propertyAlias));
        }

        public static float GetFloat(this IPublishedContent content, string propertyAlias) {
            return content?.Value<float>(propertyAlias) ?? default(float);
        }

        public static T GetFloat<T>(this IPublishedContent content, string propertyAlias, Func<float, T> func) {
            return content == null ? default(T) : func(content.Value<float>(propertyAlias));
        }

        public static double GetDouble(this IPublishedContent content, string propertyAlias) {
            return content?.Value<double>(propertyAlias) ?? default(double);
        }

        public static T GetDouble<T>(this IPublishedContent content, string propertyAlias, Func<double, T> func) {
            return content == null ? default(T) : func(content.Value<double>(propertyAlias));
        }

        public static bool GetBoolean(this IPublishedContent content, string propertyAlias) {
            return content != null && StringUtils.ParseBoolean(content.Value(propertyAlias) + string.Empty);
        }

        public static T GetBoolean<T>(this IPublishedContent content, string propertyAlias, Func<bool, T> func) {
            return content == null ? default(T) : func(StringUtils.ParseBoolean(content.Value(propertyAlias) + string.Empty));
        }

        public static DateTime GetDateTime(this IPublishedContent content, string propertyAlias) {
            return content == null || !content.HasValue(propertyAlias) ? default(DateTime) : content.Value<DateTime>(propertyAlias);
        }

        public static DateTime GetDateTime(this IPublishedContent content, string propertyAlias, DateTime fallback) {
            return content == null || !content.HasValue(propertyAlias) ? fallback : content.Value<DateTime>(propertyAlias);
        }

        public static EssentialsDateTime GetEssentialsDateTime(this IPublishedContent content, string propertyAlias) {
            return content == null || !content.HasValue(propertyAlias) ? null : new EssentialsDateTime(content.Value<DateTime>(propertyAlias));
        }

        public static JObject GetJObject(this IPublishedContent content, string propertyAlias) {
            return content?.Value<JObject>(propertyAlias);
        }

        public static T GetJObject<T>(this IPublishedContent content, string propertyAlias, Func<JObject, T> function) {
            return content == null ? default(T) : function(content.Value<JObject>(propertyAlias));
        }

        public static JArray GetJArray(this IPublishedContent content, string propertyAlias) {
            return content?.Value<JArray>(propertyAlias);
        }

        public static T GetJArray<T>(this IPublishedContent content, string propertyAlias, Func<JArray, T> function) {
            return content == null ? default(T) : function(content.Value<JArray>(propertyAlias));
        }

    }

}