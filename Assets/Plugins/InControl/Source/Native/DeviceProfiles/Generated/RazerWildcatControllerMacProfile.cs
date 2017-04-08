namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RazerWildcatControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerWildcatControllerMacProfile()
		{
			Name = "Razer Wildcat Controller";
			Meta = "Razer Wildcat Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1532,
					ProductID = 0x0a03,
				},
			};
		}
	}
	// @endcond
}


