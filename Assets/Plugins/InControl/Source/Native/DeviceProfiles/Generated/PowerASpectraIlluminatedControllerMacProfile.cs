namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PowerASpectraIlluminatedControllerMacProfile : Xbox360DriverMacProfile
	{
		public PowerASpectraIlluminatedControllerMacProfile()
		{
			Name = "PowerA Spectra Illuminated Controller";
			Meta = "PowerA Spectra Illuminated Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x542a,
				},
			};
		}
	}
	// @endcond
}


