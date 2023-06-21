using System.Collections.ObjectModel;

namespace BowlingApp.Models
{
	/// <summary>
	/// A collection of delivery display values for a single frame.
	/// </summary>
	/// <remarks>
	/// Use of this custom class will ensure that the collection always contains exactly 3 items.
	/// If at any point an attempt is made to remove one or more items, they will not actually be removed,
	/// but instead the item at that index will simply be set to the <see cref="ShotValue.NotSet"/> value.
	/// </remarks>
	public class DeliveryDisplayCollection : ObservableCollection<string>
	{
		/// <summary>
		/// Creates a new instance of <see cref="DeliveryDisplayCollection"/>.
		/// </summary>
		public DeliveryDisplayCollection()
		{
			Add(ShotValue.NotSet);
			Add(ShotValue.NotSet);
			Add(ShotValue.NotSet);
		}

		/// <summary>
		/// Sets the value of the item at the given index to <see cref="ShotValue.NotSet"/>.
		/// </summary>
		/// <param name="index">
		/// The index of the item to modify.
		/// </param>
		protected override void RemoveItem(int index)
		{
			Items[index] = ShotValue.NotSet;
		}

		/// <summary>
		/// Sets the value of all items to <see cref="ShotValue.NotSet"/>.
		/// </summary>
		protected override void ClearItems()
		{
			for (int i = 0; i < Items.Count; i++)
			{
				Items[i] = ShotValue.NotSet;
			}
		}
	}
}
