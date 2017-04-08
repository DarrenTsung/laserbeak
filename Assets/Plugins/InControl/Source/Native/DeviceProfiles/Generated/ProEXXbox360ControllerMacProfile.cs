namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class ProEXXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public ProEXXbox360ControllerMacProfile()
		{
			Name = "Pro EX Xbox 360 Controller";
			Meta = "Pro EX Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x530a,
				},
			};
		}
	}
	// @endcond
}


