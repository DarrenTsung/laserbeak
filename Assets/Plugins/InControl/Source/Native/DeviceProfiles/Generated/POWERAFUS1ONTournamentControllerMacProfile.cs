namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class POWERAFUS1ONTournamentControllerMacProfile : Xbox360DriverMacProfile
	{
		public POWERAFUS1ONTournamentControllerMacProfile()
		{
			Name = "POWER A FUS1ON Tournament Controller";
			Meta = "POWER A FUS1ON Tournament Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5397,
				},
			};
		}
	}
	// @endcond
}


