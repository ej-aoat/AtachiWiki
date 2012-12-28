using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex;

namespace AtachiWiki.ConsoleTest
{
	class Program
	{

		static void Main(string[] args)
		{
			string strAppPath;

			// アプリケーションの実行パスを取得する
			strAppPath = System.AppDomain.CurrentDomain.BaseDirectory;
			Console.WriteLine("Application Current Directory:" + strAppPath);

			//内容を読み込む
			string text = System.IO.File.ReadAllText(strAppPath + @"\Sample.txt");
			Console.WriteLine("読み込んだテキスト:" + text);

			var engine = new WikiEngine();
			string rs = engine.Render(text);

			Console.WriteLine(text.Substring(1, 18));
			Console.WriteLine(rs);

			Console.ReadLine();
		}
	}
}
