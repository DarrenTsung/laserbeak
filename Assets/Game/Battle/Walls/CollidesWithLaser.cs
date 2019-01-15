using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using DT.Game.Battle.Lasers;
using DTAnimatorStateMachine;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public enum CollidesWithLaserType {
		DestroyLaser,
		ReflectLaser,
		HasDelegate
	}

	public interface ILaserCollisionDelegate {
		void HandleLaserHit(Laser laser);
	}

	public class CollidesWithLaser : MonoBehaviour {
		// PRAGMA MARK - Internal
		[Header("Properties")]
		[SerializeField]
		private CollidesWithLaserType type_ = CollidesWithLaserType.ReflectLaser;

		private Rigidbody rigidbody_;

		private bool collisionDelegateCached_ = false;
		private ILaserCollisionDelegate collisionDelegate_ = null;
		private ILaserCollisionDelegate CollisionDelegate_ {
			get {
				if (!collisionDelegateCached_) {
					collisionDelegate_ = this.GetOnlyComponentInChildren<ILaserCollisionDelegate>();
					collisionDelegateCached_ = true;
				}

				return collisionDelegate_;
			}
		}

		private void Awake() {
			rigidbody_ = this.GetRequiredComponentInParent<Rigidbody>();
		}

		private void OnTriggerEnter(Collider collider) {
			Laser laser = collider.gameObject.GetComponentInParent<Laser>();
			if (laser == null) {
				return;
			}

			switch (type_) {
				case CollidesWithLaserType.DestroyLaser:
				{
					laser.HandleHit(destroy: true);
					break;
				}
				case CollidesWithLaserType.ReflectLaser:
				default:
				{
					laser.Ricochet(-this.transform.right, rigidbody_.velocity, context: this);
					break;
				}
				case CollidesWithLaserType.HasDelegate:
				{
					if (CollisionDelegate_ == null) {
						Debug.LogWarning("No collision delegate to HandleLaserHit!");
						break;
					}

					CollisionDelegate_.HandleLaserHit(laser);
					break;
				}
			}
		}
	}
}