namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechF310ControllerMacProfile : Xbox360DriverMacProfile
	{
		public LogitechF310ControllerMacProfile()
		{
			Name = "Logitech F310 Controller";
			Meta = "Logitech F310 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xc21d,
				},
			};
		}
	}
	// @endcond
}


