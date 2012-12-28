using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiPlex.Plugins
{
	/// <summary>
	/// フォントプラグイン
	/// </summary>
	public class FontPlugin : IWikiPlugin
	{
		#region IBinWikiPlugin メンバー

		public void PreRenderer(WikiEngine engine)
		{
		}

		public string PostRenderer(WikiEngine engine, WikiPlex.Formatting.Formatter formatter, string renderedHtml)
		{
			return renderedHtml;
		}

		public string RendererBegin(Dictionary<string, string> arguments)
		{
			string fontColor = "";
			if (arguments.ContainsKey("color"))
			{
				var colorCode = arguments["color"].Trim();
				fontColor = "color=\"" + colorCode + "\"";
			}

			return string.Format("<font {0}>", fontColor);
		}

		public string RendererEnd()
		{
			return "</font>";
		}

		public string Renderer(string body)
		{
			return "";
		}

		public string SingleRenderer()
		{
			return "";
		}

		public bool IsBodyRenderer
		{
			get { return false; }
		}

		public string Id
		{
			get { return "font"; }
		}

		#endregion
	}
}
