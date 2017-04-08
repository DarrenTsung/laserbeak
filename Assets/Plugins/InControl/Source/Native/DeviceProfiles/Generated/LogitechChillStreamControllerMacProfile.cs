namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class LogitechChillStreamControllerMacProfile : Xbox360DriverMacProfile
	{
		public LogitechChillStreamControllerMacProfile()
		{
			Name = "Logitech Chill Stream Controller";
			Meta = "Logitech Chill Stream Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x046d,
					ProductID = 0xc242,
				},
			};
		}
	}
	// @endcond
}


