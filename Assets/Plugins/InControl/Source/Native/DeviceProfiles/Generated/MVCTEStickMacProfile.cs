namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MVCTEStickMacProfile : Xbox360DriverMacProfile
	{
		public MVCTEStickMacProfile()
		{
			Name = "MVC TE Stick";
			Meta = "MVC TE Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf039,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0xb738,
				},
			};
		}
	}
	// @endcond
}


