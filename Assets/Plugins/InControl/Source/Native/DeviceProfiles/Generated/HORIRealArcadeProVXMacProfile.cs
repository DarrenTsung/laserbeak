namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HORIRealArcadeProVXMacProfile : Xbox360DriverMacProfile
	{
		public HORIRealArcadeProVXMacProfile()
		{
			Name = "HORI Real Arcade Pro VX";
			Meta = "HORI Real Arcade Pro VX on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x001b,
				},
			};
		}
	}
	// @endcond
}


