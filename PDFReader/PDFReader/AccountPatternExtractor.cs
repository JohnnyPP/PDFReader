using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDFReader
{
	/// <summary>
	/// This class extracts data that matches the pattern
	/// </summary>
	class AccountPatternExtractor
	{
		private static string _SearchIn;
		private static string _SearchFor;

		public AccountPatternExtractor(string searchIn, string searchFor)
		{
			_SearchFor = searchFor;
			_SearchIn = searchIn;
		}

		public Tuple<List<DateTime>, List<double>> Extract()
		{
			return DateAndAccount(ValidateDate(FindPattern(_SearchIn, _SearchFor)));
		}

		private static List<string> FindPattern(string searchIn, string searchFor)
		{
			var foundPatterns = new List<string>();
			using (StringReader reader = new StringReader(searchIn))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					if (line.Contains(searchFor))
					{
						foundPatterns.Add(line);
					}
				}
			}

			return foundPatterns;
		}

		private static List<string> ValidateDate(List<string> foundPatterns)
		{
			var validatedPatterns = new List<string>();

			foreach (var foundPattern in foundPatterns)
			{
				var foundDate = FindDate(foundPattern);

				if (foundDate != null)
					validatedPatterns.Add(foundDate);
			}

			// validated patterns should be printed on the screen
			return validatedPatterns;
		}

		private static Tuple<List<DateTime>, List<double>> DateAndAccount(List<string> foundPatterns)
		{
			var numers = new List<double>();
			var dateTimes = new List<DateTime>();

			foreach (var foundPattern in foundPatterns)
			{
				var foundNumber = FindNumber(foundPattern);

				if (foundNumber == null)
					continue;

				numers.Add(double.Parse(foundNumber, NumberStyles.Currency));
				dateTimes.Add(FindDateTime(foundPattern));
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