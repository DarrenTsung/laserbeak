namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class HoriXbox360GemPadExMacProfile : Xbox360DriverMacProfile
	{
		public HoriXbox360GemPadExMacProfile()
		{
			Name = "Hori Xbox 360 Gem Pad Ex";
			Meta = "Hori Xbox 360 Gem Pad Ex on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x550d,
				},
			};
		}
	}
	// @endcond
}


