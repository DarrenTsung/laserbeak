namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class AfterglowPrismaticXboxOneControllerMacProfile : XboxOneDriverMacProfile
	{
		public AfterglowPrismaticXboxOneControllerMacProfile()
		{
			Name = "Afterglow Prismatic Xbox One Controller";
			Meta = "Afterglow Prismatic Xbox One Controller on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x0139,
				},
			};
		}
	}
	// @endcond
}


