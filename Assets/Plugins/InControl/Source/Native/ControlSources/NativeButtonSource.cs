namespace InControl
{
	public class NativeButtonSource : InputControlSource
	{
		public int ButtonIndex;


		public NativeButtonSource( int buttonIndex )
		{
			ButtonIndex = buttonIndex;
		}


		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public bool GetState( InputDevice inputDevice )
		{
			var nativeDevice = inputDevice as NativeInputDevice;
			return nativeDevice.ReadRawButtonState( ButtonIndex );
		}
	}
}

