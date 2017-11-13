using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PDFReaderUnitTest
{
	[TestClass]
	public class PDFReaderUnitTest
	{
		[TestMethod]
		public void PDFReaderUnitTestMethod()
		{
			var reader = new PDFReader.PDFReader(@"D:\Git\PDF\PDFs\");
			var readString = reader.Read();

			Assert.AreNotEqual(readString, null, "Reader could not read PDF files.");
			Assert.AreEqual(readString.Length, 106271, "Wrong length of the read PDF files. Are you using all PDFs from the year 2015?");
			Assert.AreEqual(readString.EndsWith("ING-DiBa AG"), true, "Last PDF ends with unexpected string.");
		}
	}
}
