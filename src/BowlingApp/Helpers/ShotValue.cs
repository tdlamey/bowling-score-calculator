using System;
using System.Collections.Generic;

namespace BowlingApp
{
	/// <summary>
	/// A class for providing general information about individual shot values.
	/// </summary>
	public class ShotValue
	{
		#region Fields
		internal static readonly string NotSet = string.Empty;

		public const string ZeroDisplay = "-";
		public const string Zero        = "0";
		public const string One         = "1";
		public const string Two         = "2";
		public const string Three       = "3";
		public const string Four        = "4";
		public const string Five        = "5";
		public const string Six         = "6";
		public const string Seven       = "7";
		public const string Eight       = "8";
		public const string Nine        = "9";
		public const string Spare       = "/";
		public const string Strike      = "X";
		#endregion

		#region Properties
		/// <summary>
		/// All possible delivery values.
		/// </summary>
		internal static List<string> AllValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Spare, Strike };

		/// <summary>
		/// All possible values for a first delivery.
		/// </summary>
		internal static List<string> FirstShotValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Strike };

		/// <summary>
		/// All numerical values for a delivery.
		/// </summary>
		internal static List<string> NumericalValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine };
		#endregion

		#region Methods
		/// <summary>
		/// Converts a delivery value to its corresponding display value.
		/// </summary>
		/// <param name="value">
		/// The delivery value to convert.
		/// </param>
		/// <returns>
		/// Returns "-" for a zero value, all other values remain as-is.
		/// </returns>
		internal static string GetDisplayValue(string value)
		{
			return value == Zero ? ZeroDisplay : value;
		}

		/// <summary>
		/// Returns the next possible delivery values, based on the previous delivery.
		/// </summary>
		/// <param name="previousShotValue">
		/// The value of the previous delivery.
		/// </param>
		/// <returns>
		/// Returns the next possible delivery values, based on the previous delivery.
		/// </returns>
		/// <exception cref="ArgumentException">
		/// The value passed in <paramref name="previousShotValue"/> is not a valid value.
		/// </exception>
		internal static List<string> GetNextShotValues(string previousShotValue)
		{
			if (previousShotValue == Strike || previousShotValue == Spare)
			{
				return FirstShotValues;
			}

			if (!NumericalValues.Contains(previousShotValue))
			{
				throw new ArgumentException("Invalid argument value.", nameof(previousShotValue));
			}

			int previousShot = int.Parse(previousShotValue);

			var shotValues = new List<string>();

			foreach (var shotValue in NumericalValues)
			{
				if (10 > previousShot + int.Parse(shotValue))
				{
					shotValues.Add(shotValue);
				}
			}

			shotValues.Add(Spare);

			return shotValues;
		}
		#endregion
	}
}
