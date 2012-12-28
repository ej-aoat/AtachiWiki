using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Parsing;

namespace WikiPlex.Formatting.Renderers
{
	/// <summary>
	/// 
	/// </summary>
	public interface IWikiRenderer
	{
		void PreRenderer();
		void PostRenderer();

		IWikiEngine Engine { set; }

		/// <summary>
		/// レンダリング時のScopeを設定します。
		/// PerformExpand()が呼び出される直前にWikiEnginにより設定します。
		/// Scopeが存在しない場合、NULLを設定します。
		/// </summary>
		Scope CurrentRendereScope { set; }
	}
}
