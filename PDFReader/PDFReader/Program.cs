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
	partial class Program
	{
		static void Main(string[] args)
		{
			var dateTimeAndNumber = new List<Tuple<List<DateTime>, List<double>, List<string>>>();
			var baseExtractorData = new List<Tuple<List<DateTime>, List<double>, List<string>>>();
			var searchFor = new List<string> { "Zeiss", "AMAZON" };
			var searchForPay = new List<string> { "AMAZON" };
            var rejectPattern = new List<Tuple<string, string>>
            {
                Tuple.Create( "Ueberweisung", "Kolanek" ),
                Tuple.Create( "Gutschrift", "Kolanek" ),
                Tuple.Create( "Ueberweisung", "MONEYOU" ),
                Tuple.Create( "Ueberweisung", "Extra-Konto" )
			};

			PDFReader reader = new PDFReader(@"D:\Git\PDF\PDFs\");
			string readString = reader.Read();
			Console.Write(readString);

			AccountPatternExtractor extractor = new AccountPatternExtractor(readString, searchForPay);
			dateTimeAndNumber = extractor.Extract();
			extractor.Print(dateTimeAndNumber);

			AccountPositiveExtractor positiveExtractor = new AccountPositiveExtractor(readString, rejectPattern);
			AccountExtractor baseExtractor = new AccountExtractor(readString, rejectPattern);

			baseExtractorData = baseExtractor.Extract();
			baseExtractor.Print(baseExtractorData);

            Console.WriteLine();

            AccountMonthlySum accountMonthlySumPositive = new AccountMonthlySum(baseExtractorData);
            accountMonthlySumPositive.Sum(SumIndex.Index0);
            accountMonthlySumPositive.PrintSum();

            Console.WriteLine();

            AccountMonthlySum accountMonthlySumNegative = new AccountMonthlySum(baseExtractorData);
            accountMonthlySumNegative.Sum(SumIndex.Index1);
            accountMonthlySumNegative.PrintSum();

            Console.WriteLine();

            var results = accountMonthlySumPositive.MonthlySums.Zip(accountMonthlySumNegative.MonthlySums, (f, s) => f + s).ToList();

            results.ForEach(Console.WriteLine);

            Console.WriteLine();

            AccountMonthlySum accountPay = new AccountMonthlySum(dateTimeAndNumber);
            accountPay.Sum(SumIndex.Index0);
            accountPay.PrintSum();

        }
    }
}
