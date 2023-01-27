using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ConnectionParametrizationExample.Converters
{
	public class CheckBoxToListConverter : IValueConverter
	{
		List<string> bound;
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bound = value as List<string>;

			if (bound.Contains(parameter.ToString()))
				return true;
			else
				return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isChecked = (bool)value;

			if (isChecked)
			{
				bound.Add(parameter.ToString());
			}
			else
			{
				bound.Remove(parameter.ToString());
			}
			return bound;
		}
	}
}
