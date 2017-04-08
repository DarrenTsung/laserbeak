namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MicrosoftXbox360ControllerMacProfile : Xbox360DriverMacProfile
	{
		public MicrosoftXbox360ControllerMacProfile()
		{
			Name = "Microsoft Xbox 360 Controller";
			Meta = "Microsoft Xbox 360 Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x028e,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x028f,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x02a0,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x045e,
					ProductID = 0x02a0,
				},
			};
		}
	}
	// @endcond
}


