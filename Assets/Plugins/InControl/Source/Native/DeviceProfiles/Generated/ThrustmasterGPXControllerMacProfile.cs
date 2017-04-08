namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class ThrustmasterGPXControllerMacProfile : Xbox360DriverMacProfile
	{
		public ThrustmasterGPXControllerMacProfile()
		{
			Name = "Thrustmaster GPX Controller";
			Meta = "Thrustmaster GPX Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x044f,
					ProductID = 0xb326,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5b02,
				},
			};
		}
	}
	// @endcond
}


