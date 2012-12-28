using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Formatting;

namespace WikiPlex.Plugins
{
	/// <summary>
	/// BinWikiのプラグインマクロで呼び出すプラグイン
	/// </summary>
	public interface IWikiPlugin
	{
		/// <summary>
		/// レンダリング前に行う処理を実装するメソッドです。
		/// </summary>
		/// <remarks>
		/// 一度のレンダリングで使用する内部的な変数は、このメソッドでクリアしましょう。
		/// </remarks>
		/// <param name="engine">処理を行うWikiエンジン(WikiEngine)</param>
		void PreRenderer(WikiEngine engine);

		/// <summary>
		/// レンダリング後に行う処理を実装するメソッドです。
		/// </summary>
		/// <param name="engine">処理を行うWikiエンジン(WikiEngine)</param>
		/// <param name="renderedHtml">処理後のHTML</param>
		string PostRenderer(WikiEngine engine, Formatter formatter, string renderedHtml);

		/// <summary>
		/// プラグイン記述の開始時にレンダリングする文字列を返すメソッドです。
		/// </summary>
		/// <param name="arguments">引数</param>
		/// <returns></returns>
		string RendererBegin(Dictionary<string, string> arguments);

		/// <summary>
		/// プラグイン記述の終了時にレンダリングする文字列を返すメソッドです。
		/// </summary>
		/// <returns></returns>
		string RendererEnd();

		/// <summary>
		/// プラグイン本体がレンダリングする文章
		/// </summary>
		/// <remarks>
		/// IsBodyRendererがTrueを返す場合のみ呼び出します。
		/// </remarks>
		/// <param name="body"></param>
		/// <returns></returns>
		string Renderer(string body);

		/// <summary>
		/// シングルラインプラグインとしての呼び出しで本文をレンダリングする文章
		/// </summary>
		/// <remarks>
		/// IsBodyRendererがTrueを返す場合のみ呼び出します。
		/// シングルラインプラグインでもRendererBeginとRendererEndの呼び出しは行います。
		/// </remarks>
		/// <returns></returns>
		string SingleRenderer();

		/// <summary>
		/// 本文のレンダリングを行う場合、Trueを返します。
		/// </summary>
		/// <returns></returns>
		bool IsBodyRenderer { get; }

		/// <summary>
		/// プラグイン名
		/// 使用するプラグインはすべてユニークな「プラグイン名」を持つ必要があります。
		/// </summary>
		string Id { get; }
	}
}
