using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;
using WikiPlex.Common;
using WikiPlex.Compilation;
using WikiPlex.Compilation.Macros;
using WikiPlex.Formatting;
using WikiPlex.Formatting.Renderers;
using WikiPlex.Parsing;
using WikiPlex.Plugins;

namespace WikiPlex
{
	/// <summary>
	/// The public entry point for the wiki engine.
	/// </summary>
	public class WikiEngine : IWikiEngine
	{
		private static readonly MacroCompiler Compiler = new MacroCompiler();
		private static readonly Regex NewLineRegex = new Regex(@"(?<!\r|</tr>|</li>|</ul>|</ol>|<hr />|</blockquote>)(?:\n|&#10;)(?!<h[1-6]>|<hr />|<ul>|<ol>|</li>|</blockquote>)");
		private static readonly Regex PreRegex = new Regex(@"(?s)((?><pre>)(?>.*?</pre>))");

		private readonly Parser parser;

		public static IEnumerable<IRenderer> GetRenderers()
		{
			var siteRenderers = new IRenderer[] { 
				new TableRenderer(),
				new PluginRenderer(),
				new HeaderRenderer(),
				new CommentRenderer()
			};

			return WikiPlex.Renderers.All.Union(siteRenderers);
		}


		//=====================================================================
		#region コンストラクタ/デストラクタ
		//=====================================================================
		
		/// <summary>
		/// Instantiates a new instance of the <see cref="WikiEngine"/>.
		/// </summary>
		public WikiEngine()
			: this(new Parser(Compiler),WikiPlugins.All)
		{ }

		/// <summary>
		/// プラグインリストを使用する
		/// </summary>
		/// <param name="plugins"></param>
		public WikiEngine(IEnumerable<IWikiPlugin> plugins)
			: this(new Parser(Compiler), plugins)
		{ }

		/// <summary>
		/// Instantiates a new instance of the <see cref="WikiEngine"/>.
		/// </summary>
		/// <param name="parser">The macro parser to use.</param>
		protected internal WikiEngine(Parser parser,IEnumerable<IWikiPlugin> plugins)
		{
			this.parser = parser;
			this.plugins = plugins;
		}
		#endregion

		//=====================================================================
		#region 公開メソッド
		//=====================================================================
		
		/// <summary>
		/// Renders the wiki content using the statically registered macros and renderers.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <returns>The rendered html content.</returns>
		public virtual string Render(string wikiContent)
		{
			return Render(wikiContent, GetRenderers());
		}

		/// <summary>
		/// Renders the wiki content using the a custom formatter with statically registered macros.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <param name="formatter">The custom formatter used when rendering.</param>
		/// <returns>The rendered html content.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown when formatter is null.</exception>
		public virtual string Render(string wikiContent, Formatter formatter)
		{
			return Render(wikiContent, Macros.All, formatter);
		}

		/// <summary>
		/// Renders the wiki content using the specified macros and statically registered renderers.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <param name="macros">A collection of macros to be used when rendering.</param>
		/// <returns>The rendered html content.</returns>
		/// <exception cref="System.ArgumentNullException">Thrown when macros is null.</exception>
		/// <exception cref="System.ArgumentException">Thrown when macros is an empty enumerable.</exception>
		public virtual string Render(string wikiContent, IEnumerable<IMacro> macros)
		{
			return Render(wikiContent, macros, GetRenderers());
		}

		/// <summary>
		/// Renders the wiki content using the specified renderers with statically registered macros.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <param name="renderers">A collection of renderers to be used when rendering.</param>
		/// <returns>The rendered html content.</returns>
		public virtual string Render(string wikiContent, IEnumerable<IRenderer> renderers)
		{
			return Render(wikiContent, Macros.All, renderers);
		}

		/// <summary>
		/// Renders the wiki content using the specified macros and renderers.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <param name="macros">A collection of macros to be used when rendering.</param>
		/// <param name="renderers">A collection of renderers to be used when rendering.</param>
		/// <returns>The rendered html content.</returns>
		public virtual string Render(string wikiContent, IEnumerable<IMacro> macros, IEnumerable<IRenderer> renderers)
		{
			Guard.NotNullOrEmpty(renderers, "renderers");

			var formatter = new Formatter(renderers);
			return Render(wikiContent, macros, formatter);
		}

		/// <summary>
		/// Renders the wiki content using the specified macros and custom formatter.
		/// </summary>
		/// <param name="wikiContent">The wiki content to be rendered.</param>
		/// <param name="macros">A collection of macros to be used when rendering.</param>
		/// <param name="formatter">The custom formatter used when rendering.</param>
		/// <returns>The rendered html content.</returns>
		/// <exception cref="System.ArgumentNullException">
		/// <para>Thrown when macros is null.</para>
		/// <para>- or -</para>
		/// <para>Thrown when formatter is null.</para>
		/// </exception>
		/// <exception cref="System.ArgumentException">Thrown when macros is an empty enumerable.</exception>
		public virtual string Render(string wikiContent, IEnumerable<IMacro> macros, Formatter formatter)
		{
			Guard.NotNullOrEmpty(macros, "macros");
			Guard.NotNull(formatter, "formatter");

			if (string.IsNullOrEmpty(wikiContent))
				return wikiContent;

			foreach (var plugin in Plugins)
			{
				plugin.PreRenderer(this);
			}

			foreach (var post in formatter.Renderers)
			{
				var r = post as IWikiRenderer;
				if (r != null)
				{
					r.Engine = this;
					r.PreRenderer();
				}
			}

			if (IsInsertReturnCodeAtEnd)
				wikiContent += "\n";

			wikiContent = wikiContent.Replace("\r\n", "\n");
			parser.Parse(wikiContent, macros, ScopeAugmenters.All, formatter.RecordParse);
			var rt = ReplaceNewLines(formatter.Format(wikiContent));

			foreach (var post in formatter.Renderers)
			{
				var r = post as IWikiRenderer;
				if (r != null) r.PostRenderer();
			}

			foreach (var plugin in Plugins)
			{
				rt = plugin.PostRenderer(this, formatter, rt);
			}

			return rt;
		}

		public IWikiPlugin FindPlugin(string pluginName)
		{
			var r = from u in Plugins
					where u.Id == pluginName
					select u;
			var res = r.FirstOrDefault();
			return res;
		}
		#endregion


		//=====================================================================
		#region プロパティ
		//=====================================================================
		public IEnumerable<IWikiPlugin> Plugins { get { return plugins; } }

		/// <summary>
		/// レンダリングする文字列の末尾に強制的に改行コードを1つ追加するかどうか。
		/// </summary>
		public bool IsInsertReturnCodeAtEnd { get; set; }
		#endregion


		//=====================================================================
		#region フィールド
		//=====================================================================
		IEnumerable<IWikiPlugin> plugins;
		#endregion

		private static string ReplaceNewLines(string input)
		{
			string replacedInput = NewLineRegex.Replace(input, "<br />");

			var match = PreRegex.Match(replacedInput);
			if (!match.Success)
				return replacedInput;

			// now we need to remove any <br /> tags within <pre></pre> tags
			var output = new StringBuilder(input.Length);
			int currentIndex = 0;
			while (match.Success)
			{
				output.Append(replacedInput.Substring(currentIndex, match.Groups[0].Index - currentIndex));
				output.Append(replacedInput.Substring(match.Groups[0].Index, match.Groups[0].Length).Replace("<br />", "\n"));
				currentIndex = match.Groups[0].Index + match.Groups[0].Length;
				match = match.NextMatch();
			}

			output.Append(replacedInput.Substring(currentIndex));

			return output.ToString();
		}
	}
}