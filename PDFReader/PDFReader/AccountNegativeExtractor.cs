using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	/// <summary>
	/// This class extracts all negative amounts from the account avoiding internal transfers
	/// </summary>
	class AccountNegativeExtractor : IExtractor
	{
		#region Fields

		private readonly string _SearchIn;

		#endregion

		#region Constructors

		public AccountNegativeExtractor(string searchIn)
		{
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
