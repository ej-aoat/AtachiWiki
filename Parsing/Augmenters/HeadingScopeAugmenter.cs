using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Compilation.Macros;

namespace WikiPlex.Parsing
{
	class HeadingScopeAugmenter : IScopeAugmenter
	{
		public virtual IList<Scope> Augment(IMacro macro, IList<Scope> capturedScopes, string content)
		{
			IList<Scope> augmentedScopes = new List<Scope>();

			foreach (var scope in capturedScopes)
			{
				string CurrentContent = content.Substring(scope.Index, scope.Length);

				if (scope.Name == ScopeName.HeadingOneBegin)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingOneEnd)
				{
					scope.Data =  scope.Index;
				}
				if (scope.Name == ScopeName.HeadingTwoBegin)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingTwoEnd)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingThreeEnd)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingFourEnd)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingFiveEnd)
				{
					scope.Data = scope.Index;
				}
				if (scope.Name == ScopeName.HeadingSixEnd)
				{
					scope.Data = scope.Index;
				}

				augmentedScopes.Add(scope);

			}

			return augmentedScopes;
		}
	}
}
