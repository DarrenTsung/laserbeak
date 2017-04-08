namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class BatarangControllerMacProfile : Xbox360DriverMacProfile
	{
		public BatarangControllerMacProfile()
		{
			Name = "Batarang Controller";
			Meta = "Batarang Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x15e4,
					ProductID = 0x3f10,
				},
			};
		}
	}
	// @endcond
}


