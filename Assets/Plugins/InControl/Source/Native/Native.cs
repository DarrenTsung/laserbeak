namespace InControl
{
	using System;
	using System.Runtime.InteropServices;
	using UnityEngine;
	using DeviceHandle = System.UInt32;

	// @cond nodoc
	internal static class Native
	{
		const string LibraryName = "InControlNative";

#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX

		[DllImport( LibraryName, EntryPoint = "InControl_Init" )]
		public static extern void Init( NativeInputOptions options );


		[DllImport( LibraryName, EntryPoint = "InControl_Stop" )]
		public static extern void Stop();


		[DllImport( LibraryName, EntryPoint = "InControl_GetVersionInfo" )]
		public static extern void GetVersionInfo( out NativeVersionInfo versionInfo );


		[DllImport( LibraryName, EntryPoint = "InControl_GetDeviceInfo" )]
		public static extern bool GetDeviceInfo( DeviceHandle handle, out NativeDeviceInfo deviceInfo );


		[DllImport( LibraryName, EntryPoint = "InControl_GetDeviceState" )]
		public static extern bool GetDeviceState( DeviceHandle handle, out IntPtr deviceState );


		[DllImport( LibraryName, EntryPoint = "InControl_GetDeviceEvents" )]
		public static extern Int32 GetDeviceEvents( out IntPtr deviceEvents );


		[DllImport( LibraryName, EntryPoint = "InControl_SetHapticState" )]
		public static extern void SetHapticState( UInt32 handle, Byte motor0, Byte motor1 );


		[DllImport( LibraryName, EntryPoint = "InControl_SetLightColor" )]
		public static extern void SetLightColor( UInt32 handle, Byte red, Byte green, Byte blue );


		[DllImport( LibraryName, EntryPoint = "InControl_SetLightFlash" )]
		public static extern void SetLightFlash( UInt32 handle, Byte flashOnDuration, Byte flashOffDuration );

#else

		public static void Init( NativeInputOptions options ) { }
		public static void Stop() { }
		public static void GetVersionInfo( out NativeVersionInfo versionInfo ) { versionInfo = new NativeVersionInfo(); }
		public static bool GetDeviceInfo( DeviceHandle handle, out NativeDeviceInfo deviceInfo ) { deviceInfo = new NativeDeviceInfo(); return false; }
		public static bool GetDeviceState( DeviceHandle handle, out IntPtr deviceState ) { deviceState = IntPtr.Zero; return false; }
		public static Int32 GetDeviceEvents( out IntPtr deviceEvents ) { deviceEvents = IntPtr.Zero; return 0; }
		public static void SetHapticState( UInt32 handle, Byte motor0, Byte motor1 ) { }
		public static void SetLightColor( UInt32 handle, Byte red, Byte green, Byte blue ) { }
		public static void SetLightFlash( UInt32 handle, Byte flashOnDuration, Byte flashOffDuration ) { }

#endif

	}
	//@endcond
}

