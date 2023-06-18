using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace BowlingApp.Converters
{
	public class DictionaryValueConverter : IMultiValueConverter
	{
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if (values == null)
			{
				throw new ArgumentNullException(nameof(values), "The converter value may not be null.");
			}
			if (values.Length < 2)
			{
				throw new ArgumentNullException(nameof(values), "Some values are missing from the multi-binding source list.");
			}

			if (values[0] is string key)
			{
				if (values[1] is IDictionary<string, bool> dictionary)
				{
					if (dictionary.TryGetValue(key, out bool returnValue))
					{
						return returnValue;
					}
					else
					{
						throw new ArgumentException($"The specified key was not found in the dictionary. Key: {key}", nameof(values));
					}
				}
				else
				{
					throw new ArgumentException($"Values of this type are not supported. Type: {values[1].GetType().Name}", nameof(values));
				}
			}
			else
			{
				throw new ArgumentException($"The key type is invalid for this dictionary. Type: {values[0].GetType().Name}", nameof(values));
			}
		}

		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			//This converter will only be used for 'OneWay' binding, so the ConvertBack method is not required.
			throw new NotSupportedException("This operation is not supported.");
		}
	}
}
