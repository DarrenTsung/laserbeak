namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzFightPadControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzFightPadControllerMacProfile()
		{
			Name = "Mad Catz FightPad Controller";
			Meta = "Mad Catz FightPad Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf028,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4728,
				},
			};
		}
	}
	// @endcond
}


