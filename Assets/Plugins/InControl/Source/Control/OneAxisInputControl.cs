namespace InControl
{
	using System;
	using UnityEngine;


	public class OneAxisInputControl : IInputControl
	{
		public ulong UpdateTick { get; protected set; }

		float sensitivity = 1.0f;
		float lowerDeadZone = 0.0f;
		float upperDeadZone = 1.0f;
		float stateThreshold = 0.0f;

		public float FirstRepeatDelay = 0.8f;
		public float RepeatDelay = 0.1f;

		public bool Raw;

		internal bool Enabled = true;

		ulong pendingTick;
		bool pendingCommit;

		float nextRepeatTime;
		float lastPressedTime;
		bool wasRepeated;

		bool clearInputState;

		InputControlState lastState;
		InputControlState nextState;
		InputControlState thisState;


		void PrepareForUpdate( ulong updateTick )
		{
			if (IsNull)
			{
				return;
			}

			if (updateTick < pendingTick)
			{
				throw new InvalidOperationException( "Cannot be updated with an earlier tick." );
			}

			if (pendingCommit && updateTick != pendingTick)
			{
				throw new InvalidOperationException( "Cannot be updated for a new tick until pending tick is committed." );
			}

			if (updateTick > pendingTick)
			{
				lastState = thisState;
				nextState.Reset();
				pendingTick = updateTick;
				pendingCommit = true;
			}
		}


		public bool UpdateWithState( bool state, ulong updateTick, float deltaTime )
		{
			if (IsNull)
			{
				return false;
			}

			PrepareForUpdate( updateTick );

			nextState.Set( state || nextState.State );

			return state;
		}


		public bool UpdateWithValue( float value, ulong updateTick, float deltaTime )
		{
			if (IsNull)
			{
				return false;
			}

			PrepareForUpdate( updateTick );

			if (Utility.Abs( value ) > Utility.Abs( nextState.RawValue ))
			{
				nextState.RawValue = value;

				if (!Raw)
				{
					value = Utility.ApplyDeadZone( value, lowerDeadZone, upperDeadZone );
					//value = Utility.ApplySmoothing( value, lastState.Value, deltaTime, sensitivity );
				}

				nextState.Set( value, stateThreshold );

				return true;
			}

			return false;
		}


		internal bool UpdateWithRawValue( float value, ulong updateTick, float deltaTime )
		{
			if (IsNull)
			{
				return false;
			}

			Raw = true;

			PrepareForUpdate( updateTick );

			if (Utility.Abs( value ) > Utility.Abs( nextState.RawValue ))
			{
				nextState.RawValue = value;
				nextState.Set( value, stateThreshold );
				return true;
			}

			return false;
		}


		internal void SetValue( float value, ulong updateTick )
		{
			if (IsNull)
			{
				return;
			}

			if (updateTick > pendingTick)
			{
				lastState = thisState;
				nextState.Reset();
				pendingTick = updateTick;
				pendingCommit = true;
			}

			nextState.RawValue = value;
			nextState.Set( value, StateThreshold );
		}


		public void ClearInputState()
		{
			lastState.Reset();
			thisState.Reset();
			nextState.Reset();
			wasRepeated = false;
			clearInputState = true;
		}


		public void Commit()
		{
			if (IsNull)
			{
				return;
			}

			pendingCommit = false;
			// nextState.Set( Utility.ApplySmoothing( nextState.Value, lastState.Value, Time.deltaTime, sensitivity ), stateThreshold );
			thisState = nextState;

			if (clearInputState)
			{
				// The net result here should be that the entire state will return zero/false
				// from when ResetState() is called until the next call to Commit(), which is
				// the next update tick, and WasPressed, WasReleased and WasRepeated will then
				// return false during this following tick.
				lastState = nextState;
				UpdateTick = pendingTick;
				clearInputState = false;
				return;
			}

			var lastPressed = lastState.State;
			var thisPressed = thisState.State;

			wasRepeated = false;
			if (lastPressed && !thisPressed) // if was released...
			{
				nextRepeatTime = 0.0f;
			}
			else
			if (thisPressed) // if is pressed...
			{
				if (lastPressed != thisPressed) // if has changed...
				{
					nextRepeatTime = Time.realtimeSinceStartup + FirstRepeatDelay;
				}
				else
				if (Time.realtimeSinceStartup >= nextRepeatTime)
				{
					wasRepeated = true;
					nextRepeatTime = Time.realtimeSinceStartup + RepeatDelay;
				}
			}

			if (thisState != lastState)
			{
				UpdateTick = pendingTick;
			}
		}


		public void CommitWithState( bool state, ulong updateTick, float deltaTime )
		{
			UpdateWithState( state, updateTick, deltaTime );
			Commit();
		}


		public void CommitWithValue( float value, ulong updateTick, float deltaTime )
		{
			UpdateWithValue( value, updateTick, deltaTime );
			Commit();
		}


		internal void CommitWithSides( InputControl negativeSide, InputControl positiveSide, ulong updateTick, float deltaTime )
		{
			LowerDeadZone = Mathf.Max( negativeSide.LowerDeadZone, positiveSide.LowerDeadZone );
			UpperDeadZone = Mathf.Min( negativeSide.UpperDeadZone, positiveSide.UpperDeadZone );
			Raw = negativeSide.Raw || positiveSide.Raw;
			var value = Utility.ValueFromSides( negativeSide.RawValue, positiveSide.RawValue );
			CommitWithValue( value, updateTick, deltaTime );
		}


		public bool State
		{
			get
			{
				return Enabled && thisState.State;
			}
		}


		public bool LastState
		{
			get
			{
				return Enabled && lastState.State;
			}
		}


		public float Value
		{
			get
			{
				return Enabled ? thisState.Value : 0.0f;
			}
		}


		public float LastValue
		{
			get
			{
				return Enabled ? lastState.Value : 0.0f;
			}
		}


		public float RawValue
		{
			get
			{
				return Enabled ? thisState.RawValue : 0.0f;
			}
		}


		internal float NextRawValue
		{
			get
			{
				return Enabled ? nextState.RawValue : 0.0f;
			}
		}


		public bool HasChanged
		{
			get
			{
				return Enabled && thisState != lastState;
			}
		}


		public bool IsPressed
		{
			get
			{
				return Enabled && thisState.State;
			}
		}


		public bool WasPressed
		{
			get
			{
				return Enabled && thisState && !lastState;
			}
		}


		public bool WasReleased
		{
			get
			{
				return Enabled && !thisState && lastState;
			}
		}


		public bool WasRepeated
		{
			get
			{
				return Enabled && wasRepeated;
			}
		}


		public float Sensitivity
		{
			get
			{
				return sensitivity;
			}

			set
			{
				sensitivity = Mathf.Clamp01( value );
			}
		}


		public float LowerDeadZone
		{
			get
			{
				return lowerDeadZone;
			}

			set
			{
				lowerDeadZone = Mathf.Clamp01( value );
			}
		}


		public float UpperDeadZone
		{
			get
			{
				return upperDeadZone;
			}

			set
			{
				upperDeadZone = Mathf.Clamp01( value );
			}
		}


		public float StateThreshold
		{
			get
			{
				return stateThreshold;
			}

			set
			{
				stateThreshold = Mathf.Clamp01( value );
			}
		}


		public bool IsNull
		{
			get
			{
				return ReferenceEquals( this, InputControl.Null );
			}
		}


		public static implicit operator bool( OneAxisInputControl instance )
		{
			return instance.State;
		}


		public static implicit operator float( OneAxisInputControl instance )
		{
			return instance.Value;
		}
	}
}
