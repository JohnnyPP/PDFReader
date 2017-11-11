using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDFReader
{
	class AccountBaseExtractor
	{
		#region Fields

		private readonly string _SearchIn;
		private readonly List<Tuple<string, string>> _SearchFor;

        #endregion

        #region Constructors

        public AccountBaseExtractor(string searchIn, List<Tuple<string, string>> searchFor)
		{
			_SearchFor = searchFor;
			_SearchIn = searchIn;
		}

		#endregion

		#region Methods

		public List<Tuple<List<DateTime>, List<double>, List<string>>> Extract()
		{
			return DateAndAccount(RejectPattern(ValidateAmount(ValidateDate(StringToList(_SearchIn))), _SearchFor));
		}

		public void Print(List<Tuple<List<DateTime>, List<double>, List<string>>> dataToPrint)
		{
			Console.WriteLine(Environment.NewLine);

			foreach (var tuple in dataToPrint)
			{
				for (var i = 0; i < tuple.Item1.Count; i++)
				{
					Console.WriteLine(tuple.Item1[i]);
					Console.WriteLine(tuple.Item2[i]);
					Console.WriteLine(tuple.Item3[i]);
				}
			}
        }

		private static List<Tuple<List<DateTime>, List<double>, List<string>>> DateAndAccount(List<string> foundPatterns)
		{
			var patternPositive = new List<string>();
			var accountPositive = new List<double>();
			var dateTimesPositive = new List<DateTime>();
			var patternNegative = new List<string>();
			var accountNegative = new List<double>();
			var dateTimesNegative = new List<DateTime>();
			var result = new List<Tuple<List<DateTime>, List<double>, List<string>>>();
            var cultureInfo = new CultureInfo("de-DE");
            double foundAmount;

            foreach (var foundPattern in foundPatterns)
			{
				var foundNumber = FindAccount(foundPattern);

				if (foundNumber == null)
					continue;

                if (double.TryParse(foundNumber, NumberStyles.Currency, cultureInfo, out foundAmount))
                {
                    if (foundAmount > 0.0)
                    {
                        accountPositive.Add(foundAmount);
                        dateTimesPositive.Add(FindDateTime(foundPattern));
                        patternPositive.Add(foundPattern);
                    }
                    else
                    {
                        accountNegative.Add(foundAmount);
                        dateTimesNegative.Add(FindDateTime(foundPattern));
                        patternNegative.Add(foundPattern);
                    }
                }
			}

			result.Add(new Tuple<List<DateTime>, List<double>, List<string>>(dateTimesPositive, accountPositive, patternPositive));
			result.Add(new Tuple<List<DateTime>, List<double>, List<string>>(dateTimesNegative, accountNegative, patternNegative));

			return result;
		}


		private static int GetSign(double numberToCheck)
		{
			return Math.Sign(numberToCheck);
		}

        

        /// <summary>
        /// Rejects the search pattern that is internal or external transfers between accounts
        /// </summary>
        /// <param name="rejectIn"></param>
        /// <param name="searchFor"></param>
        /// <returns></returns>
        private static List<string> RejectPattern(List<string> rejectIn, List<Tuple<string, string>> searchFor)
		{
			var indexToReject  = new List<int>();

			for (var index = 0; index < rejectIn.Count; index++)
			{
				var line = rejectIn[index];
				foreach (var tuple in searchFor)
				{
					if (!line.Contains(tuple.Item1) || !line.Contains(tuple.Item2))
					{
						continue;
					}
					indexToReject.Add(index);
				}
			}

			foreach (var indice in indexToReject.OrderByDescending(v => v))
			{
				rejectIn.RemoveAt(indice);
			}

			return rejectIn;
		}

		private List<string> StringToList(string input)
		{
			var output = new List<string>();
			using (StringReader reader = new StringReader(input))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					output.Add(line);
				}
			}

			return output;
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

			return validatedPatterns;
		}

		private static List<string> ValidateAmount(List<string> toValidate)
		{
			var validatedAmmount = new List<string>();

			foreach (var ammount in toValidate)
			{
				if(FindAmount(ammount))
					validatedAmmount.Add(ammount);
			}

			return validatedAmmount;
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

		private static bool FindAmount(string toFind)
		{
			var index = toFind.LastIndexOf(' ');
			var amount = toFind.Substring(index + 1);

			return amount.Contains(',');
		}

		private static string FindAccount(string foundPattern)
		{
			var index = foundPattern.LastIndexOf(' ');
			var account = foundPattern.Substring(index + 1);

			return account.Contains(',') ? account : null;
		}

        #endregion
    }
}
