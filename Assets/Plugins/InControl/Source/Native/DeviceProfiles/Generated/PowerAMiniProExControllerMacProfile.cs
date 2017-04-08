namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PowerAMiniProExControllerMacProfile : Xbox360DriverMacProfile
	{
		public PowerAMiniProExControllerMacProfile()
		{
			Name = "PowerA Mini Pro Ex Controller";
			Meta = "PowerA Mini Pro Ex Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x15e4,
					ProductID = 0x3f00,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x531a,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5300,
				},
			};
		}
	}
	// @endcond
}


