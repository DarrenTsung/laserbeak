namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class JoytekXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public JoytekXbox360ControllerMacProfile()
		{
			Name = "Joytek Xbox 360 Controller";
			Meta = "Joytek Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x162e,
					ProductID = 0xbeef,
				},
			};
		}
	}
	// @endcond
}


