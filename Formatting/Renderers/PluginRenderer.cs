using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using WikiPlex.Plugins;

namespace WikiPlex.Formatting.Renderers
{
	class PluginRenderer : WikiPlex.Formatting.Renderers.Renderer, IWikiRenderer
	{
		protected override ICollection<string> ScopeNames
		{
			get
			{
				return new[] { 
					ScopeName.PluginTagStart, 
					ScopeName.PluginTagEnd, 
					ScopeName.PluginBody ,
					ScopeName.PluginClosedTag
				};
			}
		}

		IWikiPlugin currentPlugin = null;
		WikiEngine engine = null;

		protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
		{
			if (scopeName == ScopeName.PluginClosedTag)
			{
				Regex reg = new Regex(@"\<([^:]+):\s*([^/]*)/\>");
				var match = reg.Match(input);

				Dictionary<string, string> arguments = new Dictionary<string, string>();

				string pluginName = match.Groups[1].Value;
				pluginName = pluginName.ToLower();

				if (match.Groups.Count == 3)
				{
					string pluginArgs = match.Groups[2].Value;

					string[] args = ArgumentParserUtil.Parse(pluginArgs);
					foreach (var o in args)
					{
						string[] arg = o.Split('=');
						arguments[arg[0]] = arg[1];
					}
				}

				var plugin = engine.FindPlugin(pluginName);
				if (plugin == null)
					return string.Format("プラグインが見つかりません({0})", pluginName);

				StringBuilder renderedText = new StringBuilder();
				renderedText.Append(plugin.RendererBegin(arguments));

				if (plugin.IsBodyRenderer)
					renderedText.Append(plugin.SingleRenderer());

				renderedText.Append(plugin.RendererEnd());

				return renderedText.ToString();
			}
			else if (scopeName == ScopeName.PluginTagStart)
			{
				Regex reg = new Regex(@"\<([^:]+):\s*([^\>]*)\>");
				var match = reg.Match(input);

				Dictionary<string, string> arguments = new Dictionary<string, string>();

				string pluginName = match.Groups[1].Value;
				pluginName = pluginName.ToLower();

				if (match.Groups.Count == 3)
				{
					string pluginArgs = match.Groups[2].Value;

					string[] args = ArgumentParserUtil.Parse(pluginArgs);
					foreach (var o in args)
					{
						string[] arg = o.Split('=');
						arguments[arg[0]] = arg[1];
					}
				}

				var plugin = engine.FindPlugin(pluginName);
				if (plugin == null)
					return string.Format("プラグインが見つかりません({0})", pluginName);

				currentPlugin = plugin;
				return currentPlugin.RendererBegin(arguments);
			}

			if (scopeName == ScopeName.PluginTagEnd)
			{
				if (currentPlugin != null)
				{
					string result = currentPlugin.RendererEnd();
					currentPlugin = null;
					return result;
				}
				return "";
			}

			if (scopeName == ScopeName.PluginBody)
			{
				// 未登録のプラグインの場合は、本文はパースせずに空文字をかえす。
				if (currentPlugin != null)
				{
					if (currentPlugin.IsBodyRenderer)
					{
						return currentPlugin.Renderer(input);
					}
					else
					{
						var engine = new WikiEngine();
						engine.IsInsertReturnCodeAtEnd = false;
						string output = engine.Render(input);
						return output;
					}
				}
				else
				{
					return "";
				}
			}

			return null;
		}




		#region IBinWikiRenderer メンバー
		public IWikiEngine Engine
		{
			set { engine = (WikiEngine)value; }
		}

		public void PreRenderer()
		{
		}

		public void PostRenderer()
		{
		}

		#endregion
	}
}
