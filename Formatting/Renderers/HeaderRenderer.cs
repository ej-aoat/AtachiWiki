using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WikiPlex.Formatting.Renderers
{
	class HeaderRenderer : Renderer, IWikiRenderer
	{
		IList<HeaderItem> topLevelHeaderList = new List<HeaderItem>();

		public IList<HeaderItem> TopLevelItems
		{
			get { return topLevelHeaderList; }
		}

		protected override ICollection<string> ScopeNames
		{
			get
			{
				return new[] {
					ScopeName.HeadingOneBegin,ScopeName.HeadingOneBody,ScopeName.HeadingOneEnd, 
					ScopeName.HeadingTwoBegin,ScopeName.HeadingTwoBody, ScopeName.HeadingTwoEnd,
					ScopeName.HeadingThreeBegin,ScopeName.HeadingThreeBody, ScopeName.HeadingThreeEnd,
					ScopeName.HeadingFourBegin,ScopeName.HeadingFourBody,ScopeName.HeadingFourEnd, 
					ScopeName.HeadingFiveBegin, ScopeName.HeadingFiveBody,ScopeName.HeadingFiveEnd,
					ScopeName.HeadingSixBegin, ScopeName.HeadingSixBody,ScopeName.HeadingSixEnd
				};
			}
		}

		static void HeaderLevel1(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			stack.Clear(); // 無条件でスタックはクリアする

			var HeaderLevel1Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			stack.Push(HeaderLevel1Item);
			topLevelHeaderItemList.Add(HeaderLevel1Item);
		}

		static void HeaderLevel2(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			if (stack.Count != 0)
			{
				while (stack.Count != 1)
					stack.Pop();
			}
			else
			{
				var ignoreHeaderLevel1 = new HeaderItem("", "", DrawRenderer.IGNORE);
				stack.Push(ignoreHeaderLevel1); // IgnoreなLevel1ヘッダーをスタックに詰む
				topLevelHeaderItemList.Add(ignoreHeaderLevel1);
			}

			var h1item = stack.Peek();
			var HeaderLevel2Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			h1item.Children.Add(HeaderLevel2Item);
			stack.Push(HeaderLevel2Item);
		}

		static void HeaderLevel3(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			if (stack.Count >= 2)
			{
				while (stack.Count != 2)
					stack.Pop();
			}
			else
			{
				if (stack.Count == 0)
				{
					var ignoreHeaderLevel1 = new HeaderItem("", "", DrawRenderer.IGNORE);
					stack.Push(ignoreHeaderLevel1); // IgnoreなLevel1ヘッダーをスタックに詰む
					topLevelHeaderItemList.Add(ignoreHeaderLevel1);

					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel1.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む
				}
				else if (stack.Count == 1)
				{
					var h1item = stack.Peek();
					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					h1item.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む
				}
			}

			var h2item = stack.Peek();
			var HeaderLevel3Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			h2item.Children.Add(HeaderLevel3Item);
			stack.Push(HeaderLevel3Item);
		}

		static void HeaderLevel4(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			if (stack.Count >= 3)
			{
				while (stack.Count != 3)
					stack.Pop();
			}
			else
			{
				if (stack.Count == 0)
				{
					var ignoreHeaderLevel1 = new HeaderItem("", "", DrawRenderer.IGNORE);
					stack.Push(ignoreHeaderLevel1); // IgnoreなLevel1ヘッダーをスタックに詰む
					topLevelHeaderItemList.Add(ignoreHeaderLevel1);

					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel1.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む
				}
				else if (stack.Count == 1)
				{
					var h1item = stack.Peek();
					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					h1item.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む
				}
				else if (stack.Count == 2)
				{
					var h2item = stack.Peek();

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(h2item);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む
				}
			}

			var h3item = stack.Peek();
			var HeaderLevel4Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			h3item.Children.Add(HeaderLevel4Item);
			stack.Push(HeaderLevel4Item);
		}

		static void HeaderLevel5(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			if (stack.Count >= 4)
			{
				while (stack.Count != 4)
					stack.Pop();
			}
			else
			{
				if (stack.Count == 0)
				{
					var ignoreHeaderLevel1 = new HeaderItem("", "", DrawRenderer.IGNORE);
					stack.Push(ignoreHeaderLevel1); // IgnoreなLevel1ヘッダーをスタックに詰む
					topLevelHeaderItemList.Add(ignoreHeaderLevel1);

					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel1.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel3ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む
				}
				else if (stack.Count == 1)
				{
					var h1item = stack.Peek();
					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					h1item.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む
				}
				else if (stack.Count == 2)
				{
					var h2item = stack.Peek();

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(h2item);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む
				}
				else if (stack.Count == 3)
				{
					var h3item = stack.Peek();

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(h3item);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む
				}
			}

			var h4item = stack.Peek();
			var HeaderLevel5Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			h4item.Children.Add(HeaderLevel5Item);
			stack.Push(HeaderLevel5Item);
		}

		static void HeaderLevel6(Stack<HeaderItem> stack, string headerText, string ancher, IList<HeaderItem> topLevelHeaderItemList)
		{
			if (stack.Count >= 5)
			{
				while (stack.Count != 5)
					stack.Pop();
			}
			else
			{
				if (stack.Count == 0)
				{
					var ignoreHeaderLevel1 = new HeaderItem("", "", DrawRenderer.IGNORE);
					stack.Push(ignoreHeaderLevel1); // IgnoreなLevel1ヘッダーをスタックに詰む
					topLevelHeaderItemList.Add(ignoreHeaderLevel1);

					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel1.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel3ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む

					var ignoreHeaderLevel5 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel5.Children.Add(ignoreHeaderLevel4);
					stack.Push(ignoreHeaderLevel5); // IgnoreなLevel5ヘッダーをスタックに詰む
				}
				else if (stack.Count == 1)
				{
					var h1item = stack.Peek();
					var ignoreHeaderLevel2 = new HeaderItem("", "", DrawRenderer.IGNORE);
					h1item.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel2); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(ignoreHeaderLevel2);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む

					var ignoreHeaderLevel5 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel5.Children.Add(ignoreHeaderLevel4);
					stack.Push(ignoreHeaderLevel5); // IgnoreなLevel5ヘッダーをスタックに詰む
				}
				else if (stack.Count == 2)
				{
					var h2item = stack.Peek();

					var ignoreHeaderLevel3 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel3.Children.Add(h2item);
					stack.Push(ignoreHeaderLevel3); // IgnoreなLevel2ヘッダーをスタックに詰む

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(ignoreHeaderLevel3);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む

					var ignoreHeaderLevel5 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel5.Children.Add(ignoreHeaderLevel4);
					stack.Push(ignoreHeaderLevel5); // IgnoreなLevel5ヘッダーをスタックに詰む
				}
				else if (stack.Count == 3)
				{
					var h3item = stack.Peek();

					var ignoreHeaderLevel4 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel4.Children.Add(h3item);
					stack.Push(ignoreHeaderLevel4); // IgnoreなLevel4ヘッダーをスタックに詰む

					var ignoreHeaderLevel5 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel5.Children.Add(ignoreHeaderLevel4);
					stack.Push(ignoreHeaderLevel5); // IgnoreなLevel5ヘッダーをスタックに詰む
				}
				else if (stack.Count == 4)
				{
					var h4item = stack.Peek();

					var ignoreHeaderLevel5 = new HeaderItem("", "", DrawRenderer.IGNORE);
					ignoreHeaderLevel5.Children.Add(h4item);
					stack.Push(ignoreHeaderLevel5); // IgnoreなLevel5ヘッダーをスタックに詰む
				}
			}

			var h5item = stack.Peek();
			var HeaderLevel6Item = new HeaderItem(ancher, headerText, DrawRenderer.SHOW);
			h5item.Children.Add(HeaderLevel6Item);
			stack.Push(HeaderLevel6Item);
		}

		Stack<HeaderItem> headerItemStack = new Stack<HeaderItem>();


		protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
		{
			string ancher = "";
			string escText;
			HeaderItem headerItem;

			switch (scopeName)
			{
				case ScopeName.HeadingOneBegin:
					ancher = RandomeAncherName();
					HeaderLevel1(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h1 id=\"{0}\">", ancher);
				case ScopeName.HeadingOneBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingOneEnd:
					//headerItemStack.Pop();
					return "</h1>\r";
				case ScopeName.HeadingTwoBegin:
					ancher = RandomeAncherName();
					HeaderLevel2(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h2 id=\"{0}\">", ancher);
				case ScopeName.HeadingTwoBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingTwoEnd:
					//headerItem = headerItemStack.Pop();
					return "</h2>\r";
				case ScopeName.HeadingThreeBegin:
					ancher = RandomeAncherName();
					HeaderLevel3(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h3 id=\"{0}\">", ancher);
				case ScopeName.HeadingThreeBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingThreeEnd:
					//headerItemStack.Pop();
					return "</h3>\r";
				case ScopeName.HeadingFourBegin:
					ancher = RandomeAncherName();
					HeaderLevel4(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h4 id=\"{0}\">", ancher);
				case ScopeName.HeadingFourBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingFourEnd:
					//headerItemStack.Pop();
					return "</h4>\r";
				case ScopeName.HeadingFiveBegin:
					ancher = RandomeAncherName();
					HeaderLevel5(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h5 id=\"{0}\">", ancher);
				case ScopeName.HeadingFiveBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingFiveEnd:
					//headerItemStack.Pop();
					return "</h5>\r";
				case ScopeName.HeadingSixBegin:
					ancher = RandomeAncherName();
					HeaderLevel6(headerItemStack, "", ancher, topLevelHeaderList);
					return string.Format("<h6 id=\"{0}\">", ancher);
				case ScopeName.HeadingSixBody:
					escText = htmlEncode(input);
					headerItem = headerItemStack.Peek();
					headerItem.Title = escText;
					return escText;
				case ScopeName.HeadingSixEnd:
					//headerItemStack.Pop();
					return "</h6>\r";
				default:
					return null;
			}
		}


		/// <summary>
		/// ランダムなテキストを作成します
		/// </summary>
		/// <returns></returns>
		private string RandomeAncherName()
		{
			return "hd" + Path.GetRandomFileName().Replace(".", ""); // XXXXX.XXXのようなランダムな文字列ができあがるので、「.」を消去します。
		}

		#region IBinWikiRenderer メンバー

		public void PreRenderer()
		{

		}

		public void PostRenderer()
		{

		}

		public IWikiEngine Engine
		{
			set { ;}
		}
		#endregion
	}

	class HeaderItem
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="title">後からでも設定可能ですが・・・実装ミスでコンストラクタ必須としてしまいました。</param>
		/// <param name="ancher"></param>
		/// <param name="renderer"></param>
		public HeaderItem(string ancher, string title, DrawRenderer renderer)
		{
			this.Children = new List<HeaderItem>();
			this.Title = title;
			this.AncherName = ancher;
			this.Renderer = renderer;
		}

		public string Title
		{
			get;
			set;
		}

		public string AncherName
		{
			get;
			private set;
		}

		public IList<HeaderItem> Children
		{
			get;
			private set;
		}

		public DrawRenderer Renderer
		{
			get;
			private set;
		}

		public string ToHtml()
		{
			StringBuilder sb = new StringBuilder();

			if (Renderer == DrawRenderer.SHOW)
			{
				sb.Append("<li>");
				sb.AppendFormat("<a href=\"#{0}\">", AncherName);
				sb.Append(Title);
				sb.Append("</a>");
			}

			foreach (var child in Children)
			{
				sb.Append("<ul>");
				sb.Append(child.ToHtml());
				sb.Append("</ul>");
			}

			if (Renderer == DrawRenderer.SHOW)
				sb.Append("</li>");

			return sb.ToString();
		}
	}

	enum DrawRenderer
	{
		SHOW,
		HIDDEN,
		COLLED,
		IGNORE
	}
}
