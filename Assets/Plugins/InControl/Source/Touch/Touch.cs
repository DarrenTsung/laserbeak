namespace InControl
{
	using UnityEngine;


	public class Touch
	{
		public readonly static int FingerID_None = -1;
		public readonly static int FingerID_Mouse = -2;

		public int fingerId;

		public TouchPhase phase;
		public int tapCount;

		public Vector2 position;
		public Vector2 deltaPosition;
		public Vector2 lastPosition;

		public float deltaTime;
		public ulong updateTick;

		public TouchType type;

		public float altitudeAngle;
		public float azimuthAngle;
		public float maximumPossiblePressure;
		public float pressure;
		public float radius;
		public float radiusVariance;


		internal Touch()
		{
			fingerId = FingerID_None;
			phase = TouchPhase.Ended;
		}


		internal void Reset()
		{
			fingerId = FingerID_None;
			phase = TouchPhase.Ended;
			tapCount = 0;
			position = Vector2.zero;
			deltaPosition = Vector2.zero;
			lastPosition = Vector2.zero;
			deltaTime = 0.0f;
			updateTick = 0;
			type = (TouchType) 0;
			altitudeAngle = 0.0f;
			azimuthAngle = 0.0f;
			maximumPossiblePressure = 1.0f;
			pressure = 0.0f;
			radius = 0.0f;
			radiusVariance = 0.0f;
		}


		public float normalizedPressure
		{
			get
			{
				// Return at least a tiny value otherwise pressure can be zero.
				return Mathf.Clamp( pressure / maximumPossiblePressure, 0.001f, 1.0f );
			}
		}


		internal void SetWithTouchData( UnityEngine.Touch touch, ulong updateTick, float deltaTime )
		{
			phase = touch.phase;
			tapCount = touch.tapCount;

#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6 || UNITY_4_7 || UNITY_5_0 || UNITY_5_1 || UNITY_5_2
			type = TouchType.Direct;
			altitudeAngle = Mathf.PI / 2.0f;
			azimuthAngle = Mathf.PI / 2.0f;
			maximumPossiblePressure = 1.0f;
			pressure = 1.0f;
			radius = 1.0f;
			radiusVariance = 0.0f;
#else
			altitudeAngle = touch.altitudeAngle;
			azimuthAngle = touch.azimuthAngle;
			maximumPossiblePressure = touch.maximumPossiblePressure;
			pressure = touch.pressure;
			radius = touch.radius;
			radiusVariance = touch.radiusVariance;
#endif

			var touchPosition = touch.position;

			// Deal with Unity Remote weirdness.
			if (touchPosition.x < 0.0f)
			{
				touchPosition.x = Screen.width + touchPosition.x;
			}

			if (phase == TouchPhase.Began)
			{
				deltaPosition = Vector2.zero;
				lastPosition = touchPosition;
				position = touchPosition;
			}
			else
			{
				if (phase == TouchPhase.Stationary)
				{
					phase = TouchPhase.Moved;
				}

				deltaPosition = touchPosition - lastPosition;
				lastPosition = position;
				position = touchPosition;
			}

			this.deltaTime = deltaTime;
			this.updateTick = updateTick;
		}


		internal bool SetWithMouseData( ulong updateTick, float deltaTime )
		{
			// Unity Remote and possibly some platforms like WP8 simulates mouse with
			// touches so detect that situation and reject the mouse.
			if (Input.touchCount > 0)
			{
				return false;
			}

			var mousePosition = new Vector2( Mathf.Round( Input.mousePosition.x ), Mathf.Round( Input.mousePosition.y ) );

			if (Input.GetMouseButtonDown( 0 ))
			{
				phase = TouchPhase.Began;
				pressure = 1.0f;
				maximumPossiblePressure = 1.0f;

				tapCount = 1;
				type = TouchType.Mouse;

				deltaPosition = Vector2.zero;
				lastPosition = mousePosition;
				position = mousePosition;

				this.deltaTime = deltaTime;
				this.updateTick = updateTick;

				return true;
			}

			if (Input.GetMouseButtonUp( 0 ))
			{
				phase = TouchPhase.Ended;
				pressure = 0.0f;
				maximumPossiblePressure = 1.0f;

				tapCount = 1;
				type = TouchType.Mouse;

				deltaPosition = mousePosition - lastPosition;
				lastPosition = position;
				position = mousePosition;

				this.deltaTime = deltaTime;
				this.updateTick = updateTick;

				return true;
			}

			if (Input.GetMouseButton( 0 ))
			{
				phase = TouchPhase.Moved;
				pressure = 1.0f;
				maximumPossiblePressure = 1.0f;

				tapCount = 1;
				type = TouchType.Mouse;

				deltaPosition = mousePosition - lastPosition;
				lastPosition = position;
				position = mousePosition;

				this.deltaTime = deltaTime;
				this.updateTick = updateTick;

				return true;
			}

			return false;
		}
	}
}
