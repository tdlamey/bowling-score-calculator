using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BowlingApp.Models
{
	/// <summary>
	/// A class for recording and evaluating the frames, delivery values, and total score for a game.
	/// </summary>
	public class GameModel : ModelBase
	{
		#region Constructor
		/// <summary>
		/// Creates a new instance of <see cref="GameModel"/>.
		/// </summary>
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

			UpdateAvailableDeliveryValues();
		}
		#endregion

		#region Fields
		private FrameModel currentFrame;
		private DeliveryModel currentDelivery;
		private int totalScore;
		#endregion

		#region Properties
		/// <summary>
		/// A collection of individual frames for the game.
		/// </summary>
		internal List<FrameModel> Frames { get; }

		/// <summary>
		/// The frame currently in progress.
		/// </summary>
		private FrameModel CurrentFrame
		{
			get => currentFrame;
			set => SetBackingFieldAndNotify(ref currentFrame, value);
		}

		/// <summary>
		/// The delivery currently next to be recorded.
		/// </summary>
		private DeliveryModel CurrentDelivery
		{
			get => currentDelivery;
			set => SetBackingFieldAndNotify(ref currentDelivery, value);
		}

		/// <summary>
		/// A collection of the possible delivery values for the current delivery.
		/// </summary>
		internal ObservableCollection<string> CurrentDeliveryAvailableValues { get; }

		/// <summary>
		/// The running total score for the game.
		/// </summary>
		public int TotalScore
		{
			get => totalScore;
			private set => SetBackingFieldAndNotify(ref totalScore, value);
		}
		#endregion

		#region Methods
		/// <summary>
		/// Occurs when a delivery value is assigned.
		/// </summary>
		/// <param name="deliveryValue">
		/// The value to assign.
		/// </param>
		/// <exception cref="ArgumentException">
		/// The value is <paramref name="deliveryValue"/> is not valid.
		/// </exception>
		internal void OnDeliveryValueAssigned(string deliveryValue)
		{
			if (!CurrentDeliveryAvailableValues.Contains(deliveryValue))
			{
				throw new ArgumentException("The provided value is not valid for the current shot.", nameof(deliveryValue));
			}

			//Assign the shot value.
			CurrentDelivery.Value = deliveryValue;
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
						MoveToNextDeliveryInFrame();
					}
					else
					{
						//This will be the second shot

						if (CurrentDelivery.IsSpare ||
							CurrentDelivery.IsStrike ||
							CurrentFrame.Deliveries.First().IsStrike)
						{
							//Move to the next shot in the current frame.
							MoveToNextDeliveryInFrame();
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
						MoveToNextDeliveryInFrame();
					}
				}
			}
		}

		/// <summary>
		/// Moves focus to the first delivery of the next frame.
		/// </summary>
		private void MoveToNextFrame()
		{
			var next = Frames.IndexOf(CurrentFrame) + 1;

			CurrentFrame = Frames[next];
			CurrentDelivery = CurrentFrame.Deliveries.First();

			UpdateAvailableDeliveryValues();
		}

		/// <summary>
		/// Moves focus to the next delivery of the current frame.
		/// </summary>
		private void MoveToNextDeliveryInFrame()
		{
			var next = CurrentFrame.Deliveries.IndexOf(CurrentDelivery) + 1;

			CurrentDelivery = CurrentFrame.Deliveries[next];

			UpdateAvailableDeliveryValues();
		}

		/// <summary>
		/// Updates the available delivery values, based on the current state of the game.
		/// </summary>
		private void UpdateAvailableDeliveryValues()
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

		/// <summary>
		/// Disables score input.
		/// </summary>
		private void DisableInput()
		{
			CurrentDeliveryAvailableValues.Clear();
		}

		/// <summary>
		/// Iterates through all frames and assigns scores where appropriate.
		/// </summary>
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

		/// <summary>
		/// Determines the bonus score for any given frame.
		/// </summary>
		/// <param name="currentFrame">
		/// The frame for which to calculate the bonus score.
		/// </param>
		/// <returns>
		/// Returns the bonus score for the given frame.
		/// </returns>
		/// <exception cref="ArgumentNullException">
		/// The value of <paramref name="currentFrame"/> is null.
		/// </exception>
		/// <exception cref="ArgumentException">
		/// The frame referenced in <paramref name="currentFrame"/> is not found in the collection.
		/// </exception>
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
