namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriFightingStickEX2MacProfile : Xbox360DriverMacProfile
	{
		public HoriFightingStickEX2MacProfile()
		{
			Name = "Hori Fighting Stick EX2";
			Meta = "Hori Fighting Stick EX2 on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x000a,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf505,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x000d,
				},
			};
		}
	}
	// @endcond
}


