using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
