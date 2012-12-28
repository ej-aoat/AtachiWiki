using System.Collections.Generic;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
	class TableScopeAugmenter : IScopeAugmenter
	{
		public virtual IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
		{
			IList<Scope> augmentedScopes = new List<Scope>();
			bool isCellStart = false;
			bool isCell = false;

			foreach (var scope in capturedScopes)
			{
				string CurrentContent = content.Substring(scope.Index, scope.Length);

				if (scope.Name == ScopeName.MTableBegin)
				{
					augmentedScopes.Add(scope);
				}
				else if (scope.Name == ScopeName.MTableEnd)
				{
					if (isCellStart)
					{
						// TDタグの閉じタグを追加するためのレンダラーを呼び出すスコープ
						isCellStart = false;
						isCell = false;
						var cellEndScope = new Scope(ScopeName.MTableCellEnd, scope.Index, 1);
						augmentedScopes.Add(cellEndScope);
					}

					var tableEndScope = new Scope(ScopeName.MTableEnd, scope.Index + 1, 1);
					augmentedScopes.Add(tableEndScope);
				}
				else if (scope.Name == ScopeName.MTableCellStart)
				{
					if (isCellStart && isCell)
					{
						isCellStart = false;
						var cellEndScope = new Scope(ScopeName.MTableCellEnd, scope.Index - 1, 1);
						augmentedScopes.Add(cellEndScope);
					}

					isCellStart = true;
					isCell = false;
					augmentedScopes.Add(scope);
				}
				else if (scope.Name == ScopeName.MTableCell)
				{
					if (isCellStart && isCell)
					{
						isCellStart = false;
						var cellEndScope = new Scope(ScopeName.MTableCellEnd, scope.Index - 1, 1);
						augmentedScopes.Add(cellEndScope);
					}

					isCell = true;
					//augmentedScopes.Add(scope); // セル内のテキストはTextFormattingレンダラに処理させるので、スコープに追加しない。
				}
				else if (scope.Name == ScopeName.MTableNewLine)
				{
					if (isCellStart && isCell)
					{
						isCellStart = false;
						isCell = false;
						var cellEndScope = new Scope(ScopeName.MTableCellEnd, scope.Index - 1, 1);
						augmentedScopes.Add(cellEndScope);
					}

					augmentedScopes.Add(scope);
				}
				else if (scope.Name == ScopeName.MTableNewHeaderLine)
				{
					// 実装内容はMTableNewLineと同じはず。

					if (isCellStart && isCell)
					{
						isCellStart = false;
						isCell = false;
						var cellEndScope = new Scope(ScopeName.MTableCellEnd, scope.Index - 1, 1);
						augmentedScopes.Add(cellEndScope);
					}

					augmentedScopes.Add(scope);
				}
				else if (scope.Name == ScopeName.Remove)
				{
					augmentedScopes.Add(scope);
				}

			}

			return augmentedScopes;
		}
	}
}