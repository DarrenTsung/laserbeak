namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzControllerMacProfile()
		{
			Name = "Mad Catz Controller";
			Meta = "Mad Catz Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4716,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf902,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf0ca,
				},
			};
		}
	}
	// @endcond
}


