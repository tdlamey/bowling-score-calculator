using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BowlingApp
{
	/// <summary>
	/// Provides members for notifying the user interface that a change has occurred.
	/// </summary>
	public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
	{
		/// <summary>
		/// Occurs when a property value has changed.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Notifies the user interface that the specified property has changed.
		/// </summary>
		/// <param name="propertyName">
		/// The name of the property that changed.
		/// </param>
		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		/// <summary>
		/// Sets the backing field and notifies the user interface that the specified property has changed.
		/// </summary>
		/// <typeparam name="TProperty"></typeparam>
		/// <param name="backingField">
		/// The backing field to be assigned.
		/// </param>
		/// <param name="newValue">
		/// The new value to assign to the backing field.
		/// </param>
		/// <param name="propertyName">
		/// The name of the property that changed.
		/// </param>
		protected void SetBackingFieldAndNotify<TProperty>(ref TProperty backingField, TProperty newValue, [CallerMemberName] string propertyName = null)
		{
			//Using the Check-And-Return style here to show its usage.

			//If the values reference the same object in memory, do nothing.
			//This is suitable for all object types.
			if (ReferenceEquals(backingField, newValue)) return;

			//If the values are a 'value type' (like int or bool), compare their values.
			// This will handle mutable types which are equal but have different references.
			if (typeof(TProperty).IsValueType && typeof(IEquatable<TProperty>).IsAssignableFrom(typeof(TProperty)) && backingField.Equals(newValue)) return;

			//If none of the above checks apply, set the new value and notify.
			backingField = newValue;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
