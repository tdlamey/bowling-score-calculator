using System.Collections.Generic;
using System.Linq;

namespace BowlingApp.Models
{
	public class FrameModel : ModelBase
	{
		internal FrameModel() : this(false) { }

		internal FrameModel(bool isFinalFrame)
		{
			IsFinalFrame = isFinalFrame;

			FrameShots = new List<FrameShotModel>();

			for (int i = 0; i < MaxShotCount; i++)
			{
				FrameShots.Add(new FrameShotModel());
			}

			FrameScoreTotal = ShotValue.NotSet;
		}

		#region Fields
		private string frameScoreTotal;
		#endregion

		#region Properties
		internal bool IsFinalFrame { get; }

		private int MaxShotCount
			=> IsFinalFrame ? 3 : 2;

		internal List<FrameShotModel> FrameShots { get; }

		internal int ShotsTaken
			=> FrameShots.Count(shot => shot.HasValue);

		/// <summary>
		/// 
		/// </summary>
		/// <remarks>
		/// This score value is a string so that it can be
		/// blank in frames which have not yet been recorded.
		/// </remarks>
		public string FrameScoreTotal
		{
			get => frameScoreTotal;
			set => SetBackingFieldAndNotify(ref frameScoreTotal, value);
		}

		internal bool HasStrike
			=> FrameShots.Any(shot => shot.IsStrike);

		internal bool HasSpare
			=> FrameShots.Any(shot => shot.IsSpare);

		internal bool GetsBonusScore
			=> !IsFinalFrame && (HasStrike || HasSpare);

		internal int FrameScoreSubtotal
		{
			get
			{
				int subtotal;

				if (!FrameShots.First().HasValue)
				{
					return 0;
				}
				else if (FrameShots.First().IsStrike)
				{
					subtotal = 10;

					if (IsFinalFrame)
					{
						if (FrameShots[1].HasValue)
						{
							if (FrameShots[1].IsStrike)
							{
								subtotal += 10;

								if (FrameShots.Last().HasValue)
								{
									if (FrameShots.Last().IsStrike)
									{
										subtotal += 10;
									}
									else
									{
										//This code block indicates that the last shot must have a value and it is numerical.
										subtotal += FrameShots.Last().AsNumericalValue;
									}
								}
								//else
								// There is no final shot taken yet.
								// Return the subtotal as-is.
							}
							else if (FrameShots.Last().IsSpare)
							{
								//This code block indicates that the second shot was numerical and the third was a spare.
								subtotal += 10;
							}
							else
							{
								//This code block indicates that the second shot was taken and has a numerical value.
								// It also indicates that the last shot is not a spare, so the numerical values can be added to the subtotal as-is.
								subtotal += FrameShots[1].AsNumericalValue;

								//Since the second shot is numerical and the last shot is not a spare, the only
								// remaining possibilities for the last shot are a numerical value or yet not recorded.

								if (FrameShots.Last().HasValue)
								{
									subtotal += FrameShots.Last().AsNumericalValue;
								}
								//else
								// There is no final shot taken yet.
								// Return the subtotal as-is.
							}
						}
						//else
						// There is no second shot taken yet.
						// Return the subtotal as-is.
					}
					//else
					// A non-final frame with a strike is worth 10, and it will not have a second shot to evaluate.
					// Return the subtotal as-is.
				}
				else
				{
					//This else-block indicates the first shot has a value and it is numerical.

					if (!FrameShots[1].HasValue)
					{
						//Only the first shot has been taken, so return its numerical value.
						subtotal = FrameShots.First().AsNumericalValue;
					}
					else if (FrameShots[1].IsSpare)
					{
						subtotal = 10;

						if (IsFinalFrame)
						{
							if (FrameShots.Last().HasValue)
							{
								if (FrameShots.Last().IsStrike)
								{
									subtotal += 10;
								}
								else
								{
									//This else-block indicates that the last shot has been taken, and it is a numerical value.
									subtotal += FrameShots.Last().AsNumericalValue;
								}
							}
							//else
							// The final frame has one more shot, not yet taken.
							// Return the subtotal as-is.
						}
						//else
						// A non-final frame with a spare is worth 10, regardless of the first shot's value.
						// Return the subtotal as-is.
					}
					else
					{
						//This else-block indicates an open but completed frame, with
						// both shots having numerical values.
						// This includes the final frame, which in this case will only
						// have 2 shots taken but is still complete.
						subtotal  = FrameShots[0].AsNumericalValue;
						subtotal += FrameShots[1].AsNumericalValue;
					}
				}

				return subtotal;
			}
		}
		#endregion

		#region Methods
		internal int CalculateBonusFor(string previousFrameShotValue)
		{
			int bonus = 0;

			if (previousFrameShotValue == ShotValue.Strike)
			{
				if (ShotsTaken <= 2)
				{
					return FrameScoreSubtotal;
				}
				else
				{
					//Three shots have been taken, so calculate the total of the first two.

					if (FrameShots[1].IsSpare)
					{
						return 10;
					}
					else if (FrameShots.First().IsStrike)
					{
						bonus += 10;

						if (FrameShots[1].IsStrike)
						{
							bonus += 10;
						}
						else
						{
							//The second shot must have a numerical value.
							bonus += FrameShots[1].AsNumericalValue;
						}
					}
					else
					{
						//The first and second shots must both be numerical values.
						bonus += FrameShots[0].AsNumericalValue;
						bonus += FrameShots[1].AsNumericalValue;
					}
				}
			}
			else if (previousFrameShotValue == ShotValue.Spare)
			{
				if (FrameShots.First().HasValue)
				{
					if (FrameShots.First().IsStrike)
					{
						bonus += 10;
					}
					else
					{
						//The first shot must be numerical.
						bonus += FrameShots.First().AsNumericalValue;
					}
				}
				//else
				// If the first shot has no value, then just return a bonus of 0.
			}
			//else
			// If the previous shot was neither a strike nor a spare, then just return a bonus of 0.

			return bonus;
		}
		#endregion
	}
}
