using System;
using Skybrud.UmbracoEssentials.Content;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Skybrud.UmbracoEssentials.Extensions.PublishedContent {
    
    public static partial class PublishedContentExtensions {

        public static IPublishedContent TypedContent(this IPublishedContent content, string propertyAlias, bool recursive = false) {
            return ContentUtils.TypedContent(content.GetPropertyValue<string>(propertyAlias, recursive) ?? "");
        }

        public static T TypedContent<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            return ContentUtils.TypedContent(content.GetPropertyValue<string>(propertyAlias) ?? "", func);
        }

        public static IPublishedContent[] TypedCsvContent(this IPublishedContent content, string propertyAlias, bool recursive = false) {
            return ContentUtils.TypedCsvContent(content.GetPropertyValue<string>(propertyAlias, recursive) ?? "");
        }

        public static T[] TypedCsvContent<T>(this IPublishedContent content, string propertyAlias, Func<IPublishedContent, T> func) {
            return ContentUtils.TypedCsvContent(content.GetPropertyValue<string>(propertyAlias) ?? "", func);
        }

    }

}