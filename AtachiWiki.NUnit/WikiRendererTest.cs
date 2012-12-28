using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.IO;
using log4net;
using WikiPlex;

namespace AtachiWiki.NUnit
{
	[SetUpFixture]
	public class Initialize
	{
		[SetUp]
		public void Init()
		{
			// Log4Netの設定ファイルを読み込む
			// 設定フェイルはNUnitのプロジェクトと同じ場所に「Log4Net.config」という名前で配置すること。
			string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
			FileInfo finfo = new FileInfo(logFilePath);
			log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);

		}
	}


	[TestFixture]
	public class WikiRendererTest
	{
		static ILog LOG = LogManager.GetLogger(typeof(WikiRendererTest));

		[Test]
		public void CommentRendererTest()
		{
			LOG.Info("Start CommentRendererTest");

			//内容を読み込む
			string s = "line1\n// ここはコメント\nline2";

			
			var engine = new WikiEngine();
			string rs = engine.Render(s, WikiEngine.GetRenderers());

			Assert.AreEqual("line1<br />line2", rs);
		}
	}
}
