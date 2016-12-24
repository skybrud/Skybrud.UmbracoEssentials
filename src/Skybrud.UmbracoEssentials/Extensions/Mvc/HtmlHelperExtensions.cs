using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Skybrud.UmbracoEssentials.Extensions.Mvc {
    
    /// <summary>
    /// Static class with various extension methods for <see cref="HtmlHelper"/>.
    /// </summary>
    public static class HtmlHelperExtensions {

        /// <summary>
        /// Generates a cachable URL based on the specified <paramref name="url"/>. If <paramref name="url"/> matches a
        /// local file, the timestamp of that file will be appended to the query for cache busting purposes. For
        /// external files - or if the file couldn't be found on the disk - the URL is not modified.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="url">The URL to be converted.</param>
        /// <returns>Returns the cachable URL.</returns>
        public static string GetCachableUrl(this HtmlHelper helper, string url) {
            return GetCachableUrl(url);
        }

        /// <summary>
        /// Generates a cachable URL based on the specified <paramref name="url"/>. If <paramref name="url"/> matches a
        /// local file, the timestamp of that file will be appended to the query for cache busting purposes. For
        /// external files - or if the file couldn't be found on the disk - the URL is not modified.
        /// </summary>
        /// <param name="helper">The HTML helper instance that this method extends.</param>
        /// <param name="url">The URL to be converted.</param>
        /// <returns>Returns the cachable URL.</returns>
        public static string GetCachableUrl<T>(this HtmlHelper<T> helper, string url) {
            return GetCachableUrl(url);
        }

        /// <summary>
        /// Generates a cachable URL based on the specified <paramref name="url"/>. If <paramref name="url"/> matches a
        /// local file, the timestamp of that file will be appended to the query for cache busting purposes. For
        /// external files - or if the file couldn't be found on the disk - the URL is not modified.
        /// </summary>
        /// <param name="url">The URL to be converted.</param>
        /// <returns>Returns the cachable URL.</returns>
        public static string GetCachableUrl(string url) {
            if (String.IsNullOrWhiteSpace(url)) return "";
            if (!url.StartsWith("/") || url.StartsWith("//")) return url;
            FileInfo file = new FileInfo(HttpContext.Current.Server.MapPath("~" + url));
            if (!file.Exists) return url;
            long ticks = file.LastWriteTimeUtc.Ticks;
            return url + (url.Contains("?") ? "&v=" : "?v=") + ticks;
        }

    }

}