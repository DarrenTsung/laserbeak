namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzSF4FightStickSEMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzSF4FightStickSEMacProfile()
		{
			Name = "Mad Catz SF4 Fight Stick SE";
			Meta = "Mad Catz SF4 Fight Stick SE on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4718,
				},
			};
		}
	}
	// @endcond
}


