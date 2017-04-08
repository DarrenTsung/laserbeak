namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class SaitekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public SaitekXbox360ControllerMacProfile()
		{
			Name = "Saitek Xbox 360 Controller";
			Meta = "Saitek Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0xcb02,
				},
			};
		}
	}
	// @endcond
}


