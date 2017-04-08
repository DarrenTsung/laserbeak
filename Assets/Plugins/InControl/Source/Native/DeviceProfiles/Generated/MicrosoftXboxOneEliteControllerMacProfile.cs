namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MicrosoftXboxOneEliteControllerMacProfile : XboxOneDriverMacProfile
	{
		public MicrosoftXboxOneEliteControllerMacProfile()
		{
			Name = "Microsoft Xbox One Elite Controller";
			Meta = "Microsoft Xbox One Elite Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x02e3,
				},
			};
		}
	}
	// @endcond
}


