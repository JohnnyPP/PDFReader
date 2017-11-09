using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	internal interface IExtractor
	{
		Tuple<List<DateTime>, List<double>, List<string>> Extract();
	}
}
