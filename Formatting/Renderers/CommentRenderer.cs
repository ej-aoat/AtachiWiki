using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using WikiPlex.Parsing;

namespace WikiPlex.Formatting.Renderers
{
	public class CommentRenderer : WikiPlex.Formatting.Renderers.Renderer, IWikiRenderer
	{
		static ILog LOG = LogManager.GetLogger(typeof(CommentRenderer));

		protected override ICollection<string> ScopeNames
		{
			get
			{
				return new[] 
				{ 
					ScopeName.Comment
				};
			}
		}

		public Scope CurrentRendereScope { set { ;} }

		protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
		{
			if (scopeName == ScopeName.Comment)
			{
				
			}

			return null;
		}


		#region IBinWikiRenderer メンバー
		WikiEngine engine = null;
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
