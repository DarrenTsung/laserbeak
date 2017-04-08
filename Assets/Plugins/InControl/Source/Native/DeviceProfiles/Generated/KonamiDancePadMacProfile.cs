namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class KonamiDancePadMacProfile : Xbox360DriverMacProfile
	{
		public KonamiDancePadMacProfile()
		{
			Name = "Konami Dance Pad";
			Meta = "Konami Dance Pad on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x12ab,
					ProductID = 0x0004,
				},
			};
		}
	}
	// @endcond
}


