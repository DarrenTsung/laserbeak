namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class MadCatzMicroConControllerMacProfile : Xbox360DriverMacProfile
	{
		public MadCatzMicroConControllerMacProfile()
		{
			Name = "Mad Catz MicroCon Controller";
			Meta = "Mad Catz MicroCon Controller on Mac";

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


