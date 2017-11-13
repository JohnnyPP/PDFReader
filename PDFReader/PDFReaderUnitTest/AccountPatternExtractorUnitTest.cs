using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDFReader;

namespace PDFReaderUnitTest
{
	/// <summary>
	/// Summary description for AccountPatternExtractorUnitTest
	/// </summary>
	[TestClass]
	public class AccountPatternExtractorUnitTest
	{
		public AccountPatternExtractorUnitTest()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void AccountPatternExtractorUnitTestMethod()
		{
			var searchFor1 = new List<string> { "AMAZON" };
			var searchFor2 = new List<string> { "Zeiss", "Gutschrift" };


			var reader = new PDFReader.PDFReader(@"D:\Git\PDF\PDFs\");
			var readString = reader.Read();

			var extractor1 = new AccountPatternExtractor(readString, searchFor1);
			var extractedData1 = extractor1.Extract();
			Assert.AreEqual(extractedData1[0].Item1.Count, 45, "Unexpected number of extracted strings.");
			extractor1.Print(extractedData1);

			var extractor2 = new AccountPatternExtractor(readString, searchFor2);
			var extractedData2 = extractor2.Extract();
			Assert.AreEqual(extractedData2[0].Item1.Count, 13, "Unexpected number of extracted strings.");
			Assert.AreEqual(extractedData2[1].Item1.Count, 44, "Unexpected number of extracted strings.");
			extractor2.Print(extractedData2);
		}
	}
}
