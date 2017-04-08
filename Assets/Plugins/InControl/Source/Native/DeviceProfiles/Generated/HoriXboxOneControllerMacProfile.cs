namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public HoriXboxOneControllerMacProfile()
		{
			Name = "Hori Xbox One Controller";
			Meta = "Hori Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0f0d,
					ProductID = 0x0067,
				},
			};
		}
	}
	// @endcond
}


