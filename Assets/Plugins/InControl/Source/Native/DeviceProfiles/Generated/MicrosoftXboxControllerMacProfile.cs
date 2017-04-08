namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MicrosoftXboxControllerMacProfile : Xbox360DriverMacProfile
	{
		public MicrosoftXboxControllerMacProfile()
		{
			Name = "Microsoft Xbox Controller";
			Meta = "Microsoft Xbox Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0xffff,
					ProductID = 0xffff,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0289,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0288,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0285,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0202,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0287,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x0288,
				},
			};
		}
	}
	// @endcond
}


