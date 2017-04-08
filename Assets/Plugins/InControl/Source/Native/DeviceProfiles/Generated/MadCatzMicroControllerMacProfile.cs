namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzMicroControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzMicroControllerMacProfile()
		{
			Name = "Mad Catz Micro Controller";
			Meta = "Mad Catz Micro Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0738,
					ProductID = 0x4736,
				},
			};
		}
	}
	// @endcond
}


