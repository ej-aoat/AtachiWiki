using System.Collections.Generic;

namespace WikiPlex.Compilation.Macros
{
	public class TableMacro : IMacro
	{
		public string Id { get { return "Table"; } }

		public IList<MacroRule> Rules
		{
			get
			{
				return new List<MacroRule>
						   {
							   new MacroRule(
								   @"(^\{\|[^\n]*)(\n)", // Wiki "{|"
														   // 2番目のマッチングは、オプション指定文字列用。
								   new Dictionary<int,string>
								   {
									   {1, ScopeName.MTableBegin},
									   {2, ScopeName.Remove},
								   }),
								new MacroRule(
								   @"(^\|\})(\n)", // Wiki "|}"
								   new Dictionary<int,string>
								   {
									   {1,ScopeName.MTableEnd},
									   {2, ScopeName.Remove},
								   }),
								new MacroRule(
								   @"(^\|\-)([^\n]*)(\n)", // Wiki "|-"
								   new Dictionary<int,string>
								   {
									   {1,ScopeName.MTableNewLine},
									   {2, ScopeName.Remove},
									   {3, ScopeName.Remove},
								   }),
								new MacroRule(
								   @"(^\|\+)([^\n]*)(\n)", // Wiki "|+"
								   new Dictionary<int,string>
								   {
									   {1,ScopeName.MTableNewHeaderLine},
									   {2, ScopeName.Remove},
									   {3, ScopeName.Remove},
								   }),
								new MacroRule(
									@"(?im)^(\|.+)(\|)(.*)$",
									new Dictionary<int,string>
								   {
									   {1,ScopeName.MTableCellStart},
									   {2,ScopeName.Remove},
									   {3,ScopeName.MTableCell},
								   }),
								   
								new MacroRule(
									@"(?im)^(\|)([^\|]*)$",
									new Dictionary<int,string>
								   {
									   {1,ScopeName.MTableCellStart},
									   {2,ScopeName.MTableCell},
								   }),
								   
						   };
			}
		}
	}
}