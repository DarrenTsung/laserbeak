namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzBrawlStickMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzBrawlStickMacProfile()
		{
			Name = "Mad Catz Brawl Stick";
			Meta = "Mad Catz Brawl Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf019,
				},
			};
		}
	}
	// @endcond
}


