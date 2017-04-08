namespace InControl.NativeProfile
{
	using System;


	// @cond nodoc
	public class PDPXboxOneArcadeStickMacProfile : XboxOneDriverMacProfile
	{
		public PDPXboxOneArcadeStickMacProfile()
		{
			Name = "PDP Xbox One Arcade Stick";
			Meta = "PDP Xbox One Arcade Stick on Mac";

			Matchers = new[] {
				new NativeInputDeviceMatcher {
					VendorID = 0x0e6f,
					ProductID = 0x015c,
				},
			};
		}
	}
	// @endcond
}


