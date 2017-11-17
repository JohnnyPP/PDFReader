using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFReader
{
	/// <summary>
	/// This class sums the account amount for each month
	/// </summary>
	public class AccountMonthlySum
	{
        #region Fields

        private int _Month;
        private int _LastValue = 0;
        private int _MonthIndex = 0;
        private int _StoredMonthIndex = 0;
        private int _StoredPreviousMonthIndex = 0;
        private List<Tuple<List<DateTime>, List<double>, List<string>>> _DataToSum;
        private List<double> _MonthlySums = new List<double>();
        private Enum _SumType;

        #endregion

        #region Constructors

        public AccountMonthlySum(List<Tuple<List<DateTime>, List<double>, List<string>>> dataToSum)
		{
            _DataToSum = dataToSum;
        }

        #endregion

        #region Properties

        private int Month
        {
            get { return _Month; }
            set
            {
                _Month = value;
                if (_Month != _LastValue)
                {
                    _LastValue = _Month;
                    _StoredMonthIndex = _MonthIndex;

                    if (_StoredMonthIndex != 0)
                    {
                        _MonthlySums.Add(_DataToSum[Convert.ToInt32(_SumType)].Item2.GetRange(_StoredPreviousMonthIndex, _StoredMonthIndex - _StoredPreviousMonthIndex).Sum());
                        _StoredPreviousMonthIndex = _StoredMonthIndex;
                    }
                }
            }
        }

        public List<double> MonthlySums
        {
            get { return _MonthlySums; }
            set { _MonthlySums = value; }
        }

        #endregion

        #region Methods

        public void Sum(Enum sumType)
		{
            _SumType = sumType;
            foreach (var item in _DataToSum[Convert.ToInt32(_SumType)].Item1)
            {
                Month = item.Month;
                _MonthIndex++;
                if (_MonthIndex == _DataToSum[Convert.ToInt32(_SumType)].Item1.Count)
                    Month = 0; //triggers last summing
            }
        }

        public void PrintSum()
        {
            _MonthlySums.ForEach(Console.WriteLine);
        }

		#endregion
	}
}
