namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RedOctaneControllerMacProfile : Xbox360DriverMacProfile
	{
		public RedOctaneControllerMacProfile()
		{
			Name = "Red Octane Controller";
			Meta = "Red Octane Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1430,
					ProductID = 0xf801,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1430,
					ProductID = 0x02a0,
				},
			};
		}
	}
	// @endcond
}


