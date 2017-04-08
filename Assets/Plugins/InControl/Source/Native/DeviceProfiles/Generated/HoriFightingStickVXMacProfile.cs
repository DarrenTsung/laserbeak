namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriFightingStickVXMacProfile : Xbox360DriverMacProfile
	{
		public HoriFightingStickVXMacProfile()
		{
			Name = "Hori Fighting Stick VX";
			Meta = "Hori Fighting Stick VX on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf503,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5502,
				},
			};
		}
	}
	// @endcond
}


