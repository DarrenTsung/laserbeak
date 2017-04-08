namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class GuitarHeroControllerMacProfile : Xbox360DriverMacProfile
	{
		public GuitarHeroControllerMacProfile()
		{
			Name = "Guitar Hero Controller";
			Meta = "Guitar Hero Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1430,
					ProductID = 0x4748,
				},
			};
		}
	}
	// @endcond
}


