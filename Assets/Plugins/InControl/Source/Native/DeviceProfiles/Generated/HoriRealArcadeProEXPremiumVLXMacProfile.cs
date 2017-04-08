namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriRealArcadeProEXPremiumVLXMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProEXPremiumVLXMacProfile()
		{
			Name = "Hori Real Arcade Pro EX Premium VLX";
			Meta = "Hori Real Arcade Pro EX Premium VLX on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf506,
				},
			};
		}
	}
	// @endcond
}


