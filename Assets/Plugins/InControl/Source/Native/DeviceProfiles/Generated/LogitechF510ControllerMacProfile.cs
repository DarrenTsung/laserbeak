namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechF510ControllerMacProfile : Xbox360DriverMacProfile
	{
		public LogitechF510ControllerMacProfile()
		{
			Name = "Logitech F510 Controller";
			Meta = "Logitech F510 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xc21e,
				},
			};
		}
	}
	// @endcond
}


