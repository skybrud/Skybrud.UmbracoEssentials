using System;
using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Composing;

namespace Skybrud.UmbracoEssentials.Extensions.Udi {

    public static class UdiExtensions {
		
        /// <summary>
		/// Find node in Umbraco and return the guid of the node.
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public static Guid? GetGuid(this int nodeId) {
			IPublishedContent node = Current.UmbracoContext?.ContentCache.GetById(nodeId);
            return node?.Key;
		}

		/// <summary>
		/// Find node in Umbraco and returns the guid-string (w. - removed)
		/// </summary>
		/// <param name="nodeId"></param>
		/// <returns></returns>
		public static string GetGuidStringExamine(this int nodeId) {
			Guid? g = GetGuid(nodeId);
			return g == null ? string.Empty : g.ToString().Replace("-", "");
        }

		/// <summary>
		/// Find nodes in Umbraco and returns the guid-strings (w. - removed)
		/// </summary>
		/// <param name="nodeIds">int[]</param>
		/// <returns></returns>
		public static string GetGuidsStringExamine(this int[] nodeIds) {

            List<string> guids = new List<string>();

			foreach (int i in nodeIds) {
				Guid? g = GetGuid(i);
				if (g == null) continue;
				guids.Add(g.ToString().Replace("-", ""));
			}

			return string.Join(" ", guids.ToArray());
		}

		/// <summary>
		/// Find nodes in Umbraco and returns the guid-strings (w. - removed)
		/// </summary>
		/// <param name="nodeIds">string</param>
		/// <returns></returns>
		public static string GetGuidsStringExamine(this string nodeIds) {
			return nodeIds.ToInt32Array().GetGuidsStringExamine();
		}

    }

}