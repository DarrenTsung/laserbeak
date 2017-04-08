namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class DroidBoxPS3AndroidProfile : UnityInputDeviceProfile
	{
		// https://www.amazon.com/DroidBOX-Gamepad-Handheld-Touchscreen-Mali-T764-Linux/dp/B01B4ESXT0
		// https://droidbox.co.uk/gpd-xd-droidbox-playon-best-mini-handheld-games-tablet-console-android.html
		// This controller is capable of switching between PS3 and Xbox modes.
		public DroidBoxPS3AndroidProfile()
		{
			Name = "DroidBOX GPD XD PlayON - PS3 Mode";
			Meta = "DroidBOX GPD XD PlayON - PS3 Mode on Android";

			DeviceClass = InputDeviceClass.Controller;

			IncludePlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"PLAYSTATION(R)3",
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "Cross",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Circle",
					Target = InputControlType.Action2,
					Source = Button1
				},
				new InputControlMapping {
					Handle = "Square",
					Target = InputControlType.Action3,
					Source = Button2
				},
				new InputControlMapping {
					Handle = "Triangle",
					Target = InputControlType.Action4,
					Source = Button3
				},
				new InputControlMapping {
					Handle = "Start",
					Target = InputControlType.Start,
					Source = Button10
				},
				new InputControlMapping {
					Handle = "Select",
					Target = InputControlType.Select,
					Source = Button11
				},
				new InputControlMapping {
					Handle = "L1",
					Target = InputControlType.LeftBumper,
					Source = Button4
				},
				new InputControlMapping {
					Handle = "L2",
					Target = InputControlType.LeftTrigger,
					Source = Button6
				},
				new InputControlMapping {
					Handle = "L3",
					Target = InputControlType.LeftStickButton,
					Source = Button8
				},
				new InputControlMapping {
					Handle = "R1",
					Target = InputControlType.RightBumper,
					Source = Button5
				},
				new InputControlMapping {
					Handle = "R2",
					Target = InputControlType.RightTrigger,
					Source = Button7
				},
				new InputControlMapping {
					Handle = "R3",
					Target = InputControlType.RightStickButton,
					Source = Button9
				},
			};

			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),

				RightStickLeftMapping( Analog2 ),
				RightStickRightMapping( Analog2 ),
				RightStickUpMapping( Analog3 ),
				RightStickDownMapping( Analog3 ),

				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}
	// @endcond
}

