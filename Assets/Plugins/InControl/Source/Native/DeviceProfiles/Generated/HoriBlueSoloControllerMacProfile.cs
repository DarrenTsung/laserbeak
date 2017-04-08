namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriBlueSoloControllerMacProfile : Xbox360DriverMacProfile
	{
		public HoriBlueSoloControllerMacProfile()
		{
			Name = "Hori Blue Solo Controller ";
			Meta = "Hori Blue Solo Controller	on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xfa01,
				},
			};
		}
	}
	// @endcond
}


