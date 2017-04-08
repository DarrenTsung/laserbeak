namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class BETAOPControllerMacProfile : Xbox360DriverMacProfile
	{
		public BETAOPControllerMacProfile()
		{
			Name = "BETAOP Controller";
			Meta = "BETAOP Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x11c0,
					ProductID = 0x5506,
				},
			};
		}
	}
	// @endcond
}


