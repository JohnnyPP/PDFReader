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
			var reader = new PDFReader.PDFReader(@"D:\Git\PDF\PDFs\");
			_ReadString = reader.Read();
		}

		private TestContext testContextInstance;
		private static string _ReadString;

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
		public void AccountPatternExtractorSearchFor1Item()
		{

			var searchFor = new List<string> { "AMAZON" };

			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			Assert.AreEqual(extractedData[0].Item1.Count, 45, "Unexpected number of extracted matches.");
			extractor.Print(extractedData);
		}


		[TestMethod]
		public void AccountPatternExtractorSearchFor2Items()
		{
			var searchFor = new List<string> { "Zeiss", "Gutschrift" };

			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			Assert.AreEqual(extractedData[0].Item1.Count, 13, "Unexpected number of extracted strings.");
			Assert.AreEqual(extractedData[1].Item1.Count, 44, "Unexpected number of extracted strings.");
			extractor.Print(extractedData);
		}

		[TestMethod]
		public void AccountPatternExtractorSearchForNoItem()
		{
			var searchFor = new List<string> { "Amazon" };

			var extractor = new AccountPatternExtractor(_ReadString, searchFor);
			var extractedData = extractor.Extract();
			Assert.AreEqual(extractedData[0].Item1.Count, 0, "No match should be found.");
			extractor.Print(extractedData);
		}
	}
}
