using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WikiPlex.Repositories;
using WikiPlex.Common;

namespace WikiPlex.Formatting.Renderers
{
	/// <summary>
	/// WikiPlexのImageRendererを参照に作成しています。
	/// </summary>
	public class ImageRepositoryRenderer : WikiPlex.Formatting.Renderers.Renderer
	{
		private const string ImageNoLink = "<img src=\"file:///{2}\" {3}/>";
		private const string ImageNoLinkWithStyle = "<div style=\"clear:both;height:0;\">&nbsp;</div><img style=\"float:{0};{1}\" src=\"{2}\" {3}/>";

		readonly IImageRepository imageRepository;
		string lastErrorMessage = "";

		public ImageRepositoryRenderer(IImageRepository repository)
		{
			imageRepository = repository;
		}

		protected override ICollection<string> ScopeNames
		{
			get
			{
				return new[] { 
					ScopeName.ImageRepositoryNoAlogn,
					ScopeName.ImageRepositoryHashNoAlogn,
				};
			}
		}

		/// <summary>
		/// Gets the invalid macro error text.
		/// </summary>
		protected override string InvalidMacroError
		{
			get { return "Cannot resolve image repository macro, invalid number of parameters. " + lastErrorMessage; }
		}

		protected override string PerformExpand(string scopeName, string input, Func<string, string> htmlEncode, Func<string, string> attributeEncode)
		{
			FloatAlignment alignment = FloatAlignment.None;
			var renderMethod = GetRenderMethod(scopeName);

			return RenderException.ConvertAny(() => renderMethod(this, input, alignment, attributeEncode));
		}

		private static FloatAlignment GetAlignment(string scopeName)
		{
			switch (scopeName)
			{
				default:
					return FloatAlignment.None;
			}
		}


		private static Func<ImageRepositoryRenderer, string, FloatAlignment, Func<string, string>, string> GetRenderMethod(string scopeName)
		{
			switch (scopeName)
			{
				case ScopeName.ImageRepositoryNoAlogn:
					return RenderImageDataWithAltMacro;
				case ScopeName.ImageRepositoryHashNoAlogn:
					return RenderImageDataHashWithAltMacro;
			}

			return null;
		}

		private static string RenderImageDataWithAltMacro(ImageRepositoryRenderer macro, string input, FloatAlignment alignment, Func<string, string> encode)
		{
			string format = alignment == FloatAlignment.None ? ImageNoLink : ImageNoLinkWithStyle;
			ImagePart parts = Utility.ExtractImageParts(input, ImagePartExtras.None, false);

			var imageName = parts.ImageUrl.Replace("title:", "");
			string localFilePath = macro.imageRepository.GetRepositoryImagePath(imageName);
			if (localFilePath == "")
			{
				macro.lastErrorMessage = imageName + "が見つかりません。";
				throw new ApplicationException();
			}

			var msg = string.Format(format, alignment.GetStyle(), alignment.GetPadding(), localFilePath, parts.Dimensions);
			return msg;
		}

		private static string RenderImageDataHashWithAltMacro(ImageRepositoryRenderer macro, string input, FloatAlignment alignment, Func<string, string> encode)
		{
			string format = alignment == FloatAlignment.None ? ImageNoLink : ImageNoLinkWithStyle;
			ImagePart parts = Utility.ExtractImageParts(input, ImagePartExtras.None, false);

			var imageName = parts.ImageUrl.Replace("hash:", "");
			string localFilePath = macro.imageRepository.GetRepositoryImageFromHashPath(imageName);
			if (localFilePath == "")
			{
				macro.lastErrorMessage = imageName + "が見つかりません。";
				throw new ApplicationException();
			}

			var msg = string.Format(format, alignment.GetStyle(), alignment.GetPadding(), localFilePath, parts.Dimensions);
			return msg;
		}
	}
}
