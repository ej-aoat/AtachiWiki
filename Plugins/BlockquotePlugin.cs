using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiPlex.Plugins
{
	/// <summary>
	/// 
	/// </summary>
	public class BlockquotePlugin : IWikiPlugin
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
			return "<blockquote>";
		}

		public string RendererEnd()
		{
			return "</blockquote>";
		}

		public string Renderer(string body)
		{
			return string.Empty;
		}

		public string SingleRenderer()
		{
			return string.Empty;
		}

		public bool IsBodyRenderer
		{
			get { return false; }
		}

		public string Id
		{
			get { return "blockquote"; }
		}

		#endregion
	}
}
