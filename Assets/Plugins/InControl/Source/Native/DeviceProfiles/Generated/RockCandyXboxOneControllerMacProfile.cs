namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RockCandyXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public RockCandyXboxOneControllerMacProfile()
		{
			Name = "Rock Candy Xbox One Controller";
			Meta = "Rock Candy Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0146,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0246,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0346,
				},
			};
		}
	}
	// @endcond
}


