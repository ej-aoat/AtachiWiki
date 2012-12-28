using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WikiPlex.Repositories
{
	public interface IImageRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="repositoryItemName"></param>
		/// <returns></returns>
		string GetRepositoryImagePath(string repositoryItemName);

		/// <summary>
		/// キーから画像のURLを取得します
		/// </summary>
		/// <param name="imageHash"></param>
		/// <returns></returns>
		string GetRepositoryImageFromHashPath(string imageHash);
	}
}
