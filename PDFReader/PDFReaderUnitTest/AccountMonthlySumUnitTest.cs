using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDFReader;

namespace PDFReaderUnitTest
{
	[TestClass]
	public class AccountMonthlySumUnitTest
	{
		private static string _ReadString;
		private readonly List<double> _ExpectedValuesPositive = new List<double>() { 2973.17, 3014.77, 2928.93, 3107.53, 3924.05, 3315.69, 2987.53, 2967.53, 2987.53, 3396.3, 5307.55, 3124.79 };
		private readonly List<double> _ExpectedValuesNegative = new List<double>() { -109.17, -146.82, -240.19, -709.89, -411.74, -54.99, -118.07, -547.36 };
		private readonly List<double> _MonthlySumPositive = new List<double>() { 3583.2, 5701.04, 5074.23, 3465.41, 4207.56, 3527.93, 3103.58, 3240.25, 3094.32, 3556.72, 5620.05, 3282.7 };
		private readonly List<double> _MontlySumNegative = new List<double>() { -921.15, -6735.63, -1565.75, -1782.59, -896.14, -1820.61, -1133.66, -1036.5, -3236.42, -1578.9, -2272.33, -1738.61 };
		private readonly List<double> _MontlyPositiveNegativeSum = new List<double>() { 2662.05, -1034.59, 3508.48, 1682.82, 3311.42, 1707.32, 1969.92, 2203.75, -142.1, 1977.82, 3347.72, 1544.09 };

		readonly List<Tuple<string, string>> _RejectPattern = new List<Tuple<string, string>>
		{
			Tuple.Create( "Ueberweisung", "Kolanek" ),
			Tuple.Create( "Gutschrift", "Kolanek" ),
			Tuple.Create( "Ueberweisung", "MONEYOU" ),
			Tuple.Create( "Ueberweisung", "Extra-Konto" )
		};

		public AccountMonthlySumUnitTest()
		{
			var reader = new PDFReader.PDFReader(@"D:\Git\PDF\PDFs\");
			_ReadString = reader.Read();
		}

		[TestMethod]
		public void AccountMonthlySumPositive1Item()
		{
			var searchFor = new List<string> { "Zeiss" };
			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			var extractedDatasSum = new AccountMonthlySum(extractedData);

			extractedDatasSum.Sum(SumIndex.Index0);
			AssertDoubleLists(_ExpectedValuesPositive, extractedDatasSum.MonthlySums);
			extractedDatasSum.PrintSum();
		}

		[TestMethod]
		public void AccountMonthlySumNegative1Item()
		{
			var searchFor = new List<string> { "AMAZON" };
			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			var extractedDatasSum = new AccountMonthlySum(extractedData);

			extractedDatasSum.Sum(SumIndex.Index0);
			AssertDoubleLists(_ExpectedValuesNegative, extractedDatasSum.MonthlySums);
			extractedDatasSum.PrintSum();
		}

		[TestMethod]
		public void AccountMonthlyPositiveNegativeSum()
		{
			var baseExtractor = new AccountExtractor(_ReadString, _RejectPattern);
			var baseExtractorData = baseExtractor.Extract();

			var accountMonthlySumPositive = new AccountMonthlySum(baseExtractorData);
			accountMonthlySumPositive.Sum(SumIndex.Index0);
			AssertDoubleLists(_MonthlySumPositive, accountMonthlySumPositive.MonthlySums);
			Console.WriteLine("Positive monthly sums");
			accountMonthlySumPositive.PrintSum();

			Console.WriteLine();

			var accountMonthlySumNegative = new AccountMonthlySum(baseExtractorData);
			accountMonthlySumNegative.Sum(SumIndex.Index1);
			AssertDoubleLists(_MontlySumNegative, accountMonthlySumNegative.MonthlySums);
			Console.WriteLine("Negative monthly sums");
			accountMonthlySumNegative.PrintSum();

			Console.WriteLine();

			var results = accountMonthlySumPositive.MonthlySums.Zip(accountMonthlySumNegative.MonthlySums, (f, s) => f + s).ToList();
			AssertDoubleLists(_MontlyPositiveNegativeSum, results);
			Console.WriteLine("Positive + negative monthly sums");
			results.ForEach(Console.WriteLine);
		}


		private void AssertDoubleLists(List<double> expected, List<double> actual)
		{
			for (var i = 0; i < expected.Count; i++)
			{
				Assert.AreEqual(expected[i], actual[i], 0.000001, "Wrong date in the list.");
			}
		}
	}
}
