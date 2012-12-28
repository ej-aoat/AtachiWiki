using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using log4net;
using System.Text;

namespace WikiPlex.Formatting.Renderers
{
	class TableRenderer : WikiPlex.Formatting.Renderers.Renderer//, IBinWikiRenderer
	{
		ILog LOG = LogManager.GetLogger(typeof(TableRenderer));

		WikiEngine engine = null;
		bool isHeadingRow = false;
		bool isRow = false;

		int colspan = 0;

		protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
		{
			if (scopeName == ScopeName.MTableBegin)
				return "<table border=\"1\">";
			if (scopeName == ScopeName.MTableEnd)
			{
				if (isRow)
					return "</tr></table>";
				return "</table>";
			}
			if (scopeName == ScopeName.MTableCellStart)
			{
				string colspanText = "";
				string bgcolor = "";

				StringBuilder styleText = new StringBuilder();

				string[] args = input.Split('|');

				// セル引数のパラメータを解析
				foreach (var arg in args)
				{
					if (arg == "") continue;

					// ◇colspan
					if (arg.StartsWith(">"))
					{
						string REGEX_PATTERN = @"(>+)";
						Match m = Regex.Match(arg, REGEX_PATTERN);
						if (m.Success)
						{
							var spanText = m.Groups[1].Value;
							colspan = spanText.Length;
						}
					}
					// ◇セルの背景色設定
					else if (arg.StartsWith("BGCOLOR:"))
					{
						string REGEX_PATTERN = @"bgcolor\:(.+)";
						var arglow = arg.ToLower();
						Match m = Regex.Match(arglow, REGEX_PATTERN);
						if (m.Success)
						{
							var colorText = m.Groups[1].Value;
							bgcolor = "bgcolor=\"" + colorText + "\"";
						}
					}
					// ◇
					else if (arg.StartsWith("width:"))
					{
						string REGEX_PATTERN = @"width\:(.+)";
						var arglow = arg.ToLower();
						Match m = Regex.Match(arglow, REGEX_PATTERN);
						if (m.Success)
						{
							var widthText = m.Groups[1].Value;
							styleText.Append("width:" + widthText).Append("; ");
						}
					}
				}

				if (colspan != 0)
				{
					colspanText = "colspan=\"" + colspan + "\"";
					colspan = 0;
				}

				if (styleText.Length != 0)
					styleText.Insert(0, "style=\"").Append("\"");

				if (isHeadingRow)
				{
					return string.Format("<th {0} {1}>", colspanText + bgcolor, styleText.ToString());
				}
				else
				{
					return string.Format("<td {0} {1}>", colspanText + bgcolor, styleText.ToString());
				}
			}
			if (scopeName == ScopeName.MTableCellEnd)
			{
				if (isHeadingRow)
					return "</th>";
				return "</td>";
			}
			if (scopeName == ScopeName.MTableCell)
			{

				return input;
			}
			if (scopeName == ScopeName.MTableNewLine)
			{
				isHeadingRow = false;
				isRow = true;

				if (isRow)
					return "</tr><tr>";
				else
					return "<tr>";
			}
			if (scopeName == ScopeName.MTableNewHeaderLine)
			{
				isHeadingRow = true;
				isRow = true;

				if (isRow)
					return "</tr><tr>";
				else
					return "<tr>";
			}

			return null;
		}


		protected override ICollection<string> ScopeNames
		{
			get
			{
				return new[]
				{
					ScopeName.MTableBegin,
					ScopeName.MTableEnd,
					ScopeName.MTableCellStart,
					ScopeName.MTableCell,ScopeName.MTableCellEnd,
					ScopeName.MTableNewLine,ScopeName.MTableNewHeaderLine
				};
			}
		}

		#region IBinWikiRenderer メンバー

		public void PreRenderer()
		{
			isHeadingRow = false;
		}

		public void PostRenderer()
		{

		}

		public WikiPlex.IWikiEngine Engine
		{
			set { engine = value as WikiEngine; }
		}

		#endregion
	}
}