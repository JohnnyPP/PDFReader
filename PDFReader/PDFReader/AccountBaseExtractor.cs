using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	abstract class AccountBaseExtractor
	{
		public AccountBaseExtractor()
		{
			
		}

		/// <summary>
		/// Rejects the search pattern that is internal or external transfers between accounts
		/// </summary>
		/// <param name="searchIn"></param>
		/// <param name="searchFor"></param>
		/// <returns></returns>
		public static List<string> RejectPattern(string searchIn, string searchFor)
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
	}
}
