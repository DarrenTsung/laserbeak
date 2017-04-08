namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class ThrustmasterFerrari458RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public ThrustmasterFerrari458RacingWheelMacProfile()
		{
			Name = "Thrustmaster Ferrari 458 Racing Wheel";
			Meta = "Thrustmaster Ferrari 458 Racing Wheel on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5b00,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5b03,
				},
			};
		}
	}
	// @endcond
}


