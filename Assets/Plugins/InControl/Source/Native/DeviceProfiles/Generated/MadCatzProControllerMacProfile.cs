namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzProControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzProControllerMacProfile()
		{
			Name = "Mad Catz Pro Controller";
			Meta = "Mad Catz Pro Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4726,
				},
			};
		}
	}
	// @endcond
}


