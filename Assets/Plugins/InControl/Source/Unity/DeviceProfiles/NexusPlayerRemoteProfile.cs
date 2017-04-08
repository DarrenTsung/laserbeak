namespace InControl
{
	// @cond nodoc
	[AutoDiscover]
	public class NexusPlayerRemoteProfile : UnityInputDeviceProfile
	{
		public NexusPlayerRemoteProfile()
		{
			Name = "Nexus Player Remote";
			Meta = "Nexus Player Remote";

			DeviceClass = InputDeviceClass.Remote;

			IncludePlatforms = new[] {
				"Android"
			};

			JoystickNames = new[] {
				"Google Nexus Remote"
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
