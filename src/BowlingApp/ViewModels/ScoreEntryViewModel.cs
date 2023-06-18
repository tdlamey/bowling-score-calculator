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
