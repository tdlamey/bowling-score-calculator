using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingApp.Models
{
	public class GameModel : ModelBase
	{
		#region Constructor
		internal GameModel()
		{
			Frames = new List<FrameModel>();

			for (int i = 0; i < 9; i++)
			{
				Frames.Add(new FrameModel());
			}

			Frames.Add(new FrameModel(true));

			CurrentFrame = Frames.First();
			CurrentDelivery = CurrentFrame.Deliveries.First();

			CurrentDeliveryAvailableValues = new ObservableCollection<string>();

			UpdateAvailableShotValues();
		}
		#endregion

		#region Fields
		private FrameModel currentFrame;
		private DeliveryModel currentDelivery;
		private int totalScore;
		#endregion

		#region Properties
		internal List<FrameModel> Frames { get; }

		private FrameModel CurrentFrame
		{
			get => currentFrame;
			set => SetBackingFieldAndNotify(ref currentFrame, value);
		}

		private DeliveryModel CurrentDelivery
		{
			get => currentDelivery;
			set => SetBackingFieldAndNotify(ref currentDelivery, value);
		}

		internal ObservableCollection<string> CurrentDeliveryAvailableValues { get; }

		public int TotalScore
		{
			get => totalScore;
			private set => SetBackingFieldAndNotify(ref totalScore, value);
		}
		#endregion

		#region Methods
		internal void OnShotValueAssigned(string shotValue)
		{
			if (!CurrentDeliveryAvailableValues.Contains(shotValue))
			{
				throw new ArgumentException("The provided value is not valid for the current shot.", nameof(shotValue));
			}

			//Assign the shot value.
			CurrentDelivery.Value = shotValue;
			CurrentFrame.UpdateDeliveryDisplay();

			//Update the current score.
			UpdateScore();

			//Determine what to do next.
			if (CurrentFrame.IsFinalFrame)
			{
				if (CurrentDelivery == CurrentFrame.Deliveries.Last())
				{
					//Disable all input and do nothing else.
					DisableInput();
				}
				else
				{
					if (CurrentDelivery == CurrentFrame.Deliveries.First())
					{
						//Move to the next shot in the current frame.
						MoveToNextShotInFrame();
					}
					else
					{
						//This will be the second shot

						if (CurrentDelivery.IsSpare ||
							CurrentDelivery.IsStrike ||
							CurrentFrame.Deliveries.First().IsStrike)
						{
							//Move to the next shot in the current frame.
							MoveToNextShotInFrame();
						}
						else
						{
							//Disable all input and do nothing else.
							DisableInput();
						}
					}
				}
			}
			else
			{
				if (CurrentDelivery == CurrentFrame.Deliveries.Last())
				{
					//Move to next frame.
					MoveToNextFrame();
				}
				else
				{
					if (CurrentDelivery.IsStrike)
					{
						//Move to the next frame.
						MoveToNextFrame();
					}
					else
					{
						//Move to the next shot in the current frame.
						MoveToNextShotInFrame();
					}
				}
			}
		}

		private void MoveToNextFrame()
		{
			var next = Frames.IndexOf(CurrentFrame) + 1;

			CurrentFrame = Frames[next];
			CurrentDelivery = CurrentFrame.Deliveries.First();

			UpdateAvailableShotValues();
		}

		private void MoveToNextShotInFrame()
		{
			var next = CurrentFrame.Deliveries.IndexOf(CurrentDelivery) + 1;

			CurrentDelivery = CurrentFrame.Deliveries[next];

			UpdateAvailableShotValues();
		}

		private void UpdateAvailableShotValues()
		{
			List<string> shotValues;

			if (CurrentDelivery == CurrentFrame.Deliveries.First())
			{
				shotValues = ShotValue.FirstShotValues;
			}
			else
			{
				if (CurrentFrame.IsFinalFrame)
				{
					var previousShotIndex = CurrentFrame.Deliveries.IndexOf(CurrentDelivery) - 1;
					var previousShotValue = CurrentFrame.Deliveries[previousShotIndex].Value;

					shotValues = ShotValue.GetNextShotValues(previousShotValue);
				}
				else
				{
					shotValues = ShotValue.GetNextShotValues(CurrentFrame.Deliveries.First().Value);
				}
			}

			CurrentDeliveryAvailableValues.Clear();
			CurrentDeliveryAvailableValues.AddRange(shotValues);
		}

		private void DisableInput()
		{
			CurrentDeliveryAvailableValues.Clear();
		}

		private void UpdateScore()
		{
			int total = 0;

			foreach (var frame in Frames)
			{
				//If no shots have been taken in the current frame, ensure
				// the frame score total is cleared and continue to the next frame.
				if (frame.ShotsTaken == 0)
				{
					frame.FrameScoreTotal = ShotValue.NotSet;
					continue;
				}

				//Add the score for just the pins hit in the current frame.
				total += frame.FrameScoreSubtotal;

				//Add a bonus score for strikes and spares.
				// If the current frame does not get a bonus score, this will simply add 0.
				total += GetScoreBonus(frame);

				frame.FrameScoreTotal = total.ToString();
			}

			TotalScore = total;
		}

		private int GetScoreBonus(FrameModel currentFrame)
		{
			int bonus = 0;

			if (currentFrame == null)
			{
				throw new ArgumentNullException(nameof(currentFrame), "Cannot perform bonus calculation, frame object is null.");
			}

			//If the current frame is not eligible for bonus scoring, simply return 0.
			// For any code after this if-block, we can safely assume the current frame
			// is non-final and has a strike or spare.
			if (!currentFrame.GetsBonusScore)
			{
				return 0;
			}

			var index = Frames.IndexOf(currentFrame);
			if (index == -1)
			{
				throw new ArgumentException("Frame not found int the game.", nameof(currentFrame));
			}

			var shotReceivingBonus = currentFrame.HasStrike ? ShotValue.Strike : ShotValue.Spare;

			//Get the next frame.
			// Since the current frame is eligible for a bonus score, we know it's
			// not the final frame. Therefore, index checking is not necessary here.
			FrameModel nextFrame = Frames[index + 1];

			if (nextFrame.IsFinalFrame || currentFrame.HasSpare)
			{
				//If the next frame is the final one, or if the current frame has only a spare,
				// then we can only get a bonus from the very next frame.
				bonus += nextFrame.CalculateBonusFor(shotReceivingBonus);
			}
			else
			{
				//If this else-block executes, we can assume the current frame
				// has a strike and the next frame is not the final one.

				//Get the next frame's bonus score.
				bonus += nextFrame.CalculateBonusFor(shotReceivingBonus);

				//If the next frame only had one shot to give bonus points for, then
				// attempt to get additional bonus points from the shot after that.
				if (nextFrame.ShotsTaken == 1)
				{
					//Since we know the next frame is not the final one, we can safely get
					// the frame after that without index checking.
					FrameModel nextFrame2 = Frames[index + 2];

					//Hard-code a spare into this argument, so we only get the bonus points
					// from the first shot, if there is one.
					bonus += nextFrame2.CalculateBonusFor(ShotValue.Spare);
				}
			}

			return bonus;
		}
		#endregion
	}
}
