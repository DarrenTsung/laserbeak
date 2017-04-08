namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPAfterglowControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPAfterglowControllerMacProfile()
		{
			Name = "PDP Afterglow Controller";
			Meta = "PDP Afterglow Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0413,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0xfafc,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf907,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0xfafd,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf900,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0113,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0xf900,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0213,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x12ab,
					ProductID = 0x0301,
				},
			};
		}
	}
	// @endcond
}


