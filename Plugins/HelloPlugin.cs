using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Formatting;

namespace WikiPlex.Plugins
{
	public class HelloPlugin : IWikiPlugin
	{
		#region IBinWikiPlugin メンバー

		public bool IsBodyRenderer
		{
			get { return true; }
		}

		public string Id
		{
			get { return "hello"; }
		}


		public void PreRenderer(WikiEngine engine)
		{
			Console.WriteLine("PreRenderer");
		}

		public string PostRenderer(WikiEngine engine, Formatter formatter, string renderedHtml)
		{
			return renderedHtml;
		}

		public string RendererBegin(Dictionary<string, string> arguments)
		{
			Console.WriteLine("RendererBegin");
			return "";
		}

		public string RendererEnd()
		{
			Console.WriteLine("RendererEnd");
			return "";
		}

		public string Renderer(string body)
		{
			Console.WriteLine("Renderer");
			return "";
		}

		public string SingleRenderer()
		{
			Console.WriteLine("SingleRenderer");
			return "Hello";
		}

		#endregion
	}
}
