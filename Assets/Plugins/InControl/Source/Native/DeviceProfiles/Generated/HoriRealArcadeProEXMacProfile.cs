namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriRealArcadeProEXMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProEXMacProfile()
		{
			Name = "Hori Real Arcade Pro EX";
			Meta = "Hori Real Arcade Pro EX on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf504,
				},
			};
		}
	}
	// @endcond
}


