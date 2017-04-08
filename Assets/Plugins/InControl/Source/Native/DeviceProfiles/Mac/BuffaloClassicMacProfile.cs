namespace InControl.NativeProfile
{
	// @cond nodoc
	public class BuffaloClassicMacProfile : NativeInputDeviceProfile
	{
		public BuffaloClassicMacProfile()
		{
			Name = "iBuffalo Classic Controller";
			Meta = "iBuffalo Classic Controller on Mac";

			DeviceClass = InputDeviceClass.Controller;

			IncludePlatforms = new[] {
				"OS X"
			};

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x583,
					ProductID = 0x2060,
					VersionNumber = 0x100
				},
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "B",
					Target = InputControlType.Action1,
					Source = Button( 1 )
				},
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action2,
					Source = Button( 0 )
				},
				new InputControlMapping {
					Handle = "Y",
					Target = InputControlType.Action3,
					Source = Button( 3 )
				},
				new InputControlMapping {
					Handle = "X",
					Target = InputControlType.Action4,
					Source = Button( 2 )
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button( 4 )
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button( 5 )
				},
				new InputControlMapping {
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button( 6 )
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button( 7 )
				},
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "DPad Left",
					Target = InputControlType.DPadLeft,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "DPad Right",
					Target = InputControlType.DPadRight,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "DPad Up",
					Target = InputControlType.DPadUp,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "DPad Down",
					Target = InputControlType.DPadDown,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
			};
		}
	}
	// @endcond
}

