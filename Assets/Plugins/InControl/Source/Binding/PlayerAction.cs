namespace InControl
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.IO;
	using UnityEngine;


	/// <summary>
	/// This class represents a single action that may have multiple controls bound to it.
	/// A bound control is represented by a subclass of BindingSource. For example,
	/// DeviceBindingSource provides input from a control on any supported InputDevice.
	/// Similarly, KeyBindingSource provides input from one or more keypresses. An action
	/// may have any number of bindings.
	/// Actions have two groups of bindings defined: default bindings and regular bindings.
	/// Default bindings are the predefined default bindings, and the current bindings for
	/// the action can be reset to this group in a single operation. Regular bindings are those
	/// added by users, most likely at runtime in a settings menu or the like. There are no
	/// other distinctions between these groupings; they are purely for organizational convenience.
	/// </summary>
	public class PlayerAction : OneAxisInputControl
	{
		/// <summary>
		/// The unique identifier for this action within the context of its owning action set.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Gets the owning action set containing this action.
		/// </summary>
		public PlayerActionSet Owner { get; private set; }

		/// <summary>
		/// Configures how this action listens for new bindings.
		/// When <c>null</c> (default) the owner's <see cref="PlayerActionSet.ListenOptions"/> will be used.
		/// <seealso cref="ListenForBinding()"/>
		/// </summary>
		public BindingListenOptions ListenOptions = null;

		/// <summary>
		/// The binding source type that last provided input to this action.
		/// </summary>
		public BindingSourceType LastInputType = BindingSourceType.None;

		/// <summary>
		/// Occurs when the binding source type that last provided input to this action changes.
		/// </summary>
		public event Action<BindingSourceType> OnLastInputTypeChanged;

		/// <summary>
		/// Updated when <see cref="LastInputType"/> changes.
		/// </summary>
		public ulong LastInputTypeChangedTick;

		/// <summary>
		/// The <see cref="InputDeviceClass"/> of the binding source that last provided input to this action.
		/// </summary>
		public InputDeviceClass LastDeviceClass = InputDeviceClass.Unknown;

		/// <summary>
		/// The <see cref="InputDeviceStyle"/> of the binding source that last provided input to this action.
		/// </summary>
		public InputDeviceStyle LastDeviceStyle = InputDeviceStyle.Unknown;

		/// <summary>
		/// This property can be used to store whatever arbitrary game data you want on this action.
		/// </summary>
		public object UserData { get; set; }

		List<BindingSource> defaultBindings = new List<BindingSource>();
		List<BindingSource> regularBindings = new List<BindingSource>();
		List<BindingSource> visibleBindings = new List<BindingSource>();

		readonly ReadOnlyCollection<BindingSource> bindings;
		readonly ReadOnlyCollection<BindingSource> unfilteredBindings;

		readonly static BindingSourceListener[] bindingSourceListeners = new BindingSourceListener[] {
			new DeviceBindingSourceListener(),
			new UnknownDeviceBindingSourceListener(),
			new KeyBindingSourceListener(),
			new MouseBindingSourceListener()
		};


		/// <summary>
		/// Construct an action belonging to a given action set.
		/// </summary>
		/// <param name="name">A unique identifier for this action within the context of its owning action set.</param>
		/// <param name="owner">The action set to contain (own) this action.</param>
		public PlayerAction( string name, PlayerActionSet owner )
		{
			Raw = true;
			Name = name;
			Owner = owner;
			bindings = new ReadOnlyCollection<BindingSource>( visibleBindings );
			unfilteredBindings = new ReadOnlyCollection<BindingSource>( regularBindings );
			owner.AddPlayerAction( this );
		}


		/// <summary>
		/// Adds a default binding for the action. This will also add it to the regular bindings.
		/// </summary>
		/// <param name="binding">The BindingSource to add.</param>
		public void AddDefaultBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return;
			}

			if (binding.BoundTo != null)
			{
				throw new InControlException( "Binding source is already bound to action " + binding.BoundTo.Name );
			}

			if (!defaultBindings.Contains( binding ))
			{
				defaultBindings.Add( binding );
				binding.BoundTo = this;
			}

			if (!regularBindings.Contains( binding ))
			{
				regularBindings.Add( binding );
				binding.BoundTo = this;

				if (binding.IsValid)
				{
					visibleBindings.Add( binding );
				}
			}
		}


		/// <summary>
		/// A convenience method for adding a KeyBindingSource to the default bindings.
		/// </summary>
		/// <param name="keys">A list of one or more keys making up a KeyCombo for the binding source.</param>
		public void AddDefaultBinding( params Key[] keys )
		{
			AddDefaultBinding( new KeyBindingSource( keys ) );
		}


		/// <summary>
		/// A convenience method for adding a MouseBindingSource to the default bindings.
		/// </summary>
		/// <param name="control">The Mouse control to add.</param>
		public void AddDefaultBinding( Mouse control )
		{
			AddDefaultBinding( new MouseBindingSource( control ) );
		}


		/// <summary>
		/// A convenience method for adding a DeviceBindingSource to the default bindings.
		/// </summary>
		/// <param name="control">The InputControlType to add.</param>
		public void AddDefaultBinding( InputControlType control )
		{
			AddDefaultBinding( new DeviceBindingSource( control ) );
		}


		/// <summary>
		/// Add a regular binding to the action. A binding cannot be added if it matches an
		/// existing binding on the action, or if it is already bound to another action.
		/// </summary>
		/// <returns><c>true</c>, if binding was added, <c>false</c> otherwise.</returns>
		/// <param name="binding">The BindingSource to add.</param>
		public bool AddBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return false;
			}

			if (binding.BoundTo != null)
			{
				Debug.LogWarning( "Binding source is already bound to action " + binding.BoundTo.Name );
				return false;
			}

			if (regularBindings.Contains( binding ))
			{
				return false;
			}

			regularBindings.Add( binding );
			binding.BoundTo = this;

			if (binding.IsValid)
			{
				visibleBindings.Add( binding );
			}

			return true;
		}


		/// <summary>
		/// Insert a regular binding to the action at the specified index. A binding cannot be
		/// inserted if it matches an existing binding on the action, or if it is already bound to
		/// another action.
		/// </summary>
		/// <returns><c>true</c>, if binding was inserted, <c>false</c> otherwise.</returns>
		/// <param name="binding">The index at which to insert.</param>
		/// <param name="binding">The BindingSource to insert.</param>
		public bool InsertBindingAt( int index, BindingSource binding )
		{
			if (index < 0 || index > visibleBindings.Count)
			{
				throw new InControlException( "Index is out of range for bindings on this action." );
			}

			if (index == visibleBindings.Count)
			{
				return AddBinding( binding );
			}

			if (binding == null)
			{
				return false;
			}

			if (binding.BoundTo != null)
			{
				Debug.LogWarning( "Binding source is already bound to action " + binding.BoundTo.Name );
				return false;
			}

			if (regularBindings.Contains( binding ))
			{
				return false;
			}

			var regularIndex = (index == 0) ? 0 : regularBindings.IndexOf( visibleBindings[index] );
			regularBindings.Insert( regularIndex, binding );
			binding.BoundTo = this;

			if (binding.IsValid)
			{
				visibleBindings.Insert( index, binding );
			}

			return true;
		}


		/// <summary>
		/// Add a regular binding to the action replacing an existing binding. A binding cannot be
		/// added if is already bound to another action. If the binding to replace is not present
		/// on this action, the binding will not be added.
		/// </summary>
		/// <returns><c>true</c>, if binding was added, <c>false</c> otherwise.</returns>
		/// <param name="findBinding">The BindingSource to replace.</param>
		/// <param name="withBinding">The BindingSource to replace it with.</param>
		public bool ReplaceBinding( BindingSource findBinding, BindingSource withBinding )
		{
			if (findBinding == null || withBinding == null)
			{
				return false;
			}

			if (withBinding.BoundTo != null)
			{
				Debug.LogWarning( "Binding source is already bound to action " + withBinding.BoundTo.Name );
				return false;
			}

			var index = regularBindings.IndexOf( findBinding );
			if (index < 0)
			{
				Debug.LogWarning( "Binding source to replace is not present in this action." );
				return false;
			}

			findBinding.BoundTo = null;
			regularBindings[index] = withBinding;
			withBinding.BoundTo = this;

			index = visibleBindings.IndexOf( findBinding );
			if (index >= 0)
			{
				visibleBindings[index] = withBinding;
			}

			return true;
		}


		public bool HasBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return false;
			}

			var foundBinding = FindBinding( binding );
			if (foundBinding == null)
			{
				return false;
			}

			return foundBinding.BoundTo == this;
		}


		internal BindingSource FindBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return null;
			}

			var index = regularBindings.IndexOf( binding );
			if (index >= 0)
			{
				return regularBindings[index];
			}

			return null;
		}


		internal void FindAndRemoveBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return;
			}

			var bindingIndex = regularBindings.IndexOf( binding );
			if (bindingIndex >= 0)
			{
				var foundBinding = regularBindings[bindingIndex];
				if (foundBinding.BoundTo == this)
				{
					foundBinding.BoundTo = null;
					regularBindings.RemoveAt( bindingIndex );
					UpdateVisibleBindings();
				}
			}
		}


		internal int CountBindingsOfType( BindingSourceType bindingSourceType )
		{
			var count = 0;
			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				var binding = regularBindings[i];
				if (binding.BoundTo == this && binding.BindingSourceType == bindingSourceType)
				{
					count += 1;
				}
			}
			return count;
		}


		internal void RemoveFirstBindingOfType( BindingSourceType bindingSourceType )
		{
			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				var binding = regularBindings[i];
				if (binding.BoundTo == this && binding.BindingSourceType == bindingSourceType)
				{
					binding.BoundTo = null;
					regularBindings.RemoveAt( i );
					return;
				}
			}
		}


		internal int IndexOfFirstInvalidBinding()
		{
			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				if (!regularBindings[i].IsValid)
				{
					return i;
				}
			}

			return -1;
		}


		/// <summary>
		/// Removes the binding from the action.
		/// Note: the action is only marked for removal, and is not immediately removed. This is
		/// to allow for safe removal during iteration over the Bindings collection.
		/// </summary>
		/// <param name="binding">The BindingSource to remove.</param>
		public void RemoveBinding( BindingSource binding )
		{
			if (binding == null)
			{
				return;
			}

			if (binding.BoundTo != this)
			{
				throw new InControlException( "Cannot remove a binding source not bound to this action." );
			}

			binding.BoundTo = null;
		}


		/// <summary>
		/// Removes the binding at the specified index from the action.
		/// Note: the action is only marked for removal, and is not immediately removed. This is
		/// to allow for safe removal during iteration over the Bindings collection.
		/// </summary>
		/// <param name="index">The index of the BindingSource in the Bindings collection to remove.</param>
		public void RemoveBindingAt( int index )
		{
			if (index < 0 || index >= regularBindings.Count)
			{
				throw new InControlException( "Index is out of range for bindings on this action." );
			}

			regularBindings[index].BoundTo = null;
		}


		/// <summary>
		/// Clears the bindings for this action.
		/// </summary>
		public void ClearBindings()
		{
			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				regularBindings[i].BoundTo = null;
			}
			regularBindings.Clear();
			visibleBindings.Clear();
		}


		/// <summary>
		/// Resets the bindings to the default bindings.
		/// </summary>
		public void ResetBindings()
		{
			ClearBindings();

			regularBindings.AddRange( defaultBindings );

			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				var binding = regularBindings[i];

				binding.BoundTo = this;

				if (binding.IsValid)
				{
					visibleBindings.Add( binding );
				}
			}
		}


		/// <summary>
		/// Begin listening for a new user defined binding.
		/// Which types of BindingSource are detected depends on the value of ListenOptions and DefaultListenOptions.
		/// Once one is found, it will be added to the regular bindings for the action and listening will stop.
		/// </summary>
		public void ListenForBinding()
		{
			ListenForBindingReplacing( null );
		}


		/// <summary>
		/// Begin listening for a new user defined binding, replacing an existing specified binding.
		/// If the binding to replace is not present on this action, the new binding will fail to be added.
		/// Which types of BindingSource are detected depends on the value of ListenOptions and DefaultListenOptions.
		/// Once one is found, it will be added to the regular bindings for the action and listening will stop.
		/// </summary>
		public void ListenForBindingReplacing( BindingSource binding )
		{
			var listenOptions = ListenOptions ?? Owner.ListenOptions;
			listenOptions.ReplaceBinding = binding;

			Owner.listenWithAction = this;

			var bindingSourceListenerCount = bindingSourceListeners.Length;
			for (var i = 0; i < bindingSourceListenerCount; i++)
			{
				bindingSourceListeners[i].Reset();
			}
		}


		/// <summary>
		/// Stop listening for new user defined bindings.
		/// </summary>
		public void StopListeningForBinding()
		{
			if (IsListeningForBinding)
			{
				Owner.listenWithAction = null;
			}
		}


		/// <summary>
		/// Gets a value indicating whether this action is listening for new user defined bindings.
		/// </summary>
		public bool IsListeningForBinding
		{
			get
			{
				return Owner.listenWithAction == this;
			}
		}


		/// <summary>
		/// Gets the valid (in context of the current device) bindings for this action as a readonly collection.
		/// What this means is, if your current active controller is an Xbox One controller and you have InputControlType.Options
		/// bound, it will not be included. This is generally the bindings you should display unless you are doing something custom.
		/// </summary>
		public ReadOnlyCollection<BindingSource> Bindings
		{
			get
			{
				return bindings;
			}
		}


		/// <summary>
		/// Gets ALL bindings for this action (including ones that don't make sense for the current device) as a readonly collection.
		/// Use of this collection is not recommended unless you really need unfettered access to all the bindings on an action.
		/// </summary>
		public ReadOnlyCollection<BindingSource> UnfilteredBindings
		{
			get
			{
				return unfilteredBindings;
			}
		}


		void RemoveOrphanedBindings()
		{
			var bindingCount = regularBindings.Count;
			for (var i = bindingCount - 1; i >= 0; i--)
			{
				if (regularBindings[i].BoundTo != this)
				{
					regularBindings.RemoveAt( i );
				}
			}
		}


		internal void Update( ulong updateTick, float deltaTime, InputDevice device )
		{
			Device = device;
			UpdateBindings( updateTick, deltaTime );
			DetectBindings();
		}


		void UpdateBindings( ulong updateTick, float deltaTime )
		{
			var lastInputType = LastInputType;
			var lastInputTypeChangedTick = LastInputTypeChangedTick;
			var lastUpdateTick = UpdateTick;
			var lastDeviceClass = LastDeviceClass;
			var lastDeviceStyle = LastDeviceStyle;

			var bindingCount = regularBindings.Count;
			for (var i = bindingCount - 1; i >= 0; i--)
			{
				var binding = regularBindings[i];

				if (binding.BoundTo != this)
				{
					regularBindings.RemoveAt( i );
					visibleBindings.Remove( binding );
				}
				else
				{
					var value = binding.GetValue( Device );
					if (UpdateWithValue( value, updateTick, deltaTime ))
					{
						lastInputType = binding.BindingSourceType;
						lastInputTypeChangedTick = updateTick;
						lastDeviceClass = binding.DeviceClass;
						lastDeviceStyle = binding.DeviceStyle;
					}
				}
			}

			Commit();

			Enabled = Owner.Enabled;

			if (lastInputTypeChangedTick > LastInputTypeChangedTick)
			{
				if (lastInputType != BindingSourceType.MouseBindingSource ||
					Utility.Abs( LastValue - Value ) >= MouseBindingSource.JitterThreshold)
				{
					var triggerEvent = lastInputType != LastInputType;

					LastInputType = lastInputType;
					LastInputTypeChangedTick = lastInputTypeChangedTick;
					LastDeviceClass = lastDeviceClass;
					LastDeviceStyle = lastDeviceStyle;

					if (OnLastInputTypeChanged != null && triggerEvent)
					{
						OnLastInputTypeChanged.Invoke( lastInputType );
					}
				}
			}

			if (UpdateTick > lastUpdateTick)
			{
				activeDevice = LastInputTypeIsDevice ? Device : null;
			}
		}


		void DetectBindings()
		{
			if (IsListeningForBinding)
			{
				BindingSource binding = null;
				var listenOptions = ListenOptions ?? Owner.ListenOptions;

				var bindingSourceListenerCount = bindingSourceListeners.Length;
				for (var i = 0; i < bindingSourceListenerCount; i++)
				{
					binding = bindingSourceListeners[i].Listen( listenOptions, device );
					if (binding != null)
					{
						break;
					}
				}

				if (binding == null)
				{
					// No binding found.
					return;
				}

				var onBindingFound = listenOptions.OnBindingFound;
				if (onBindingFound != null && !onBindingFound( this, binding ))
				{
					// Binding rejected by user.
					return;
				}

				if (HasBinding( binding ))
				{
					var onBindingRejected = listenOptions.OnBindingRejected;
					if (onBindingRejected != null)
					{
						onBindingRejected( this, binding, BindingSourceRejectionType.DuplicateBindingOnAction );
					}
					return;
				}

				if (listenOptions.UnsetDuplicateBindingsOnSet)
				{
					Owner.RemoveBinding( binding );
				}

				if (!listenOptions.AllowDuplicateBindingsPerSet && Owner.HasBinding( binding ))
				{
					var onBindingRejected = listenOptions.OnBindingRejected;
					if (onBindingRejected != null)
					{
						onBindingRejected( this, binding, BindingSourceRejectionType.DuplicateBindingOnActionSet );
					}
					return;
				}

				StopListeningForBinding();

				if (listenOptions.ReplaceBinding == null)
				{
					if (listenOptions.MaxAllowedBindingsPerType > 0)
					{
						while (CountBindingsOfType( binding.BindingSourceType ) >= listenOptions.MaxAllowedBindingsPerType)
						{
							RemoveFirstBindingOfType( binding.BindingSourceType );
						}
					}
					else
					{
						if (listenOptions.MaxAllowedBindings > 0)
						{
							while (regularBindings.Count >= listenOptions.MaxAllowedBindings)
							{
								var removeIndex = Mathf.Max( 0, IndexOfFirstInvalidBinding() );
								regularBindings.RemoveAt( removeIndex );
							}
						}
					}

					AddBinding( binding );
				}
				else
				{
					ReplaceBinding( listenOptions.ReplaceBinding, binding );
				}

				UpdateVisibleBindings();

				var onBindingAdded = listenOptions.OnBindingAdded;
				if (onBindingAdded != null)
				{
					onBindingAdded( this, binding );
				}
			}
		}


		void UpdateVisibleBindings()
		{
			visibleBindings.Clear();
			var bindingCount = regularBindings.Count;
			for (var i = 0; i < bindingCount; i++)
			{
				var binding = regularBindings[i];
				if (binding.IsValid)
				{
					visibleBindings.Add( binding );
				}
			}
		}


		InputDevice device;
		internal InputDevice Device
		{
			get
			{
				if (device == null)
				{
					device = Owner.Device;
					UpdateVisibleBindings();
				}

				return device;
			}

			set
			{
				if (device != value)
				{
					device = value;
					UpdateVisibleBindings();
				}
			}
		}


		InputDevice activeDevice;
		/// <summary>
		/// Gets the currently active device (controller) if present, otherwise returns a null device which does nothing.
		/// The currently active device is defined as the last device that provided input to this action.
		/// When LastInputType is not a device (controller), this will return the null device.
		/// </summary>
		public InputDevice ActiveDevice
		{
			get
			{
				return (activeDevice == null) ? InputDevice.Null : activeDevice;
			}
		}


		bool LastInputTypeIsDevice
		{
			get
			{
				return LastInputType == BindingSourceType.DeviceBindingSource ||
					   LastInputType == BindingSourceType.UnknownDeviceBindingSource;
			}
		}


		[Obsolete( "Please set this property on device controls directly. It does nothing here." )]
		public new float LowerDeadZone
		{
			get
			{
				return 0.0f;
			}

			set
			{
#pragma warning disable 0168, 0219
				var dummy = value;
#pragma warning restore 0168, 0219
			}
		}


		[Obsolete( "Please set this property on device controls directly. It does nothing here." )]
		public new float UpperDeadZone
		{
			get
			{
				return 0.0f;
			}

			set
			{
#pragma warning disable 0168, 0219
				var dummy = value;
#pragma warning restore 0168, 0219
			}
		}


		internal void Load( BinaryReader reader )
		{
			ClearBindings();

			var bindingCount = reader.ReadInt32();
			for (var i = 0; i < bindingCount; i++)
			{
				var bindingSourceType = (BindingSourceType) reader.ReadInt32();
				BindingSource bindingSource;

				switch (bindingSourceType)
				{
				case BindingSourceType.None:
					continue;

				case BindingSourceType.DeviceBindingSource:
					bindingSource = new DeviceBindingSource();
					break;

				case BindingSourceType.KeyBindingSource:
					bindingSource = new KeyBindingSource();
					break;

				case BindingSourceType.MouseBindingSource:
					bindingSource = new MouseBindingSource();
					break;

				case BindingSourceType.UnknownDeviceBindingSource:
					bindingSource = new UnknownDeviceBindingSource();
					break;

				default:
					throw new InControlException( "Don't know how to load BindingSourceType: " + bindingSourceType );
				}

				bindingSource.Load( reader );
				AddBinding( bindingSource );
			}
		}


		internal void Save( BinaryWriter writer )
		{
			RemoveOrphanedBindings();

			writer.Write( Name );

			var bindingCount = regularBindings.Count;
			writer.Write( bindingCount );

			for (var i = 0; i < bindingCount; i++)
			{
				var binding = regularBindings[i];
				writer.Write( (int) binding.BindingSourceType );
				binding.Save( writer );
			}
		}
	}
}
