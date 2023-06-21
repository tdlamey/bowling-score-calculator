using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace BowlingApp.Converters
{
	public class BooleanToVisibilityConverter : ValueConverter
	{
		public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value), "The converter value may not be null.");
			}
			if (targetType != typeof(Visibility) && targetType != typeof(Visibility?))
			{
				throw new ArgumentException("Invalid target type for the specified converter: " + targetType.Name, nameof(targetType));
			}

			if (value is bool bValue)
			{
				return bValue ? Visibility.Visible : Visibility.Collapsed;
			}
			else
			{
				throw new ArgumentException("Invalid binding source type for the specified converter: " + value.GetType().Name, nameof(value));
			}
		}
	}
}
