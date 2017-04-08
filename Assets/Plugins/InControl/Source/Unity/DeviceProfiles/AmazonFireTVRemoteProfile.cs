namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class AmazonFireTVRemoteProfile : UnityInputDeviceProfile
	{
		public AmazonFireTVRemoteProfile()
		{
			Name = "Amazon Fire TV Remote";
			Meta = "Amazon Fire TV Remote on Amazon Fire TV";

			DeviceClass = InputDeviceClass.Remote;
			DeviceStyle = InputDeviceStyle.AmazonFireTV;

			IncludePlatforms = new[] {
				"Amazon AFT",
			};

			JoystickNames = new[] {
				"",
				"Amazon Fire TV Remote"
			};

			ButtonMappings = new[] {
				new InputControlMapping {
					Handle = "A",
					Target = InputControlType.Action1,
					Source = Button0
				},
				new InputControlMapping {
					Handle = "Back",
					Target = InputControlType.Back,
					Source = EscapeKey
				},
				new InputControlMapping {
					Handle = "Menu",
					Target = InputControlType.Menu,
					Source = MenuKey
				}
			};

			AnalogMappings = new[] {
				DPadLeftMapping( Analog4 ),
				DPadRightMapping( Analog4 ),
				DPadUpMapping( Analog5 ),
				DPadDownMapping( Analog5 ),
			};
		}
	}
	// @endcond
}

