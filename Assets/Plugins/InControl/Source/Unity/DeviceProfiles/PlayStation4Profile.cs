namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class PlayStation4Profile : UnityInputDeviceProfile
	{
		public PlayStation4Profile()
		{
			string RegistrationMark = "\u00AE";

			Name = "DUALSHOCK" + RegistrationMark + "4 wireless controller";
			Meta = "DUALSHOCK" + RegistrationMark + "4 wireless controller on PlayStation" + RegistrationMark + "4 system";

			DeviceClass = InputDeviceClass.Controller;
			DeviceStyle = InputDeviceStyle.PlayStation4;

			IncludePlatforms = new[] {
				"PS4"
			};

			JoystickRegex = new[] {
				"controller"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "cross button",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "circle button",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "square button",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "triangle button",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "L1 button",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "R1 button",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "touch pad button",
					Target = InputControlType.TouchPadButton,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "OPTIONS button",
					Target = InputControlType.Options,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "L3 button",
					Target = InputControlType.LeftStickButton,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "R3 button",
					Target = InputControlType.RightStickButton,
					Source = Button9
				}
			};

			AnalogMappings = new[] {
				new InputControlMapping {
					Handle = "left stick left",
					Target = InputControlType.LeftStickLeft,
					Source = Analog0,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "left stick right",
					Target = InputControlType.LeftStickRight,
					Source = Analog0,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "left stick up",
					Target = InputControlType.LeftStickUp,
					Source = Analog1,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "left stick down",
					Target = InputControlType.LeftStickDown,
					Source = Analog1,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},

				new InputControlMapping {
					Handle = "right stick left",
					Target = InputControlType.RightStickLeft,
					Source = Analog3,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "right stick right",
					Target = InputControlType.RightStickRight,
					Source = Analog3,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "right stick up",
					Target = InputControlType.RightStickUp,
					Source = Analog4,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "right stick down",
					Target = InputControlType.RightStickDown,
					Source = Analog4,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},

				new InputControlMapping {
					Handle = "left button",
					Target = InputControlType.DPadLeft,
					Source = Analog5,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "right button",
					Target = InputControlType.DPadRight,
					Source = Analog5,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "up button",
					Target = InputControlType.DPadUp,
					Source = Analog6,
					SourceRange = InputRange.ZeroToOne,
					TargetRange = InputRange.ZeroToOne
				},
				new InputControlMapping {
					Handle = "down button",
					Target = InputControlType.DPadDown,
					Source = Analog6,
					SourceRange = InputRange.ZeroToMinusOne,
					TargetRange = InputRange.ZeroToOne
				},

				new InputControlMapping {
					Handle = "L2 button",
					Target = InputControlType.LeftTrigger,
					Source = Analog7,
				},
				new InputControlMapping {
					Handle = "R2 button",
					Target = InputControlType.RightTrigger,
					Source = Analog2,
					Invert = true
				},
			};
		}
	}
	// @endcond
}
