namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class NaconGC100XFControllerMacProfile : Xbox360DriverMacProfile
	{
		public NaconGC100XFControllerMacProfile()
		{
			Name = "Nacon GC-100XF Controller";
			Meta = "Nacon GC-100XF Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x11c9,
					ProductID = 0x55f0,
				},
			};
		}
	}
	// @endcond
}


