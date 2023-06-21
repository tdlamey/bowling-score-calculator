using System.Collections.ObjectModel;

namespace BowlingApp.Models
{
	//We will create a custom class to ensure that the collection always contains exactly 3 items.
	// If at any point an attempt is made to remove one or more items, they will not actually be removed,
	// but instead the item at that index will simply be set to the NotSet value, which is an empty string.
	public class DeliveryDisplayCollection : ObservableCollection<string>
	{
		public DeliveryDisplayCollection()
		{
			Add(ShotValue.NotSet);
			Add(ShotValue.NotSet);
			Add(ShotValue.NotSet);
		}

		protected override void RemoveItem(int index)
		{
			Items[index] = ShotValue.NotSet;
		}

		protected override void ClearItems()
		{
			for (int i = 0; i < Items.Count; i++)
			{
				Items[i] = ShotValue.NotSet;
			}
		}
	}
}
