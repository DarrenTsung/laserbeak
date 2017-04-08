namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechF710ControllerMacProfile : Xbox360DriverMacProfile
	{
		public LogitechF710ControllerMacProfile()
		{
			Name = "Logitech F710 Controller";
			Meta = "Logitech F710 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xc21f,
				},
			};
		}
	}
	// @endcond
}


