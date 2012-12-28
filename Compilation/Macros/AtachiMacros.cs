using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiPlex.Compilation.Macros
{

	// ◆改行コード(\n)自体を正規表現のマッチングに適合させる場合、末尾アンカーである「$」「\z」は使用しない。
	//   (a\n) → a     "a"に改行コードが続く文字列にマッチする。
	//   (a\n)$ → ×   "a"に改行コードが続く文字列で、文字列の末尾にマッチするが、既に改行コードがマッチ済みなので、「$」に対応する改行コードが無いためこの正規表現にマッチする文字列はない。


	public class PluginMacro : IMacro
	{
		public string Id { get { return "Plugin"; } }

		public IList<MacroRule> Rules
		{
			get
			{
				return new List<MacroRule> {
					new MacroRule(EscapeRegexPatterns.FullEscape),
					new MacroRule(@"(?si)(\<(\w+):\s*[^/]*/\>)",
								   new Dictionary<int, string>
								   {
									{1, ScopeName.PluginClosedTag},
								   }),
					new MacroRule(@"(?si)(\<(?<tag>\w+):\s*[^\>]*\>(?![\r\n]{1,2}))(.*?)(</\k<tag>\>)",
								   new Dictionary<int, string>
									   {
										   {1, ScopeName.PluginTagStart},
										   {2, ScopeName.PluginBody},
										   {3, ScopeName.PluginTagEnd}
									   }),
					new MacroRule(@"(?si)(\<(?<tag>\w+):\s*[^\>]*\>)(\r?\n)(.*?)(\r?\n\</\k<tag>\>(?:\r?\n)?)",
								   new Dictionary<int, string>
									   {
										   {1, ScopeName.PluginTagStart},
										   {2, ScopeName.Remove},
										   {3, ScopeName.PluginBody},
										   {4, ScopeName.PluginTagEnd}
									   }),
				};
			}
		}
	}

	

	public class CommentMacro : IMacro
	{
		public string Id { get { return "Comment Line"; } }

		public IList<MacroRule> Rules
		{
			get
			{
				return new List<MacroRule> {
						new MacroRule(EscapeRegexPatterns.FullEscape),
						new MacroRule(@"(?im)^(\/\/)([^\r\n]*)(\r?\n|$)",
							new Dictionary<int,string>
							{
								{1,ScopeName.Remove},
								{2,ScopeName.Comment},
								{3,ScopeName.Remove},
							})

					};
			}
		}
	}

}
