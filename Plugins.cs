using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using WikiPlex.Plugins;

namespace WikiPlex
{
	public class WikiPlugins
	{
		private static readonly IDictionary<string, Type> loadedPlugins;
		private static readonly ReaderWriterLockSlim rendererLock; // ?? .NETのロック機構？

		static WikiPlugins()
		{
			loadedPlugins = new Dictionary<string, Type>();
			rendererLock = new ReaderWriterLockSlim();


			// デフォルトプラグインを登録
			Register<HelloPlugin>();
			Register<ContentsPlugin>();
			Register<FontPlugin>();
			Register<BlockquotePlugin>();
		}

		/// <summary>
		/// 登録済みのすべてのプラグインリストを取得します
		/// </summary>
		public static IEnumerable<IWikiPlugin> All
		{
			get
			{
				List<IWikiPlugin> instanceList = new List<IWikiPlugin>();
				foreach (var cz in loadedPlugins.Values)
				{
					var obj = (IWikiPlugin)Activator.CreateInstance(cz);
					instanceList.Add(obj);
				}

				return instanceList;
			}
		}

		public static void Register<TRenderer>()
		  where TRenderer : class, IWikiPlugin, new()
		{
			Register(new TRenderer());
		}

		public static void Register(IWikiPlugin renderer)
		{
			rendererLock.EnterWriteLock();
			try
			{
				loadedPlugins[renderer.Id] = renderer.GetType();
			}
			finally
			{
				rendererLock.ExitWriteLock();
			}
		}


	}
}
