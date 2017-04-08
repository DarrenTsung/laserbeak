namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class EASportsControllerMacProfile : Xbox360DriverMacProfile
	{
		public EASportsControllerMacProfile()
		{
			Name = "EA Sports Controller";
			Meta = "EA Sports Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0131,
				},
			};
		}
	}
	// @endcond
}


