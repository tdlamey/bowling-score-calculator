using System;

namespace BowlingApp
{
	/// <summary>
	/// A delegate to handle delivery value events.
	/// </summary>
	/// <param name="sender">
	/// The object which sent the event.
	/// </param>
	/// <param name="e">
	/// Information about the event.
	/// </param>
	internal delegate void DeliveryValueEvent(object sender, DeliveryValueEventArgs e);

	/// <summary>
	/// A class for holding relevant information during a delivery event.
	/// </summary>
	internal class DeliveryValueEventArgs : EventArgs
	{
		/// <summary>
		/// Creates a new instance of <see cref="DeliveryValueEventArgs"/>.
		/// </summary>
		/// <param name="value">
		/// The delivery value.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The value of <paramref name="value"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The value of <paramref name="value"/> is not valid.
		/// </exception>
		internal DeliveryValueEventArgs(string value)
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}

			if (!DeliveryValue.AllValues.Contains(value))
			{
				throw new ArgumentException();
			}

			Value = value;
		}

		/// <summary>
		/// The delivery value.
		/// </summary>
		internal string Value { get; }
	}
}
