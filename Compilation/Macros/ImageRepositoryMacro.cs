using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiPlex.Compilation.Macros
{
	/// <summary>
	/// リポジトリから画像を取得し、その画像をBase64形式で出力する
	/// </summary>
	public class ImageRepositoryMacro : IMacro
	{
		// WikiPlexが実装しているImageを真似していますが、アレほど自由度を追求していません。

		/// <summary>
		/// Gets the id of the macro.
		/// </summary>
		public string Id
		{
			get { return "ImageRepository"; }
		}

		/// <summary>
		/// Gets the list of rules for the macro.
		/// </summary>
		public IList<MacroRule> Rules
		{
			get
			{
				return new List<MacroRule>()
						   {
							   new MacroRule(
								   @"(?i)(\[imageref:)((?>title:[^\]\|]*))(\])",
								   new Dictionary<int, string>
									   {
										   {1, ScopeName.Remove},
										   {2, ScopeName.ImageRepositoryNoAlogn},
										   {3, ScopeName.Remove}
									   }
								   ),
								new MacroRule(
								   @"(?i)(\[imageref:)((?>hash:[^\]\|]*))(\])",
								   new Dictionary<int, string>
									   {
										   {1, ScopeName.Remove},
										   {2, ScopeName.ImageRepositoryHashNoAlogn},
										   {3, ScopeName.Remove}
									   }
								   ),
						   };
			}
		}
	}
}
