namespace InControl
{
	using System.Collections.Generic;


	public abstract class InputDeviceManager
	{
		protected List<InputDevice> devices = new List<InputDevice>();

		public abstract void Update( ulong updateTick, float deltaTime );


		public virtual void Destroy()
		{
		}
	}
}

