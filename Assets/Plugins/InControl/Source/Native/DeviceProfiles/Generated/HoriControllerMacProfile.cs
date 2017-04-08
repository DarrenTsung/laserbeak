namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriControllerMacProfile : Xbox360DriverMacProfile
	{
		public HoriControllerMacProfile()
		{
			Name = "Hori Controller";
			Meta = "Hori Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0x5500,
				},
			};
		}
	}
	// @endcond
}


