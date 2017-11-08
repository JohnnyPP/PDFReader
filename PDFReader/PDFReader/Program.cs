using System;
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
			Tuple<List<DateTime>, List<double>> dateTimeAndNumber = new Tuple<List<DateTime>, List<double>>(null, null);
			string readString = PDFReader.PdfText(@"D:\DTemp\test2.pdf");
			Console.Write(readString);
			dateTimeAndNumber = DateAndNumber(ValidateDateInFoundPattern(FindPattern(readString, "AMAZON")));
		}


		private static List<string> FindPattern(string searchIn, string searchFor)
		{
			List<string> foundPatterns = new List<string>();
			using (StringReader reader = new StringReader(searchIn))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.Contains(searchFor))
					{
						Console.WriteLine(line);
						foundPatterns.Add(line);
					}
				}
			}

			return foundPatterns;
		}

		private static List<string> ValidateDateInFoundPattern(List<string> foundPatterns)
		{
			List<string> validatedPatterns = new List<string>();

			Console.WriteLine("\nValidating dates...");

			foreach (var foundPattern in foundPatterns)
			{
				var foundDate = FindDate(foundPattern);

				if (foundDate != null)
					validatedPatterns.Add(foundDate);
			}

			foreach (var validatedPattern in validatedPatterns)
			{
				Console.WriteLine(validatedPattern);
			}

			// validated patterns should be printed on the screen
			return validatedPatterns;
		}

		private static Tuple<List<DateTime>, List<double>> DateAndNumber(List<string> foundPatterns)
		{
			List<double> numers = new List<double>();
			List<DateTime> dateTimes = new List<DateTime>();

			foreach (var foundPattern in foundPatterns)
			{
				var foundNumber = FindNumber(foundPattern);

				if (foundNumber != null)
				{
					numers.Add(double.Parse(foundNumber, NumberStyles.Currency));
					dateTimes.Add(FindDateTime(foundPattern));
				}
			}

			return new Tuple<List<DateTime>, List<double>>(dateTimes, numers);
		}

		private static string FindDate(string foundPattern)
		{
			var regex = new Regex(@"\b\d{2}\.\d{2}.\d{4}\b");
			foreach (Match m in regex.Matches(foundPattern))
			{
				DateTime dt;
				if (DateTime.TryParseExact(m.Value, "dd.MM.yyyy", null, DateTimeStyles.None, out dt))
					return foundPattern;
			}

			return null;
		}

		private static DateTime FindDateTime(string foundPattern)
		{
			var index = foundPattern.IndexOf(' ');
			var date = foundPattern.Substring(0, index);
			return DateTime.Parse(date);
		}

		private static string FindNumber(string foundPattern)
		{
			var index = foundPattern.LastIndexOf(' ');
			var number = foundPattern.Substring(index + 1);

			return number.Contains(',') ? number : null;
		}
	}
}
