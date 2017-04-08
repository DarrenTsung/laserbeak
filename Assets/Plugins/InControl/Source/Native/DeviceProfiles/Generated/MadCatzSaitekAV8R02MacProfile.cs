namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzSaitekAV8R02MacProfile : Xbox360DriverMacProfile
	{
		public MadCatzSaitekAV8R02MacProfile()
		{
			Name = "Mad Catz Saitek AV8R02";
			Meta = "Mad Catz Saitek AV8R02 on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0xcb29,
				},
			};
		}
	}
	// @endcond
}


