namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RockCandyXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public RockCandyXbox360ControllerMacProfile()
		{
			Name = "Rock Candy Xbox 360 Controller";
			Meta = "Rock Candy Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x021f,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0xfafe,
				},
			};
		}
	}
	// @endcond
}


