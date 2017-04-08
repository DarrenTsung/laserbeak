namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechG920RacingWheelMacProfile : Xbox360DriverMacProfile
	{
		public LogitechG920RacingWheelMacProfile()
		{
			Name = "Logitech G920 Racing Wheel";
			Meta = "Logitech G920 Racing Wheel on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xc261,
				},
			};
		}
	}
	// @endcond
}


