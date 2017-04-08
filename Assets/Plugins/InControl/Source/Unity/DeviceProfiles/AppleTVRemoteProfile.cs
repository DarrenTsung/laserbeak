namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class AppleTVRemoteProfile : UnityInputDeviceProfile
	{
		public AppleTVRemoteProfile()
		{
			Name = "Apple TV Remote";
			Meta = "Apple TV Remote on tvOS";

			DeviceClass = InputDeviceClass.Remote;
			DeviceStyle = InputDeviceStyle.AppleMFi;

			IncludePlatforms = new[] {
				"AppleTV"
			};

			JoystickRegex = new[] {
				"Remote"
			};

			LowerDeadZone = 0.05f;
			UpperDeadZone = 0.95f;

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "TouchPad Click",
					Target = InputControlType.Action1,
					Source = Button14
				},
				new InputControlMapping {
					Handle = "Play/Pause",
					Target = InputControlType.Action2,
					Source = Button15
				},
				new InputControlMapping {
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = Button0
				},
			};

			AnalogMappings = new[] {
				LeftStickLeftMapping( Analog0 ),
				LeftStickRightMapping( Analog0 ),
				LeftStickUpMapping( Analog1 ),
				LeftStickDownMapping( Analog1 ),

				new InputControlMapping {
					Handle = "TouchPad X",
					Target = InputControlType.TouchPadXAxis,
					Source = Analog0,
					Raw = true
				},
				new InputControlMapping {
					Handle = "TouchPad Y",
					Target = InputControlType.TouchPadYAxis,
					Source = Analog1,
					Raw = true
				},

				new InputControlMapping {
					Handle = "Orientation X",
					Target = InputControlType.TiltX,
					Source = Analog15,
					Passive = true
				},
				new InputControlMapping {
					Handle = "Orientation Y",
					Target = InputControlType.TiltY,
					Source = Analog16,
					Passive = true
				},
				new InputControlMapping {
					Handle = "Orientation Z",
					Target = InputControlType.TiltZ,
					Source = Analog17,
					Passive = true
				},

				new InputControlMapping {
					Handle = "Acceleration X",
					Target = InputControlType.Analog0,
					Source = Analog18,
					Passive = true
				},
				new InputControlMapping {
					Handle = "Acceleration Y",
					Target = InputControlType.Analog1,
					Source = Analog19,
					Passive = true
				},
			};
		}
	}
	// @endcond
}

