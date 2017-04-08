namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class RazerSabertoothEliteControllerMacProfile : Xbox360DriverMacProfile
	{
		public RazerSabertoothEliteControllerMacProfile()
		{
			Name = "Razer Sabertooth Elite Controller";
			Meta = "Razer Sabertooth Elite Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1689,
					ProductID = 0xfe00,
				},
				new NativeInputDeviceMatcher {
					VendorID = 0x24c6,
					ProductID = 0x5d04,
				},
			};
		}
	}
	// @endcond
}


