namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPVersusControllerMacProfile : Xbox360DriverMacProfile
	{
		public PDPVersusControllerMacProfile()
		{
			Name = "PDP Versus Controller";
			Meta = "PDP Versus Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0xf904,
				},
			};
		}
	}
	// @endcond
}


