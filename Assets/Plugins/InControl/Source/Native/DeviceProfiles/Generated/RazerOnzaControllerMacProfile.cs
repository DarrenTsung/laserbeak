namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RazerOnzaControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerOnzaControllerMacProfile()
		{
			Name = "Razer Onza Controller";
			Meta = "Razer Onza Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xfd01,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1689,
					ProductID = 0xfd01,
				},
			};
		}
	}
	// @endcond
}


