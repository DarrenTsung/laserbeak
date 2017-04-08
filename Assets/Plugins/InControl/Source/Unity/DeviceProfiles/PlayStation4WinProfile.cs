namespace InControl
{
	// This profile works for connection over USB ONLY.
	// Unfortunately, supporting the profiles for USB and bluetooth
	// are mutually exclusive. I picked USB since getting the controller
	// to pair properly over bluetooth is very flaky.
	// The alternate mappings are commented out below.
	//
	// @cond nodoc
	[AutoDiscover]
	public class PlayStation4WinProfile : UnityInputDeviceProfile
	{
		public PlayStation4WinProfile()
		{
			string RegistrationMark = "\u00AE";

			Name = "PlayStation 4 Controller";
			Meta = "PlayStation 4 Controller on Windows";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.PlayStation4;

			IncludePlatforms = new[] {
				"Windows"
			};

			JoystickNames = new[] {
				"Wireless Controller",
				"DUALSHOCK" + RegistrationMark + "4 USB Wireless Adaptor"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Left Bumper",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "Right Bumper",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "Share",
					Target = InputControlType.Share,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "Options",
					Target = InputControlType.Options,
					Source = Button9
				},
				new InputControlMapping {
					Handle = "Left Stick Button",
					Target = InputControlType.LeftStickButton,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Right Stick Button",
					Target = InputControlType.RightStickButton,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "System",
					Target = InputControlType.System,
					Source = Button12
				},
				new InputControlMapping {
					Handle = "TouchPad Button",
					Target = InputControlType.TouchPadButton,
					Source = Button13
				},
			};

			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),

				RightStickLeftMapping( Analog2 ),
				RightStickRightMapping( Analog2 ),
				RightStickUpMapping( Analog5 ),
				RightStickDownMapping( Analog5 ),

				DPadLeftMapping( Analog6 ),
				DPadRightMapping( Analog6 ),
				DPadUpMapping2( Analog7 ),
				DPadDownMapping2( Analog7 ),

				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog3,
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog4,
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
			};

			/*
			 * These are the alternate bluetooth mappings.
			 * 
			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog2 ),
				LeftStickDownMapping( Analog2 ),

				RightStickLeftMapping( Analog3 ),
				RightStickRightMapping( Analog3 ),
				RightStickUpMapping( Analog6 ),
				RightStickDownMapping( Analog6 ),

				DPadLeftMapping( Analog7 ),
				DPadRightMapping( Analog7 ),
				DPadUpMapping2( Analog8 ),
				DPadDownMapping2( Analog8 ),

				new InputControlMapping {
					Handle = "Left Trigger",
					Target = InputControlType.LeftTrigger,
					Source = Analog4,
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
				new InputControlMapping {
					Handle = "Right Trigger",
					Target = InputControlType.RightTrigger,
					Source = Analog5,
					SourceRange = InputRange.MinusOneToOne,
					TargetRange = InputRange.ZeroToOne,
					IgnoreInitialZeroValue = true
				},
			};
			*/
		}
	}
	// @endcond
}

