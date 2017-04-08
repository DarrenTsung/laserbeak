namespace InControl
{
	using UnityEngine;


	public class UnityInputDevice : InputDevice
	{
		static string[,] analogQueries;
		static string[,] buttonQueries;

		public const int MaxDevices = 10;
		public const int MaxButtons = 20;
		public const int MaxAnalogs = 20;

		internal int JoystickId { get; private set; }

		UnityInputDeviceProfileBase profile;


		public UnityInputDevice( UnityInputDeviceProfileBase deviceProfile )
			: this( deviceProfile, 0, "" )
		{
		}


		public UnityInputDevice( int joystickId, string joystickName )
			: this( null, joystickId, joystickName )
		{
		}


		public UnityInputDevice( UnityInputDeviceProfileBase deviceProfile, int joystickId, string joystickName )
		{
			profile = deviceProfile;

			JoystickId = joystickId;
			if (joystickId != 0)
			{
				SortOrder = 100 + joystickId;
			}

			SetupAnalogQueries();
			SetupButtonQueries();

			AnalogSnapshot = null;

			if (IsKnown)
			{
				Name = profile.Name;
				Meta = profile.Meta;

				DeviceClass = profile.DeviceClass;
				DeviceStyle = profile.DeviceStyle;

				var analogMappingCount = profile.AnalogCount;
				for (var i = 0; i < analogMappingCount; i++)
				{
					var analogMapping = profile.AnalogMappings[i];
					if (Utility.TargetIsAlias( analogMapping.Target ))
					{
						Debug.LogError( "Cannot map control \"" + analogMapping.Handle + "\" as InputControlType." + analogMapping.Target + " in profile \"" + deviceProfile.Name + "\" because this target is reserved as an alias. The mapping will be ignored." );
					}
					else
					{
						var analogControl = AddControl( analogMapping.Target, analogMapping.Handle );
						analogControl.Sensitivity = Mathf.Min( profile.Sensitivity, analogMapping.Sensitivity );
						analogControl.LowerDeadZone = Mathf.Max( profile.LowerDeadZone, analogMapping.LowerDeadZone );
						analogControl.UpperDeadZone = Mathf.Min( profile.UpperDeadZone, analogMapping.UpperDeadZone );
						analogControl.Raw = analogMapping.Raw;
						analogControl.Passive = analogMapping.Passive;
					}
				}

				var buttonMappingCount = profile.ButtonCount;
				for (var i = 0; i < buttonMappingCount; i++)
				{
					var buttonMapping = profile.ButtonMappings[i];
					if (Utility.TargetIsAlias( buttonMapping.Target ))
					{
						Debug.LogError( "Cannot map control \"" + buttonMapping.Handle + "\" as InputControlType." + buttonMapping.Target + " in profile \"" + deviceProfile.Name + "\" because this target is reserved as an alias. The mapping will be ignored." );
					}
					else
					{
						var buttonControl = AddControl( buttonMapping.Target, buttonMapping.Handle );
						buttonControl.Passive = buttonMapping.Passive;
					}
				}
			}
			else
			{
				Name = "Unknown Device";
				Meta = "\"" + joystickName + "\"";

				for (var i = 0; i < NumUnknownButtons; i++)
				{
					AddControl( InputControlType.Button0 + i, "Button " + i );
				}

				for (var i = 0; i < NumUnknownAnalogs; i++)
				{
					AddControl( InputControlType.Analog0 + i, "Analog " + i, 0.2f, 0.9f );
				}
			}
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			if (IsKnown)
			{
				var analogMappingCount = profile.AnalogCount;
				for (var i = 0; i < analogMappingCount; i++)
				{
					var analogMapping = profile.AnalogMappings[i];
					var analogValue = analogMapping.Source.GetValue( this );
					var targetControl = GetControl( analogMapping.Target );

					if (!(analogMapping.IgnoreInitialZeroValue && targetControl.IsOnZeroTick && Utility.IsZero( analogValue )))
					{
						var mappedValue = analogMapping.MapValue( analogValue );
						targetControl.UpdateWithValue( mappedValue, updateTick, deltaTime );
					}
				}

				var buttonMappingCount = profile.ButtonCount;
				for (var i = 0; i < buttonMappingCount; i++)
				{
					var buttonMapping = profile.ButtonMappings[i];
					var buttonState = buttonMapping.Source.GetState( this );

					UpdateWithState( buttonMapping.Target, buttonState, updateTick, deltaTime );
				}
			}
			else
			{
				for (var i = 0; i < NumUnknownButtons; i++)
				{
					UpdateWithState( InputControlType.Button0 + i, ReadRawButtonState( i ), updateTick, deltaTime );
				}

				for (var i = 0; i < NumUnknownAnalogs; i++)
				{
					UpdateWithValue( InputControlType.Analog0 + i, ReadRawAnalogValue( i ), updateTick, deltaTime );
				}
			}
		}


		static void SetupAnalogQueries()
		{
			if (analogQueries == null)
			{
				analogQueries = new string[MaxDevices, MaxAnalogs];

				for (var joystickId = 1; joystickId <= MaxDevices; joystickId++)
				{
					for (var analogId = 0; analogId < MaxAnalogs; analogId++)
					{
						analogQueries[joystickId - 1, analogId] = "joystick " + joystickId + " analog " + analogId;
					}
				}
			}
		}


		static void SetupButtonQueries()
		{
			if (buttonQueries == null)
			{
				buttonQueries = new string[MaxDevices, MaxButtons];

				for (var joystickId = 1; joystickId <= MaxDevices; joystickId++)
				{
					for (var buttonId = 0; buttonId < MaxButtons; buttonId++)
					{
						buttonQueries[joystickId - 1, buttonId] = "joystick " + joystickId + " button " + buttonId;
					}
				}
			}
		}


		static string GetAnalogKey( int joystickId, int analogId )
		{
			return analogQueries[joystickId - 1, analogId];
		}


		static string GetButtonKey( int joystickId, int buttonId )
		{
			return buttonQueries[joystickId - 1, buttonId];
		}


		internal override bool ReadRawButtonState( int index )
		{
			if (index < MaxButtons)
			{
				var buttonQuery = buttonQueries[JoystickId - 1, index];
				return Input.GetKey( buttonQuery );
			}
			return false;
		}


		internal override float ReadRawAnalogValue( int index )
		{
			if (index < MaxAnalogs)
			{
				var analogQuery = analogQueries[JoystickId - 1, index];
				return Input.GetAxisRaw( analogQuery );
			}
			return 0.0f;
		}


		public override bool IsSupportedOnThisPlatform
		{
			get
			{
				return profile == null || profile.IsSupportedOnThisPlatform;
			}
		}


		public override bool IsKnown
		{
			get
			{
				return profile != null;
			}
		}


		internal override int NumUnknownButtons
		{
			get
			{
				return MaxButtons;
			}
		}


		internal override int NumUnknownAnalogs
		{
			get
			{
				return MaxAnalogs;
			}
		}
	}
}

