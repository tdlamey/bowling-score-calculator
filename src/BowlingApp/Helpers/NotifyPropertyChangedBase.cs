using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BowlingApp
{
	public abstract class NotifyPropertyChangedBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected static bool IsInDesignMode
			=> DesignerProperties.GetIsInDesignMode(new DependencyObject());

		protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

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
