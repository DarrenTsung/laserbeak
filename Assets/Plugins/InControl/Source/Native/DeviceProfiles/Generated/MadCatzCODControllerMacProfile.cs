namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzCODControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzCODControllerMacProfile()
		{
			Name = "Mad Catz COD Controller";
			Meta = "Mad Catz COD Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf025,
				},
			};
		}
	}
	// @endcond
}


