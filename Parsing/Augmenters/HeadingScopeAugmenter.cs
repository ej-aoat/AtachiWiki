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
					Console.WriteLine("[S1]Pos = " + scope.Index);
				}
				if (scope.Name == ScopeName.HeadingOneEnd)
				{
					Console.WriteLine("[E1]Pos = " + scope.Index);
				}
				if (scope.Name == ScopeName.HeadingTwoBegin)
				{
					Console.WriteLine("[S2]Pos = " + scope.Index);
				}
				if (scope.Name == ScopeName.HeadingTwoEnd)
				{
					Console.WriteLine("[E2]Pos = " + scope.Index);
				}
				augmentedScopes.Add(scope);

			}

			return augmentedScopes;
		}
	}
}
