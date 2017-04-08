namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class TSZPelicanControllerMacProfile : Xbox360DriverMacProfile
	{
		public TSZPelicanControllerMacProfile()
		{
			Name = "TSZ Pelican Controller";
			Meta = "TSZ Pelican Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0201,
				},
			};
		}
	}
	// @endcond
}


