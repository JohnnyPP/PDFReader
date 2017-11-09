﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	/// <summary>
	/// This class extracts all positive amounts from the account avoiding internal transfers
	/// </summary>
	class AccountPositiveExtractor : IExtractor
	{
		#region Fields

		private readonly string _SearchIn;

		#endregion

		#region Constructors

		public AccountPositiveExtractor(string searchIn)
		{
			_SearchIn = searchIn;
		}

		#endregion

		

		#region Methods

		public Tuple<List<DateTime>, List<double>, List<string>> Extract()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
