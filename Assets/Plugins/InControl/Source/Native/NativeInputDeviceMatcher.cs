namespace InControl
{
	using System;
	using System.Text.RegularExpressions;


	public class NativeInputDeviceMatcher
	{
		public UInt16? VendorID;
		public UInt16? ProductID;
		public UInt32? VersionNumber;
		public NativeDeviceDriverType? DriverType;
		public NativeDeviceTransportType? TransportType;
		public string[] NameLiterals;
		public string[] NamePatterns;


		internal bool Matches( NativeDeviceInfo deviceInfo )
		{
			var hasMatched = false;

			if (VendorID.HasValue)
			{
				if (VendorID.Value == deviceInfo.vendorID)
				{
					hasMatched = true;
				}
				else
				{
					return false;
				}
			}

			if (ProductID.HasValue)
			{
				if (ProductID.Value == deviceInfo.productID)
				{
					hasMatched = true;
				}
				else
				{
					return false;
				}
			}

			if (VersionNumber.HasValue)
			{
				if (VersionNumber.Value == deviceInfo.versionNumber)
				{
					hasMatched = true;
				}
				else
				{
					return false;
				}
			}

			if (DriverType.HasValue)
			{
				if (DriverType.Value == deviceInfo.driverType)
				{
					hasMatched = true;
				}
				else
				{
					return false;
				}
			}

			if (TransportType.HasValue)
			{
				if (TransportType.Value == deviceInfo.transportType)
				{
					hasMatched = true;
				}
				else
				{
					return false;
				}
			}

			if (NameLiterals != null && NameLiterals.Length > 0)
			{
				var nameLiteralsCount = NameLiterals.Length;
				for (var i = 0; i < nameLiteralsCount; i++)
				{
					if (String.Equals( deviceInfo.name, NameLiterals[i], StringComparison.OrdinalIgnoreCase ))
					{
						return true;
					}
				}
				return false;
			}

			if (NamePatterns != null && NamePatterns.Length > 0)
			{
				var namePatternsCount = NamePatterns.Length;
				for (var i = 0; i < namePatternsCount; i++)
				{
					if (Regex.IsMatch( deviceInfo.name, NamePatterns[i], RegexOptions.IgnoreCase ))
					{
						return true;
					}
				}
				return false;
			}

			return hasMatched;
		}
	}
}

