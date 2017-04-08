namespace InControl
{
	public class InputControl : OneAxisInputControl
	{
		public static readonly InputControl Null = new InputControl();

		public string Handle { get; protected set; }
		public InputControlType Target { get; protected set; }

		public bool Passive;

		// TODO: This meaningless distinction should probably be removed entirely.
		public bool IsButton { get; protected set; }
		public bool IsAnalog { get; protected set; }

		ulong zeroTick;


		private InputControl()
		{
			Handle = "None";
			Target = InputControlType.None;
			Passive = false;
			IsButton = false;
			IsAnalog = false;
		}


		public InputControl( string handle, InputControlType target )
		{
			Handle = handle;
			Target = target;
			Passive = false;
			IsButton = Utility.TargetIsButton( target );
			IsAnalog = !IsButton;
		}


		public InputControl( string handle, InputControlType target, bool passive )
			: this( handle, target )
		{
			Passive = passive;
		}


		internal void SetZeroTick()
		{
			zeroTick = UpdateTick;
		}


		internal bool IsOnZeroTick
		{
			get { return UpdateTick == zeroTick; }
		}


		public bool IsStandard
		{
			get
			{
				return Utility.TargetIsStandard( Target );
			}
		}
	}
}