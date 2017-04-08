namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RazerOnzaTEControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerOnzaTEControllerMacProfile()
		{
			Name = "Razer Onza TE Controller";
			Meta = "Razer Onza TE Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xfd00,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1689,
					ProductID = 0xfd00,
				},
			};
		}
	}
	// @endcond
}


