namespace InControl
{
	using System;
	using UnityEngine;


	/// <summary>
	/// "Custom profiles" are deprecated in favor of the new user bindings API.
	/// See the PlayerAction and PlayerActionSet classes which accomplish the same goal
	/// much more elegantly and supports runtime remapping.
	/// http://www.gallantgames.com/pages/incontrol-binding-actions-to-controls 
	/// </summary>
	[Obsolete( "Custom profiles are deprecated. Use the bindings API instead.", false )]
	public class CustomInputDeviceProfile : UnityInputDeviceProfileBase
	{
		public CustomInputDeviceProfile()
		{
			Name = "Custom Device Profile";
			Meta = "Custom Device Profile";

			IncludePlatforms = new[] {
				"Windows",
				"Mac",
				"Linux"
			};

			Sensitivity = 1.0f;
			LowerDeadZone = 0.0f;
			UpperDeadZone = 1.0f;
		}


		public sealed override bool IsJoystick
		{ 
			get
			{ 
				return false; 
			} 
		}


		public sealed override bool HasJoystickName( string joystickName )
		{
			return false;
		}


		public sealed override bool HasLastResortRegex( string joystickName )
		{
			return false;
		}


		public sealed override bool HasJoystickOrRegexName( string joystickName )
		{
			return false;
		}


		#region InputControlSource Helpers

		protected static InputControlSource KeyCodeButton( params KeyCode[] keyCodeList )
		{
			return new UnityKeyCodeSource( keyCodeList );
		}

		protected static InputControlSource KeyCodeComboButton( params KeyCode[] keyCodeList )
		{
			return new UnityKeyCodeComboSource( keyCodeList );
		}

		protected static InputControlSource MouseButton0 = new UnityMouseButtonSource( 0 );
		protected static InputControlSource MouseButton1 = new UnityMouseButtonSource( 1 );
		protected static InputControlSource MouseButton2 = new UnityMouseButtonSource( 2 );
		protected static InputControlSource MouseXAxis = new UnityMouseAxisSource( "x" );
		protected static InputControlSource MouseYAxis = new UnityMouseAxisSource( "y" );
		protected static InputControlSource MouseScrollWheel = new UnityMouseAxisSource( "z" );

		#endregion
	}
}

