namespace InControl
{
	public class UnityButtonSource : InputControlSource
	{
		public int ButtonIndex;


		public UnityButtonSource( int buttonIndex )
		{
			ButtonIndex = buttonIndex;
		}


		public float GetValue( InputDevice inputDevice )
		{
			return GetState( inputDevice ) ? 1.0f : 0.0f;
		}


		public bool GetState( InputDevice inputDevice )
		{
			var unityDevice = inputDevice as UnityInputDevice;
			return unityDevice.ReadRawButtonState( ButtonIndex );
		}
	}
}

