namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriRealArcadeProHayabusaMacProfile : Xbox360DriverMacProfile
	{
		public HoriRealArcadeProHayabusaMacProfile()
		{
			Name = "Hori Real Arcade Pro Hayabusa";
			Meta = "Hori Real Arcade Pro Hayabusa on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x0063,
				},
			};
		}
	}
	// @endcond
}


