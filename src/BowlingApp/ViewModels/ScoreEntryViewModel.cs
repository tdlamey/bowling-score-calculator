﻿using BowlingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BowlingApp.ViewModels
{
	/// <summary>
	/// A view model for binding to the score input view.
	/// </summary>
	public class ScoreEntryViewModel : ViewModelBase
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="ScoreEntryViewModel"/>.
		/// </summary>
		public ScoreEntryViewModel()
		{
			ScoreInputCommand = new ParameterizedCommand(obj => OnScoreInput(obj as string));

			IsScoreInputEnabled = new ObservableDictionary<string, bool>();

			//Populate the dictionary for each button
			foreach (var shotValue in ShotValue.AllValues)
			{
				IsScoreInputEnabled.Add(shotValue, true);
			}
		}
		#endregion

		#region Events
		/// <summary>
		/// Occurs when a delivery value has been assigned by user input.
		/// </summary>
		internal event DeliveryValueEvent DeliveryValueAssigned;
		#endregion

		#region Properties
		/// <summary>
		/// A command to trigger when a score value is input by the user.
		/// </summary>
		public ICommand ScoreInputCommand { get; }

		/// <summary>
		/// A collection of score input values which are enabled for user input
		/// </summary>
		public ObservableDictionary<string, bool> IsScoreInputEnabled { get; }
		#endregion

		#region Methods
		/// <summary>
		/// Occurs when a delivery value has been assigned by user input by way of the keyboard.
		/// </summary>
		/// <param name="key"></param>
		internal void OnKeyPressed(Key key)
		{
			var shotValue = ConvertKeyToString(key);

			if (shotValue != null && IsScoreInputEnabled[shotValue])
			{
				OnScoreInput(shotValue);
			}
			//else
			// The key is not a valid one, so ignore it.
		}

		/// <summary>
		/// Converts a value of the <see cref="Key"/> enumeration to a valid
		/// delivery value, or null if not valid.
		/// </summary>
		/// <param name="key">
		/// The key value to convert.
		/// </param>
		/// <returns>
		/// Returns the key value as a valid delivery value, or null if not valid.
		/// </returns>
		private static string ConvertKeyToString(Key key)
		{
			switch (key)
			{
				case Key.X:
					return ShotValue.Strike;
				case Key.Divide:
				case Key.OemBackslash:
				case Key.Oem2:
					return ShotValue.Spare;
				case Key.D0:
				case Key.NumPad0:
					return ShotValue.Zero;
				case Key.D1:
				case Key.NumPad1:
					return ShotValue.One;
				case Key.D2:
				case Key.NumPad2:
					return ShotValue.Two;
				case Key.D3:
				case Key.NumPad3:
					return ShotValue.Three;
				case Key.D4:
				case Key.NumPad4:
					return ShotValue.Four;
				case Key.D5:
				case Key.NumPad5:
					return ShotValue.Five;
				case Key.D6:
				case Key.NumPad6:
					return ShotValue.Six;
				case Key.D7:
				case Key.NumPad7:
					return ShotValue.Seven;
				case Key.D8:
				case Key.NumPad8:
					return ShotValue.Eight;
				case Key.D9:
				case Key.NumPad9:
					return ShotValue.Nine;
				default:
					return null;
			}
		}

		/// <summary>
		/// Occurs when a delivery value has been assigned by user input.
		/// </summary>
		/// <param name="deliveryValue">
		/// The value of the individual delivery being assigned.
		/// </param>
		/// <exception cref="ArgumentException">
		/// The value of <paramref name="deliveryValue"/> is not valid.
		/// </exception>
		private void OnScoreInput(string deliveryValue)
		{
			if (ShotValue.AllValues.Contains(deliveryValue))
			{
				IsScoreInputEnabled[deliveryValue] = false;
				DeliveryValueAssigned?.Invoke(this, new DeliveryValueEventArgs(deliveryValue));
			}
			else
			{
				throw new ArgumentException("Invalid entry.", nameof(deliveryValue));
			}
		}

		/// <summary>
		/// Enables the score entry values provided, and disables all others.
		/// </summary>
		/// <param name="scoreEntries">
		/// The score values to enable.
		/// </param>
		/// <remarks>
		/// This method is not used, but provided to demonstrate use of
		/// overloading and of variable parameter input.
		/// </remarks>
		internal void SetAvailableShotValues(params string[] scoreEntries)
		{
			SetAvailableShotValues(scoreEntries.AsEnumerable());
		}

		/// <summary>
		/// Enables the score entry values provided, and disables all others.
		/// </summary>
		/// <param name="scoreEntries">
		/// The score values to enable.
		/// </param>
		internal void SetAvailableShotValues(IEnumerable<string> scoreEntries)
		{
			//Loop through the available entries, rather than looping through the
			// provided "available" entries, so invalid arguments will not cause issues
			// without providing addition code for checking.
			foreach (var key in IsScoreInputEnabled.Keys)
			{
				IsScoreInputEnabled[key] = scoreEntries.Contains(key);
			}
		}
		#endregion
	}
}
