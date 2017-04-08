namespace InControl
{
	using System;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using DeviceHandle = System.UInt32;


	public class NativeInputDevice : InputDevice
	{
		internal DeviceHandle Handle { get; private set; }
		internal NativeDeviceInfo Info { get; private set; }

		Int16[] buttons;
		Int16[] analogs;
		NativeInputDeviceProfile profile;

		int skipUpdateFrames = 0;


		internal NativeInputDevice()
		{
		}


		internal void Initialize( DeviceHandle deviceHandle, NativeDeviceInfo deviceInfo, NativeInputDeviceProfile deviceProfile )
		{
			Handle = deviceHandle;
			Info = deviceInfo;
			profile = deviceProfile;

			SortOrder = 1000 + (int) Handle;

			buttons = new Int16[Info.numButtons];
			analogs = new Int16[Info.numAnalogs];

			AnalogSnapshot = null;

			ClearInputState();
			ClearControls();

			if (IsKnown)
			{
				Name = profile.Name ?? Info.name;
				Meta = profile.Meta ?? Info.name;

				DeviceClass = profile.DeviceClass;
				DeviceStyle = profile.DeviceStyle;

				var analogMappingCount = profile.AnalogCount;
				for (var i = 0; i < analogMappingCount; i++)
				{
					var analogMapping = profile.AnalogMappings[i];
					var analogControl = AddControl( analogMapping.Target, analogMapping.Handle );
					analogControl.Sensitivity = Mathf.Min( profile.Sensitivity, analogMapping.Sensitivity );
					analogControl.LowerDeadZone = Mathf.Max( profile.LowerDeadZone, analogMapping.LowerDeadZone );
					analogControl.UpperDeadZone = Mathf.Min( profile.UpperDeadZone, analogMapping.UpperDeadZone );
					analogControl.Raw = analogMapping.Raw;
					analogControl.Passive = analogMapping.Passive;
				}

				var buttonMappingCount = profile.ButtonCount;
				for (var i = 0; i < buttonMappingCount; i++)
				{
					var buttonMapping = profile.ButtonMappings[i];
					var buttonControl = AddControl( buttonMapping.Target, buttonMapping.Handle );
					buttonControl.Passive = buttonMapping.Passive;
				}
			}
			else
			{
				Name = "Unknown Device";
				Meta = Info.name;

				for (var i = 0; i < NumUnknownButtons; i++)
				{
					AddControl( InputControlType.Button0 + i, "Button " + i );
				}

				for (var i = 0; i < NumUnknownAnalogs; i++)
				{
					AddControl( InputControlType.Analog0 + i, "Analog " + i, 0.2f, 0.9f );
				}
			}

			skipUpdateFrames = 1;
		}


		internal void Initialize( DeviceHandle deviceHandle, NativeDeviceInfo deviceInfo )
		{
			Initialize( deviceHandle, deviceInfo, this.profile );
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
			if (skipUpdateFrames > 0)
			{
				skipUpdateFrames -= 1;
				return;
			}

			IntPtr data;
			if (Native.GetDeviceState( Handle, out data ))
			{
				Marshal.Copy( data, buttons, 0, buttons.Length );
				data = new IntPtr( data.ToInt64() + (buttons.Length * sizeof( Int16 )) );
				Marshal.Copy( data, analogs, 0, analogs.Length );
			}

			if (IsKnown)
			{
				var analogMappingCount = profile.AnalogCount;
				for (var i = 0; i < analogMappingCount; i++)
				{
					var analogMapping = profile.AnalogMappings[i];
					var analogValue = analogMapping.Source.GetValue( this );
					//var mappedValue = analogMapping.MapValue( analogValue );
					//UpdateWithValue( analogMapping.Target, mappedValue, updateTick, deltaTime );

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
				for (var i = 0; i < Info.numButtons; i++)
				{
					UpdateWithState( InputControlType.Button0 + i, ReadRawButtonState( i ), updateTick, deltaTime );
				}

				for (var i = 0; i < Info.numAnalogs; i++)
				{
					UpdateWithValue( InputControlType.Analog0 + i, ReadRawAnalogValue( i ), updateTick, deltaTime );
				}
			}
		}


		internal override bool ReadRawButtonState( int index )
		{
			if (index < buttons.Length)
			{
				return buttons[index] > -Int16.MaxValue;
			}
			return false;
		}


		internal override float ReadRawAnalogValue( int index )
		{
			if (index < analogs.Length)
			{
				return analogs[index] / (float) Int16.MaxValue;
			}
			return 0.0f;
		}


		Byte FloatToByte( float value )
		{
			return (Byte) (Mathf.Clamp01( value ) * 0xFF);
		}


		public override void Vibrate( float leftMotor, float rightMotor )
		{
			Native.SetHapticState( Handle, FloatToByte( leftMotor ), FloatToByte( rightMotor ) );
		}


		public override void SetLightColor( float red, float green, float blue )
		{
			Native.SetLightColor( Handle, FloatToByte( red ), FloatToByte( green ), FloatToByte( blue ) );
		}


		public override void SetLightFlash( float flashOnDuration, float flashOffDuration )
		{
			Native.SetLightFlash( Handle, FloatToByte( flashOnDuration ), FloatToByte( flashOffDuration ) );
		}


		public bool HasSameVendorID( NativeDeviceInfo deviceInfo )
		{
			return Info.HasSameVendorID( deviceInfo );
		}


		public bool HasSameProductID( NativeDeviceInfo deviceInfo )
		{
			return Info.HasSameProductID( deviceInfo );
		}


		public bool HasSameVersionNumber( NativeDeviceInfo deviceInfo )
		{
			return Info.HasSameVersionNumber( deviceInfo );
		}


		public bool HasSameLocation( NativeDeviceInfo deviceInfo )
		{
			return Info.HasSameLocation( deviceInfo );
		}


		public bool HasSameSerialNumber( NativeDeviceInfo deviceInfo )
		{
			return Info.HasSameSerialNumber( deviceInfo );
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
				return (int) Info.numButtons;
			}
		}


		internal override int NumUnknownAnalogs
		{
			get
			{
				return (int) Info.numAnalogs;
			}
		}
	}
}
