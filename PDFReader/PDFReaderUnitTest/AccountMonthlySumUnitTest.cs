using System;
using System.Collections.Generic;
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

			extractedDatasSum.Sum(SumType.Positive);

			for (var i = 0; i < _ExpectedValuesNegative.Count; i++)
			{
				Assert.AreEqual(_ExpectedValuesPositive[i], extractedDatasSum.MonthlySums[i], 0.000001, "Wrong positive sum list");
			}

			extractedDatasSum.PrintSum();
		}

		[TestMethod]
		public void AccountMonthlySumNegative1Item()
		{
			var searchFor = new List<string> { "AMAZON" };
			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			var extractedDatasSum = new AccountMonthlySum(extractedData);

			extractedDatasSum.Sum(SumType.Positive);

			for (var i = 0; i < _ExpectedValuesNegative.Count; i++)
			{
				Assert.AreEqual(_ExpectedValuesNegative[i], extractedDatasSum.MonthlySums[i], 0.000001, "Wrong negative sum list");
			}

			extractedDatasSum.PrintSum();
		}
	}
}
