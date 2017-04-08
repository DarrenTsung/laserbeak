#if UNITY_IOS || UNITY_EDITOR
namespace InControl
{
	using System.Collections.Generic;
	using System.Threading;
	using UnityEngine;


	public class ICadeDeviceManager : InputDeviceManager
	{
		InputDevice device;
		RingBuffer<ICadeState> state;
		Thread thread;
		int timeStep;
		int bufferSize;
		bool active;


		public ICadeDeviceManager()
		{
			timeStep = Mathf.FloorToInt( Time.fixedDeltaTime * 1000.0f );
			bufferSize = 1;
			state = new RingBuffer<ICadeState>( bufferSize );

			device = new ICadeDevice( this );
			devices.Add( device );
		}


		void SetActive( bool value )
		{
			if (active != value)
			{
				active = value;

				ICadeNative.SetActive( active );

				if (active)
				{
					StartWorker();
					InputManager.AttachDevice( device );
				}
				else
				{
					StopWorker();
					InputManager.DetachDevice( device );
				}
			}
		}


		public static bool Active
		{
			get
			{
				var deviceManager = InputManager.GetDeviceManager<ICadeDeviceManager>();
				return deviceManager == null ? false : deviceManager.active;
			}

			set
			{
				var deviceManager = InputManager.GetDeviceManager<ICadeDeviceManager>();
				if (deviceManager != null)
				{
					deviceManager.SetActive( value );
				}
			}
		}


		void StartWorker()
		{
			if (thread == null)
			{
				thread = new Thread( Worker );
				thread.IsBackground = true;
				thread.Start();
			}
		}


		void StopWorker()
		{
			if (thread != null)
			{
				thread.Abort();
				thread.Join();
				thread = null;
			}
		}


		void Worker()
		{
			while (true)
			{
				state.Enqueue( ICadeNative.GetState() );
				Thread.Sleep( timeStep );
			}
		}


		internal ICadeState GetState()
		{
			return state.Dequeue();
		}


		public override void Update( ulong updateTick, float deltaTime )
		{
		}


		public override void Destroy()
		{
			StopWorker();
		}


		public static bool CheckPlatformSupport( ICollection<string> errors )
		{
			return Application.platform == RuntimePlatform.IPhonePlayer;
		}


		internal static void Enable()
		{
			var errors = new List<string>();
			if (ICadeDeviceManager.CheckPlatformSupport( errors ))
			{
				InputManager.AddDeviceManager<ICadeDeviceManager>();
			}
			else
			{
				foreach (var error in errors)
				{
					Logger.LogError( error );
				}
			}
		}
	}
}
#endif

