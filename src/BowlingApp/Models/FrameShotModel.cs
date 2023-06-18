namespace BowlingApp.Models
{
	public class FrameShotModel : ModelBase
	{
		#region Constructor
		internal FrameShotModel()
		{
			if (IsInDesignMode)
			{
				Value = ShotValue.Strike;
			}
		}
		#endregion

		#region Fields
		private string shotValue = ShotValue.NotSet;
		#endregion

		#region Properties
		public string Value
		{
			get => shotValue;
			set => SetBackingFieldAndNotify(ref shotValue, value);
		}

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
