namespace InControl
{
	public abstract class NativeInputDeviceProfile : InputDeviceProfile
	{
		public NativeInputDeviceMatcher[] Matchers;
		public NativeInputDeviceMatcher[] LastResortMatchers;


		public NativeInputDeviceProfile()
		{
			Sensitivity = 1.0f;
			LowerDeadZone = 0.2f;
			UpperDeadZone = 0.9f;
		}


		internal bool Matches( NativeDeviceInfo deviceInfo )
		{
			return Matches( deviceInfo, Matchers );
		}


		internal bool LastResortMatches( NativeDeviceInfo deviceInfo )
		{
			return Matches( deviceInfo, LastResortMatchers );
		}


		bool Matches( NativeDeviceInfo deviceInfo, NativeInputDeviceMatcher[] matchers )
		{
			if (Matchers != null)
			{
				var matchersCount = Matchers.Length;
				for (var i = 0; i < matchersCount; i++)
				{
					if (Matchers[i].Matches( deviceInfo ))
					{
						return true;
					}
				}
			}
			return false;
		}


		#region InputControlSource helpers

		protected static InputControlSource Button( int index )
		{
			return new NativeButtonSource( index );
		}

		protected static InputControlSource Analog( int index )
		{
			return new NativeAnalogSource( index );
		}

		#endregion


		#region InputDeviceMapping helpers

		protected static InputControlMapping LeftStickLeftMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Left",
				Target = InputControlType.LeftStickLeft,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftStickRightMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Right",
				Target = InputControlType.LeftStickRight,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftStickUpMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftStickDownMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftStickUpMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Up",
				Target = InputControlType.LeftStickUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftStickDownMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Stick Down",
				Target = InputControlType.LeftStickDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickLeftMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Left",
				Target = InputControlType.RightStickLeft,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickRightMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Right",
				Target = InputControlType.RightStickRight,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickUpMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickDownMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickUpMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Up",
				Target = InputControlType.RightStickUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping RightStickDownMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Stick Down",
				Target = InputControlType.RightStickDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping LeftTriggerMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Left Trigger",
				Target = InputControlType.LeftTrigger,
				Source = Analog( analog ),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		protected static InputControlMapping RightTriggerMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "Right Trigger",
				Target = InputControlType.RightTrigger,
				Source = Analog( analog ),
				SourceRange = InputRange.MinusOneToOne,
				TargetRange = InputRange.ZeroToOne,
				IgnoreInitialZeroValue = true
			};
		}

		protected static InputControlMapping DPadLeftMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Left",
				Target = InputControlType.DPadLeft,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping DPadRightMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Right",
				Target = InputControlType.DPadRight,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping DPadUpMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping DPadDownMapping( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping DPadUpMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Up",
				Target = InputControlType.DPadUp,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		protected static InputControlMapping DPadDownMapping2( int analog )
		{
			return new InputControlMapping {
				Handle = "DPad Down",
				Target = InputControlType.DPadDown,
				Source = Analog( analog ),
				SourceRange = InputRange.ZeroToMinusOne,
				TargetRange = InputRange.ZeroToOne
			};
		}

		#endregion
	}
}

