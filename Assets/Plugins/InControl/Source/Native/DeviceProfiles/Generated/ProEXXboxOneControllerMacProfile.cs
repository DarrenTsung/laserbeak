namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class ProEXXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public ProEXXboxOneControllerMacProfile()
		{
			Name = "Pro EX Xbox One Controller";
			Meta = "Pro EX Xbox One Controller on Mac";

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


