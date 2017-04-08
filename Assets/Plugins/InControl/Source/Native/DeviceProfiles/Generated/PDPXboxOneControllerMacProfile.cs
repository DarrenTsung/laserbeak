namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public PDPXboxOneControllerMacProfile()
		{
			Name = "PDP Xbox One Controller";
			Meta = "PDP Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x013a,
				},
			};
		}
	}
	// @endcond
}


