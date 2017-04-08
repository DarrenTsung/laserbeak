namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriDOA4ArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		public HoriDOA4ArcadeStickMacProfile()
		{
			Name = "Hori DOA4 Arcade Stick";
			Meta = "Hori DOA4 Arcade Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x000a,
				},
			};
		}
	}
	// @endcond
}


