namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriRealArcadeProEXSEMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProEXSEMacProfile()
		{
			Name = "Hori Real Arcade Pro EX SE";
			Meta = "Hori Real Arcade Pro EX SE on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x0016,
				},
			};
		}
	}
	// @endcond
}


