namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MicrosoftXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public MicrosoftXboxOneControllerMacProfile()
		{
			Name = "Microsoft Xbox One Controller";
			Meta = "Microsoft Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x02d1,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x02dd,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x02ea,
				},
			};
		}
	}
	// @endcond
}


