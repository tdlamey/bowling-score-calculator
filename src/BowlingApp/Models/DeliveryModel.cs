namespace BowlingApp.Models
{
	internal class DeliveryModel
	{
		#region Constructor
		internal DeliveryModel()
		{
			Value = ShotValue.NotSet;
		}
		#endregion

		#region Properties
		public string Value { get; set; }

		internal string DisplayValue
			=> ShotValue.GetDisplayValue(Value);

		internal bool HasValue
			=> Value != ShotValue.NotSet;

		internal bool IsStrike
			=> Value == ShotValue.Strike;

		internal bool IsSpare
			=> Value == ShotValue.Spare;

		internal bool IsNumerical
			=> ShotValue.NumericalValues.Contains(Value);

		internal int AsNumericalValue
			=> IsSpare ? -1 : IsStrike ? 10 : HasValue ? int.Parse(Value) : 0;
		#endregion
	}
}
