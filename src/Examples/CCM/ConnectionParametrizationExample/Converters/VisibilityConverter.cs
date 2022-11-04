using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ConnectionParametrizationExample.Converters
{
	internal class VisibilityConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			bool isVisible = (bool)value;

			if (isVisible)
			{
				return Visibility.Visible;
			}

			return Visibility.Hidden;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
