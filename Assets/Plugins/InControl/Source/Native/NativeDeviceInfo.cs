namespace InControl
{
	using System;
	using System.Runtime.InteropServices;


	[StructLayout( LayoutKind.Sequential, CharSet = CharSet.Ansi )]
	public struct NativeDeviceInfo
	{
		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 128 )]
		public string name;

		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 128 )]
		public string location;

		[MarshalAs( UnmanagedType.ByValTStr, SizeConst = 64 )]
		public string serialNumber;

		public UInt16 vendorID;
		public UInt16 productID;
		public UInt32 versionNumber;

		public NativeDeviceDriverType driverType;
		public NativeDeviceTransportType transportType;

		public UInt32 numButtons;
		public UInt32 numAnalogs;


		public bool HasSameVendorID( NativeDeviceInfo deviceInfo )
		{
			return vendorID == deviceInfo.vendorID;
		}


		public bool HasSameProductID( NativeDeviceInfo deviceInfo )
		{
			return productID == deviceInfo.productID;
		}


		public bool HasSameVersionNumber( NativeDeviceInfo deviceInfo )
		{
			return versionNumber == deviceInfo.versionNumber;
		}


		public bool HasSameLocation( NativeDeviceInfo deviceInfo )
		{
			if (String.IsNullOrEmpty( location ))
			{
				return false;
			}

			return location == deviceInfo.location;
		}


		public bool HasSameSerialNumber( NativeDeviceInfo deviceInfo )
		{
			if (String.IsNullOrEmpty( serialNumber ))
			{
				return false;
			}

			return serialNumber == deviceInfo.serialNumber;
		}
	}
}

