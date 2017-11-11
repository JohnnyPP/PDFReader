using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	/// <summary>
	/// This class extracts all positive amounts from the account avoiding internal transfers
	/// </summary>
	class AccountPositiveExtractor : AccountExtractor, IExtractor
	{
		#region Fields

		private readonly string _SearchIn;
		private readonly List<Tuple<string, string>> _SearchFor;

		#endregion

		#region Constructors

		public AccountPositiveExtractor(string searchIn, List<Tuple<string, string>> searchFor) : base(searchIn, searchFor)
		{
			_SearchFor = searchFor;
			_SearchIn = searchIn;
		}

		#endregion

		#region Methods

		public List<Tuple<List<DateTime>, List<double>, List<string>>> Extract()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
