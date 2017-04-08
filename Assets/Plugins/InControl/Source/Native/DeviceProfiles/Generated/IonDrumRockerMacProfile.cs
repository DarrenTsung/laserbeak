namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class IonDrumRockerMacProfile : Xbox360DriverMacProfile
	{
		public IonDrumRockerMacProfile()
		{
			Name = "Ion Drum Rocker";
			Meta = "Ion Drum Rocker on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x1bad,
					ProductID = 0x0130,
				},
			};
		}
	}
	// @endcond
}


