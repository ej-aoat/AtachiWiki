using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace WikiPlex
{
	class ArgumentParserUtil
	{
		public static string[] Parse(string input)
		{
			input = input + " "; // 末尾に強制的に空白を追加。

			List<string> arguments = new List<string>();

			Regex regex = new Regex(@"([^\s]+)\s+?");

			var match = regex.Match(input);

			string beforeBack = "";
			while (match.Success)
			{
				string matchText = match.Value;

				if (CountChar(matchText, '"') == 1)
				{
					if (beforeBack != "")
					{
						beforeBack += matchText;

						//一致した対象が見つかったときキャプチャした部分文字列を表示
						arguments.Add(beforeBack);

						beforeBack = "";
					}
					else
					{
						beforeBack += matchText;
					}
				}
				else if (beforeBack != "")
				{
					beforeBack += matchText;
				}
				else
				{
					//一致した対象が見つかったときキャプチャした部分文字列を表示
					arguments.Add(matchText);
				}

				//次に一致する対象を検索
				match = match.NextMatch();
			}

			return arguments.ToArray();
		}

		static int CountChar(string s, char c)
		{
			return s.Length - s.Replace(c.ToString(), "").Length;
		}
	}
}
