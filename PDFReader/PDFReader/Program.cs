using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFReader
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write(PdfText(@"D:\DTemp\test.pdf"));
		}


		public static string PdfText(string path)
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
	}
}
