using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Linq;

namespace PDFReader
{
	class PDFReader
	{
		#region Fields

		private readonly string _Path;

		#endregion

		#region Constructors

		public PDFReader(string path)
		{
			_Path = path;
		}

		#endregion

		#region Methods

		public string Read()
		{
			string readData = null;

			for (var i = 0; i < NumberOfFilesInDirectory(_Path); i++)
			{
				readData += PdfText(FileNamesAndPath(_Path)[i]);
			}

			return readData;
		}

		private static string PdfText(string path)
		{
			PdfReader reader = new PdfReader(path);
			string text = string.Empty;
			for (int page = 1; page <= reader.NumberOfPages; page++)
			{
				text += PdfTextExtractor.GetTextFromPage(reader, page);
			}
			reader.Close();
			return text;
		}

		private static int NumberOfFilesInDirectory(string path)
		{
			return System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(path), "*", System.IO.SearchOption.TopDirectoryOnly).Length;
		}

		private static string[] FileNamesAndPath(string path)
		{
			return System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(path), "*").ToArray();
		}

		#endregion
	}
}