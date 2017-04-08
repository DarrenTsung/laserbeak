namespace InControl
{
	using UnityEngine;


	public class UnityKeyCodeAxisSource : InputControlSource
	{
		public KeyCode NegativeKeyCode;
		public KeyCode PositiveKeyCode;


		public UnityKeyCodeAxisSource()
		{
		}


		public UnityKeyCodeAxisSource( KeyCode negativeKeyCode, KeyCode positiveKeyCode )
		{
			NegativeKeyCode = negativeKeyCode;
			PositiveKeyCode = positiveKeyCode;
		}


		public float GetValue( InputDevice inputDevice )
		{
			var axisValue = 0;
			
			if (Input.GetKey( NegativeKeyCode ))
			{
				axisValue--;
			}
			
			if (Input.GetKey( PositiveKeyCode ))
			{
				axisValue++;
			}
			
			return axisValue;
		}


		public bool GetState( InputDevice inputDevice )
		{
			return Utility.IsNotZero( GetValue( inputDevice ) );
		}
	}
}

