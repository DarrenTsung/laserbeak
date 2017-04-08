namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriRealArcadeProVXSAMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProVXSAMacProfile()
		{
			Name = "Hori Real Arcade Pro VX SA";
			Meta = "Hori Real Arcade Pro VX SA on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf502,
				},
			};
		}
	}
	// @endcond
}


