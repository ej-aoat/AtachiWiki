using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Formatting;
using WikiPlex.Formatting.Renderers;

namespace WikiPlex.Plugins
{
	public class ContentsPlugin : IWikiPlugin
	{
		string replaceText = string.Empty;
		#region IBinWikiPlugin メンバー

		public void PreRenderer(WikiEngine engine)
		{
			replaceText = string.Empty;
		}

		public string PostRenderer(WikiEngine engine, Formatter formatter, string renderedHtml)
		{
			if (replaceText == string.Empty) return renderedHtml;

			foreach (var r in formatter.Renderers)
			{
				if (r is HeaderRenderer)
				{
					var headerRendrer = r as HeaderRenderer;
					StringBuilder sb = new StringBuilder("");
					sb.Append("<table class=\"toc\">").Append("<tr><th><div class=\"toctitle\">目次</div></th></tr><td>");

					sb.Append("<ul class=\"content_plugin_list\">"); // プラグインは常に最上位のULタグは出力する。

					foreach (var h1 in headerRendrer.TopLevelItems)
					{
						sb.Append(h1.ToHtml());
					}
					sb.Append("</ul>");

					sb.Append("</td></tr></table>");

					return renderedHtml.Replace(replaceText, sb.ToString());
				}
			}

			return renderedHtml;
		}

		public string RendererBegin(Dictionary<string, string> arguments)
		{
			return "";
		}

		public string RendererEnd()
		{
			return "";
		}

		public string Renderer(string body)
		{
			Guid g = System.Guid.NewGuid();
			string pass = g.ToString("N").Substring(0, 8);

			replaceText = "<<<<<<<" + pass;
			return replaceText;
		}

		public string SingleRenderer()
		{
			return Renderer("");
		}

		public bool IsBodyRenderer
		{
			get { return true; }
		}

		public string Id
		{
			get { return "contents"; }
		}

		#endregion
	}
}
