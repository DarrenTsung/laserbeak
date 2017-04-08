namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzSF4FightStickRound2TEMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzSF4FightStickRound2TEMacProfile()
		{
			Name = "Mad Catz SF4 Fight Stick Round 2 TE";
			Meta = "Mad Catz SF4 Fight Stick Round 2 TE on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf038,
				},
			};
		}
	}
	// @endcond
}


