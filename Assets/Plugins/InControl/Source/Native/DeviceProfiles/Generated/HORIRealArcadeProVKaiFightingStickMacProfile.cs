namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HORIRealArcadeProVKaiFightingStickMacProfile : Xbox360DriverMacProfile
	{
		public HORIRealArcadeProVKaiFightingStickMacProfile()
		{
			Name = "HORI Real Arcade Pro V Kai Fighting Stick";
			Meta = "HORI Real Arcade Pro V Kai Fighting Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x550e,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x0078,
				},
			};
		}
	}
	// @endcond
}


