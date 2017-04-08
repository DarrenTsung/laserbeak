namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzArcadeStickMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzArcadeStickMacProfile()
		{
			Name = "Mad Catz Arcade Stick";
			Meta = "Mad Catz Arcade Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4758,
				},
			};
		}
	}
	// @endcond
}


