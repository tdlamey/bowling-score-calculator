using System;
using System.Collections.Generic;

namespace BowlingApp
{
	public class ShotValue
	{
		internal static readonly string NotSet = string.Empty;

		public const string Zero   = "0";
		public const string One    = "1";
		public const string Two    = "2";
		public const string Three  = "3";
		public const string Four   = "4";
		public const string Five   = "5";
		public const string Six    = "6";
		public const string Seven  = "7";
		public const string Eight  = "8";
		public const string Nine   = "9";
		public const string Spare  = "/";
		public const string Strike = "X";

		internal static List<string> AllValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Spare, Strike };

		internal static List<string> FirstShotValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Strike };

		internal static List<string> NumericalValues = new()
			{ Zero, One, Two, Three, Four, Five, Six, Seven, Eight, Nine };

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
	}
}
