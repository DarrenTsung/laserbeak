namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class GameStopControllerMacProfile : Xbox360DriverMacProfile
	{
		public GameStopControllerMacProfile()
		{
			Name = "GameStop Controller";
			Meta = "GameStop Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0401,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0301,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x12ab,
					ProductID = 0x0302,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf901,
				},
			};
		}
	}
	// @endcond
}


