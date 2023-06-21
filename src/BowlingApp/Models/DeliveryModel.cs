namespace BowlingApp.Models
{
	/// <summary>
	/// A class for recording and evaluating individual delivery values.
	/// </summary>
	internal class DeliveryModel
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="DeliveryModel"/>.
		/// </summary>
		internal DeliveryModel()
		{
			Value = DeliveryValue.NotSet;
		}
		#endregion

		#region Properties
		/// <summary>
		/// The recorded value, if any, of the current delivery.
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		/// Converts the delivery to a display value.
		/// </summary>
		internal string DisplayValue
			=> DeliveryValue.GetDisplayValue(Value);

		/// <summary>
		/// Determines whether the delivery has a value recorded.
		/// </summary>
		internal bool HasValue
			=> Value != DeliveryValue.NotSet;

		/// <summary>
		/// Determines whether the delivery is a strike.
		/// </summary>
		internal bool IsStrike
			=> Value == DeliveryValue.Strike;

		/// <summary>
		/// Determines whether the delivery is a spare.
		/// </summary>
		internal bool IsSpare
			=> Value == DeliveryValue.Spare;

		/// <summary>
		/// Determines whether the delivery is a numerical value.
		/// </summary>
		internal bool IsNumerical
			=> DeliveryValue.NumericalValues.Contains(Value);

		/// <summary>
		/// Converts the delivery's value to a pin count.
		/// </summary>
		/// <remarks>
		/// Use of this property is not supported for a spare value.
		/// </remarks>
		internal int AsNumericalValue
			=> IsSpare ? -1 : IsStrike ? 10 : HasValue ? int.Parse(Value) : 0;
		#endregion
	}
}
