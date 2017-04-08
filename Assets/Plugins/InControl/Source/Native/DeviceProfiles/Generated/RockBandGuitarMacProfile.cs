namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RockBandGuitarMacProfile : Xbox360DriverMacProfile
	{
		public RockBandGuitarMacProfile()
		{
			Name = "Rock Band Guitar";
			Meta = "Rock Band Guitar on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0x0002,
				},
			};
		}
	}
	// @endcond
}


