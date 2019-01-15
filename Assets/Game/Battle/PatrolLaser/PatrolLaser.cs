using System;
using System.Collections;
using UnityEngine;

using DT.Game.GameModes;
using DTAnimatorStateMachine;
using DTCommandPalette;
using DTEasings;
using DTObjectPoolManager;
using InControl;

namespace DT.Game.Battle {
	public class PatrolLaser : MonoBehaviour, IRecycleSetupSubscriber, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public void Init(Direction facingDirection, Vector3[] worldPatrolPoints) {
			if (worldPatrolPoints == null) {
				throw new ArgumentNullException();
			}

			if (worldPatrolPoints.Length <= 1) {
				throw new ArgumentException("worldPatrolPoints - should have two points or more!");
			}

			facingDirection_ = facingDirection;
			rotationContainer_.transform.rotation = Quaternion.Euler(0.0f, DirectionUtil.AngleInDegreesFrom(Direction.UP, facingDirection_), 0.0f);
			patrolDirection_ = EnumUtil.Random<HorizontalDirection>();
			worldPatrolPoints_ = worldPatrolPoints;

			int startIndex = patrolDirection_ == HorizontalDirection.RIGHT ? 0 : worldPatrolPoints_.Length - 1;
			this.transform.position = worldPatrolPoints_[startIndex];
		}


		// PRAGMA MARK - IRecycleSetupSubscriber Implementation
		void IRecycleSetupSubscriber.OnRecycleSetup() {
			laserContainer_.SetActive(false);
			enabled_ = false;
			GameModeIntroView.OnIntroFinishedPossibleMock += HandleIntroFinished;
			Reset();
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			GameModeIntroView.OnIntroFinishedPossibleMock -= HandleIntroFinished;
			this.StopAllCoroutines();
		}


		// PRAGMA MARK - Internal
		private const float kStartDelaySec = 1.5f;

		private const float kTimeMaxSec = 20.0f;

		private static readonly FloatRange kSpeedMin = new FloatRange(1.0f, 1.4f);
		private static readonly FloatRange kSpeedMax = new FloatRange(2.7f, 3.0f);

		private const float kGapMin = 0.3f;
		private const float kGapMax = 0.7f;

		// NOTE (darren): gap can only occur between (Buffer..1.0f - Buffer)
		private const float kGapBuffer = 0.1f;

		[Header("Outlets")]
		[SerializeField]
		private GameObject rotationContainer_;
		[SerializeField]
		private GameObject laserContainer_;

		[Header("Read-Only")]
		[SerializeField, ReadOnly]
		private Direction facingDirection_;
		[SerializeField, ReadOnly]
		private HorizontalDirection patrolDirection_;
		[SerializeField, ReadOnly]
		private bool enabled_ = false;
		[SerializeField, ReadOnly]
		private float speed_ = 0.0f;
		[SerializeField, ReadOnly]
		private float gap_ = 1.0f;
		[SerializeField, ReadOnly]
		private float timeAlive_ = 0.0f;

		[Space]
		[SerializeField, ReadOnly]
		private Vector3[] worldPatrolPoints_;

		private float speedMin_;
		private float speedMax_;

		private void Awake() {
			Reset();
		}

		private void HandleIntroFinished() {
			GameModeIntroView.OnIntroFinishedPossibleMock -= HandleIntroFinished;

			this.DoAfterDelay(kStartDelaySec, () => {
				laserContainer_.SetActive(true);
				enabled_ = true;
				StartPatrolling();
			});
		}

		private void StartPatrolling() {
			int nextIndex = patrolDirection_ == HorizontalDirection.RIGHT ? 1 : worldPatrolPoints_.Length - 2;
			this.StartCoroutine(PatrolToCoroutine(index: nextIndex));
		}

		private IEnumerator PatrolToCoroutine(int index) {
			Vector3 targetPatrolPoint = worldPatrolPoints_[index];
			Vector3 startPosition = this.transform.position;
			float targetDistance = (targetPatrolPoint - startPosition).magnitude;

			// NOTE (darren): decide where the gap should be (0..1)
			float gapSize = gap_;
			float gapStart = UnityEngine.Random.Range(kGapBuffer, 1.0f - kGapBuffer - gapSize);

			for (float distance = 0.0f; distance <= targetDistance; distance += speed_ * Time.deltaTime) {
				float p = Easings.Interpolate(distance / targetDistance, EaseType.QuadraticEaseInOut);
				this.transform.position = Vector3.Lerp(startPosition, targetPatrolPoint, p);

				bool inGap = p >= gapStart && p <= gapStart + gapSize;
				laserContainer_.SetActive(!inGap);
				yield return null;
			}

			this.transform.position = targetPatrolPoint;

			int nextIndex = worldPatrolPoints_.WrapIndex(patrolDirection_ == HorizontalDirection.RIGHT ? index + 1 : index - 1);
			this.StartCoroutine(PatrolToCoroutine(nextIndex));
		}

		private void Update() {
			if (!enabled_) {
				return;
			}

			timeAlive_ += Time.deltaTime;
			speed_ = Mathf.Lerp(speedMin_, speedMax_, timeAlive_ / kTimeMaxSec);
			gap_ = Mathf.Lerp(kGapMax, kGapMin, timeAlive_ / kTimeMaxSec);
		}

		private void Reset() {
			speedMin_ = kSpeedMin.Next();
			speedMax_ = kSpeedMax.Next();

			gap_ = kGapMax;
			timeAlive_ = 0.0f;
			speed_ = 0.0f;
			rotationContainer_.transform.rotation = Quaternion.identity;
		}
	}
}