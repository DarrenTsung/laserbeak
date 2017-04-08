namespace InControl
{
	public class UnityAnalogSource : InputControlSource
	{
		public int AnalogIndex;


		public UnityAnalogSource( int analogIndex )
		{
			AnalogIndex = analogIndex;
		}


		public float GetValue( InputDevice inputDevice )
		{
			var unityDevice = inputDevice as UnityInputDevice;
			return unityDevice.ReadRawAnalogValue( AnalogIndex );
		}


		public bool GetState( InputDevice inputDevice )
		{
			return Utility.IsNotZero( GetValue( inputDevice ) );
		}
	}
}

