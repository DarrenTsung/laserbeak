namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PowerAMiniXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PowerAMiniXboxOneControllerMacProfile()
		{
			Name = "Power A Mini Xbox One Controller";
			Meta = "Power A Mini Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x543a,
				},
			};
		}
	}
	// @endcond
}


