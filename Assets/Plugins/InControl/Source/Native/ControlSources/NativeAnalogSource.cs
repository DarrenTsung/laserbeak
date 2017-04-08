namespace InControl
{
	public class NativeAnalogSource : InputControlSource
	{
		public int AnalogIndex;


		public NativeAnalogSource( int analogIndex )
		{
			AnalogIndex = analogIndex;
		}


		public float GetValue( InputDevice inputDevice )
		{
			var nativeDevice = inputDevice as NativeInputDevice;
			return nativeDevice.ReadRawAnalogValue( AnalogIndex );
		}


		public bool GetState( InputDevice inputDevice )
		{
			return Utility.IsNotZero( GetValue( inputDevice ) );
		}
	}
}

