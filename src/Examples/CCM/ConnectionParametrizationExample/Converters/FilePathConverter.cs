using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ConnectionParametrizationExample.Converters
{
	internal class FilePathConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string path = (string)value;
			return Path.GetFileName(path);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
