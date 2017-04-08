namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzFightPadMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzFightPadMacProfile()
		{
			Name = "Mad Catz FightPad";
			Meta = "Mad Catz FightPad on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf02e,
				},
			};
		}
	}
	// @endcond
}


