using BowlingApp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace BowlingApp.ViewModels
{
	public class ScoreEntryViewModel : ViewModelBase
	{
		#region Constructor
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
		internal event ShotValueEvent ShotValueSelected;
		#endregion

		#region Properties
		public ICommand ScoreInputCommand { get; }

		public ObservableDictionary<string, bool> IsScoreInputEnabled { get; }
		#endregion

		#region Methods
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

		private void OnScoreInput(string shotValue)
		{
			if (ShotValue.AllValues.Contains(shotValue))
			{
				IsScoreInputEnabled[shotValue] = false;
				ShotValueSelected?.Invoke(this, new ShotValueEventArgs(shotValue));
			}
			else
			{
				throw new ArgumentException("Invalid entry.", nameof(shotValue));
			}
		}

		internal void SetAvailableShotValues(params string[] scoreEntries)
		{
			//This overload is unused, but I included it to
			// demonstrate the usage of variable parameter input.

			SetAvailableShotValues(scoreEntries.AsEnumerable());
		}

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
