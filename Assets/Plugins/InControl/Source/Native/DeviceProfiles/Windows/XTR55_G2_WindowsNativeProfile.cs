namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class XTR55_G2_WindowsNativeProfile : NativeInputDeviceProfile
	{
		public XTR55_G2_WindowsNativeProfile()
		{
			Name = "SAILI Simulator XTR5.5 G2 FMS Controller";
			Meta = "SAILI Simulator XTR5.5 G2 FMS Controller on Windows";

			DeviceClass = InputDeviceClass.Controller;

			IncludePlatforms = new[] {
				"Windows"
			};


			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0xb9b,
					ProductID = 0x4012,
					NameLiterals = new [] {
						"SAILI Simulator --- XTR5.5+G2+FMS Controller"
					}
				},
			};

			/*
			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "Left Stick Up",
					Target = InputControlType.LeftStickUp,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Down",
					Target = InputControlType.LeftStickDown,
					Source = Analog( 1 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToMinusOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Left",
					Target = InputControlType.LeftStickLeft,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToMinusOne,
				},
				new InputControlMapping {
					Handle = "Left Stick Right",
					Target = InputControlType.LeftStickRight,
					Source = Analog( 0 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Up",
					Target = InputControlType.RightStickUp,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Down",
					Target = InputControlType.RightStickDown,
					Source = Analog( 4 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToMinusOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Left",
					Target = InputControlType.RightStickLeft,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToMinusOne,
				},
				new InputControlMapping {
					Handle = "Right Stick Right",
					Target = InputControlType.RightStickRight,
					Source = Analog( 5 ),
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne,
				}
			};
			*/
		}
	}
	// @endcond
}

