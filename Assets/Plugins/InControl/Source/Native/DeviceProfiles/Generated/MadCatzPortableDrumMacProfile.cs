namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzPortableDrumMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzPortableDrumMacProfile()
		{
			Name = "Mad Catz Portable Drum";
			Meta = "Mad Catz Portable Drum on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x9871,
				},
			};
		}
	}
	// @endcond
}


