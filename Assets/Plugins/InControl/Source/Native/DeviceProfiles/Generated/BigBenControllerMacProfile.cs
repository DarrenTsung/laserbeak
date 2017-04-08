namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class BigBenControllerMacProfile : Xbox360DriverMacProfile
	{
		public BigBenControllerMacProfile()
		{
			Name = "Big Ben Controller";
			Meta = "Big Ben Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x146b,
					ProductID = 0x0601,
				},
			};
		}
	}
	// @endcond
}


