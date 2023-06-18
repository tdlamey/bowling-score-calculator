using System;

namespace BowlingApp.Helpers
{
	internal delegate void ShotValueEvent(object sender, ShotValueEventArgs e);

	internal class ShotValueEventArgs : EventArgs
	{
		internal ShotValueEventArgs(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}

			if (!ShotValue.AllValues.Contains(value))
			{
				throw new ArgumentException();
			}

			Value = value;
		}

		internal string Value { get; }
	}
}
