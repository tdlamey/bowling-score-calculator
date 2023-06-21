using System.Collections.Generic;
using System.Linq;

namespace BowlingApp.Models
{
	public class FrameModel : ModelBase
	{
		#region Constructors
		internal FrameModel() : this(false) { }

		internal FrameModel(bool isFinalFrame)
		{
			IsFinalFrame = isFinalFrame;

			Deliveries = new List<DeliveryModel>();

			for (int i = 0; i < MaxShotCount; i++)
			{
				Deliveries.Add(new DeliveryModel());
			}

			DeliveryDisplay = new DeliveryDisplayCollection();

			FrameScoreTotal = ShotValue.NotSet;
		}
		#endregion

		#region Fields
		private string frameScoreTotal;
		#endregion

		#region Properties
		//No need to implement UI notification for this property because
		// it is assigned in the constructor and cannot be changed after that.
		public bool IsFinalFrame { get; }

		private int MaxShotCount
			=> IsFinalFrame ? 3 : 2;

		internal List<DeliveryModel> Deliveries { get; }

		public DeliveryDisplayCollection DeliveryDisplay { get; }

		internal int ShotsTaken
			=> Deliveries.Count(shot => shot.HasValue);

		internal bool HasStrike
			=> Deliveries.Any(shot => shot.IsStrike);

		internal bool HasSpare
			=> Deliveries.Any(shot => shot.IsSpare);

		internal bool GetsBonusScore
			=> !IsFinalFrame && (HasStrike || HasSpare);

		internal int FrameScoreSubtotal
		{
			get
			{
				int subtotal;

				if (!Deliveries.First().HasValue)
				{
					return 0;
				}
				else if (Deliveries.First().IsStrike)
				{
					subtotal = 10;

					if (IsFinalFrame)
					{
						if (Deliveries[1].HasValue)
						{
							if (Deliveries[1].IsStrike)
							{
								subtotal += 10;

								if (Deliveries.Last().HasValue)
								{
									if (Deliveries.Last().IsStrike)
									{
										subtotal += 10;
									}
									else
									{
										//This code block indicates that the last shot must have a value and it is numerical.
										subtotal += Deliveries.Last().AsNumericalValue;
									}
								}
								//else
								// There is no final shot taken yet.
								// Return the subtotal as-is.
							}
							else if (Deliveries.Last().IsSpare)
							{
								//This code block indicates that the second shot was numerical and the third was a spare.
								subtotal += 10;
							}
							else
							{
								//This code block indicates that the second shot was taken and has a numerical value.
								// It also indicates that the last shot is not a spare, so the numerical values can be added to the subtotal as-is.
								subtotal += Deliveries[1].AsNumericalValue;

								//Since the second shot is numerical and the last shot is not a spare, the only
								// remaining possibilities for the last shot are a numerical value or yet not recorded.

								if (Deliveries.Last().HasValue)
								{
									subtotal += Deliveries.Last().AsNumericalValue;
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

					if (!Deliveries[1].HasValue)
					{
						//Only the first shot has been taken, so return its numerical value.
						subtotal = Deliveries.First().AsNumericalValue;
					}
					else if (Deliveries[1].IsSpare)
					{
						subtotal = 10;

						if (IsFinalFrame)
						{
							if (Deliveries.Last().HasValue)
							{
								if (Deliveries.Last().IsStrike)
								{
									subtotal += 10;
								}
								else
								{
									//This else-block indicates that the last shot has been taken, and it is a numerical value.
									subtotal += Deliveries.Last().AsNumericalValue;
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
						subtotal  = Deliveries[0].AsNumericalValue;
						subtotal += Deliveries[1].AsNumericalValue;
					}
				}

				return subtotal;
			}
		}

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
		#endregion

		#region Methods
		internal void UpdateDeliveryDisplay()
		{
			if (IsFinalFrame)
			{
				DeliveryDisplay[0] = Deliveries[0].DisplayValue;
				DeliveryDisplay[1] = Deliveries[1].DisplayValue;
				DeliveryDisplay[2] = Deliveries[2].DisplayValue;
			}
			else
			{
				DeliveryDisplay[0] = ShotValue.NotSet;

				if (HasStrike)
				{
					DeliveryDisplay[1] = ShotValue.NotSet;
					DeliveryDisplay[2] = Deliveries[0].DisplayValue;
				}
				else
				{
					DeliveryDisplay[1] = Deliveries[0].DisplayValue;
					DeliveryDisplay[2] = Deliveries[1].DisplayValue;
				}
			}
		}

		internal int CalculateBonusFor(string previousDeliveryValue)
		{
			int bonus = 0;

			if (previousDeliveryValue == ShotValue.Strike)
			{
				if (ShotsTaken <= 2)
				{
					return FrameScoreSubtotal;
				}
				else
				{
					//Three shots have been taken, so calculate the total of the first two.

					if (Deliveries[1].IsSpare)
					{
						return 10;
					}
					else if (Deliveries.First().IsStrike)
					{
						bonus += 10;

						if (Deliveries[1].IsStrike)
						{
							bonus += 10;
						}
						else
						{
							//The second shot must have a numerical value.
							bonus += Deliveries[1].AsNumericalValue;
						}
					}
					else
					{
						//The first and second shots must both be numerical values.
						bonus += Deliveries[0].AsNumericalValue;
						bonus += Deliveries[1].AsNumericalValue;
					}
				}
			}
			else if (previousDeliveryValue == ShotValue.Spare)
			{
				if (Deliveries.First().HasValue)
				{
					if (Deliveries.First().IsStrike)
					{
						bonus += 10;
					}
					else
					{
						//The first shot must be numerical.
						bonus += Deliveries.First().AsNumericalValue;
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
