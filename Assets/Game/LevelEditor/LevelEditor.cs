using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using DTAnimatorStateMachine;
using DTObjectPoolManager;
using InControl;

#if UNITY_EDITOR
using UnityEditor;
#endif

using DT.Game.Battle;
using DT.Game.Battle.Walls;
using DT.Game.Transitions;

namespace DT.Game.LevelEditor {
	public class LevelEditor : MonoBehaviour, IRecycleCleanupSubscriber {
		// PRAGMA MARK - Public Interface
		public static InputDevice InputDevice {
			get; private set;
		}

		public LevelEditorCursor Cursor {
			get { return cursor_; }
		}

		public void SetObjectToPlace(GameObject prefab, Action<GameObject> instanceInitialization = null, IList<AttributeData> attributeDatas = null) {
			CleanupPlacer();

			if (prefab.GetComponent<Wall>() != null) {
				placerObject_ = ObjectPoolManager.Create(GamePrefabs.Instance.WallPlacerPrefab, parent: this.gameObject);
			} else if (prefab.GetComponent<LevelEditorPlayerSpawnPoint>() != null) {
				placerObject_ = ObjectPoolManager.Create(GamePrefabs.Instance.PlayerSpawnPointPlacerPrefab, parent: this.gameObject);
			} else {
				placerObject_ = ObjectPoolManager.Create(GamePrefabs.Instance.PlatformPlacerPrefab, parent: this.gameObject);
			}
			var placer = placerObject_.GetComponent<IPlacer>();
			placer.Init(prefab, dynamicArenaData_, undoHistory_, inputDevice_, this, instanceInitialization, attributeDatas);
		}

		public void Init(InputDevice inputDevice, Action exitCallback) {
			InputDevice = inputDevice;
			inputDevice_ = inputDevice;

			cursorContextMenu_ = new CursorContextMenu(inputDevice, this);
			levelEditorMenu_ = new LevelEditorMenu(inputDevice, exitCallback, SaveDataToEditor, LoadArenaToEdit);

			cursor_ = ObjectPoolManager.Create<LevelEditorCursor>(GamePrefabs.Instance.LevelEditorCursorPrefab, parent: this.gameObject);
			cursor_.Init(inputDevice);

			var newArena = ScriptableObject.CreateInstance<ArenaConfig>();
			LoadArenaToEdit(newArena);

			SetObjectToPlace(GamePrefabs.Instance.LevelEditorObjects.FirstOrDefault());
		}


		// PRAGMA MARK - IRecycleCleanupSubscriber Implementation
		void IRecycleCleanupSubscriber.OnRecycleCleanup() {
			if (cursor_ != null) {
				ObjectPoolManager.Recycle(cursor_);
				cursor_ = null;
			}

			CleanupPlacer();

			if (undoHistory_ != null) {
				undoHistory_.Dispose();
				undoHistory_ = null;
			}

			if (levelEditorMenu_ != null) {
				levelEditorMenu_.Dispose();
				levelEditorMenu_ = null;
			}

			if (cursorContextMenu_ != null) {
				cursorContextMenu_.Dispose();
				cursorContextMenu_ = null;
			}
		}


		// PRAGMA MARK - Internal
		#if UNITY_EDITOR
		private static readonly string kDefaultNameFormat = "CustomArena{0}.asset";
		#endif

		[Header("Outlets")]
		[SerializeField]
		private DynamicArenaView dynamicArenaView_;

		private LevelEditorCursor cursor_;
		private GameObject placerObject_;
		private LevelEditorMenu levelEditorMenu_;
		private CursorContextMenu cursorContextMenu_;
		private InputDevice inputDevice_;

		private ArenaConfig editArena_;
		private UndoHistory undoHistory_;
		private DynamicArenaData dynamicArenaData_ = new DynamicArenaData();

		private Transition arenaViewTransition_;
		private Transition ArenaViewTransition_ {
			get { return arenaViewTransition_ ?? (arenaViewTransition_ = new Transition(dynamicArenaView_.gameObject).SetDynamic(true)); }
		}

		private void Update() {
			// HACK (darren): do deletion better
			if (inputDevice_.Action2.WasPressed) {
				Vector3 cursorPosition = cursor_.transform.position;
				foreach (var dynamicObject in dynamicArenaData_.Objects) {
					Vector2 position = (dynamicObject.Position - (dynamicObject.LocalScale / 2.0f)).Vector2XZValue();
					Vector2 size = dynamicObject.LocalScale.Vector2XZValue();
					Rect rect = new Rect(position, size);
					if (rect.Contains(cursorPosition.Vector2XZValue())) {
						dynamicArenaData_.RemoveObject(dynamicObject);
						return;
					}
				}
			}
		}

		private void SaveDataToEditor() {
			editArena_.SaveDynamicArenaDataJson(dynamicArenaData_.Serialize());

			// if UNITY_EDITOR, check that the asset already exists - use editorUtility to save changes instead
			#if UNITY_EDITOR
				string assetsBasedDirectoryPath = "Assets/CustomArenas";

				List<ArenaConfig> arenaConfigs = AssetDatabaseUtil.AllAssetsOfType<ArenaConfig>();
				if (arenaConfigs.Any(ac => ac == editArena_)) {
					EditorUtility.SetDirty(editArena_);
					AssetDatabase.SaveAssets();
					return;
				}

				// if not new asset, save to new asset
				int index = 0;
				string path;
				do {
					path = Path.Combine(assetsBasedDirectoryPath, string.Format(kDefaultNameFormat, index));
					index++;
				} while (AssetDatabase.LoadAssetAtPath(path, typeof(UnityEngine.Object)) != null);

				string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path);
				AssetDatabase.CreateAsset(editArena_, assetPathAndName);
				AssetDatabase.SaveAssets();
			#else
				string directoryPath = Path.Combine(Application.dataPath, "CustomArenas");
				Directory.CreateDirectory(directoryPath);

				var filenames = new HashSet<string>(Directory.GetFiles(directoryPath));

				string filename = editArena_.name;
				bool findDefaultName = string.IsNullOrEmpty(filename);
				if (findDefaultName) {
					int index = 1;
					do {
						filename = string.Format("CustomArena{0}", index);
						index++;
					} while (filenames.Contains(filename + ".asset"));
				}


				File.WriteAllText(Path.Combine(directoryPath, filename + ".asset"), JsonUtility.ToJson(editArena_));
			#endif
		}

		private void CleanupPlacer() {
			if (placerObject_ != null) {
				ObjectPoolManager.Recycle(placerObject_);
				placerObject_ = null;
			}
		}

		private void HandleArenaViewRefreshed() {
			ArenaViewTransition_.AnimateIn(instant: true);
		}

		private void LoadArenaToEdit(ArenaConfig arenaConfig) {
			editArena_ = arenaConfig;

			string dynamicArenaDataJson = editArena_.GetDynamicArenaDataJson();
			if (string.IsNullOrEmpty(dynamicArenaDataJson)) {
				dynamicArenaData_ = new DynamicArenaData();
			} else {
				dynamicArenaData_ = JsonUtility.FromJson<DynamicArenaData>(dynamicArenaDataJson);
			}
			undoHistory_ = new UndoHistory(dynamicArenaData_, inputDevice_);

			dynamicArenaView_.Init(dynamicArenaData_, editArena_.Prefab);
			dynamicArenaView_.OnViewRefreshed += HandleArenaViewRefreshed;
		}
	}
}