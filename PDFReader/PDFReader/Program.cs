﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace PDFReader
{
	class Program
	{
		//Look for patterns in string of the form
		//26.10.2017 Some string with spaces 4.145,26
		//17.10.2017 Some other string with spaces -15,24
		//DATE TXT NUMBER (with, . and possible - in the front)
		//Algorithm:
		//1. Read pdf into a string
		//2. This should be displayed on the screen. The user should define search pattern TXT or pattern should be read from the txt file
		//3. Search for the TXT
		//4. If 3. found validate
		//5. Does the line begins with valid DATE is at the beginning
		//6. If 5. is true search if NUMBER is located at the end
		//7. If 6 true store the NUMBER and store the DATE for given TXT search pattern

		static void Main(string[] args)
		{
			var dateTimeAndNumber = new List<Tuple<List<DateTime>, List<double>, List<string>>>();
			var searchFor = new List<string> { "Zeiss", "AMAZON" };

			PDFReader reader = new PDFReader(@"D:\Git\PDF\PDFs\test2.pdf");
			string readString = reader.Read();
			Console.Write(readString);

			AccountPatternExtractor extractor = new AccountPatternExtractor(readString, searchFor);
			dateTimeAndNumber = extractor.Extract();
			extractor.Print(dateTimeAndNumber);

			//AccountPositiveExtractor.Sum() - AccountNegativeExtractor.Sum() = Amount saved in the month -> this should be displayed as bar plot
		}
	}
}
