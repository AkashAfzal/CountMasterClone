using System;
using System.Threading.Tasks;
using DG.Tweening;
// using ElephantSDK;
// using GameAnalyticsSDK;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


namespace GameAssets.GameSet.GameDevUtils.Managers
{

	public enum GameState
	{

		MainMenu      = 0,
		Gameplay      = 1,
		Pause         = 2,
		Win           = 3,
		Fail          = 4,
		FinalMomentum = 5

	}

	public class GameManager : Singleton<GameManager>
	{

		[SerializeField] GameState gameCurrentState = GameState.MainMenu;
		
		public GameState GameCurrentState
		{
			get => gameCurrentState;
			private set
			{
				gameCurrentState = value;
				OnGameStateChangedEvent?.Invoke(gameCurrentState);
			}
		}


		static readonly UnityEvent<GameState> OnGameStateChangedEvent = new UnityEvent<GameState>();
		
		public LevelManager levelManager;
		public UIManager    uiManager;

		[SerializeField] float        beforeLevelCompleteDelay;
		[SerializeField] float        beforeLevelFailDelay;
		


		public int InfinityCurrentLevel    => levelManager.InfinityCurrentLevelNumber();
		public int NotInfinityCurrentLevel => levelManager.CurrentPlayLevelNumber();
		bool       IsLevelCompleteNotInvoke;


		public delegate void OnMainMenu();

		public static event OnMainMenu onMainMenuEvent;

		public delegate void OnGamePlay();

		public static event OnGamePlay onGamePlayEvent;

		public delegate void OnPause();

		public static event OnPause onPauseEvent;


		public delegate void OnFinalMomentum();

		public static event OnFinalMomentum onFinalMomentumEvent;

		public delegate void OnComplete();

		public static event OnComplete onCompleteEvent;

		public delegate void OnFailed();

		public static event OnFailed onFailedEvent;
		

		void OnEnable()
		{
			Application.targetFrameRate = 60;
			OnGameStateChangedEvent.AddListener(OnGameStateChanged);
		}

		void OnDisable()=> OnGameStateChangedEvent.RemoveAllListeners();

		void Start()
		{
			LoadLevelAtStart(); 
			ChangeGameState(gameCurrentState);
		}


		public void ChangeGameState(GameState state)
		{
			GameCurrentState = state;
		}

		async void OnGameStateChanged(GameState state)
		{
			switch (state)
			{
				case GameState.MainMenu:
					Time.timeScale = 1;
					uiManager.EnableUIScreen(GameState.MainMenu);
					onMainMenuEvent?.Invoke();
					break;

				case GameState.Gameplay:
					Time.timeScale = 1;
					uiManager.EnableUIScreen(GameState.Gameplay);
					onGamePlayEvent?.Invoke();
					break;

				case GameState.Pause:
					Time.timeScale = 0;
					uiManager.EnableUIScreen(GameState.Pause);
					onPauseEvent?.Invoke();
					break;

				case GameState.FinalMomentum:
					uiManager.EnableUIScreen(GameState.FinalMomentum);
					onFinalMomentumEvent?.Invoke();
					break;

				case GameState.Win:
					if (IsLevelCompleteNotInvoke) return;
					IsLevelCompleteNotInvoke = true;
					await Task.Delay(TimeSpan.FromSeconds(beforeLevelCompleteDelay));
					SoundManager.Instance.PlayOneShot(SoundManager.Instance.winPanelOpen, 1);
					uiManager.EnableUIScreen(GameState.Win);
					
					// Elephant.LevelCompleted(InfinityCurrentLevel);
					// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "LevelComplete", InfinityCurrentLevel);
					HapticFeedback.Generate(UIFeedbackType.Success);
					NextUnlockLevel();
					onCompleteEvent?.Invoke();
					break;

				case GameState.Fail:
					Camera.main.DOShakePosition(0.5f, new Vector3(1, 1, 1));
					await Task.Delay(TimeSpan.FromSeconds(beforeLevelFailDelay));
					SoundManager.Instance.PlayOneShot(SoundManager.Instance.failClip, 1);
					uiManager.EnableUIScreen(GameState.Fail);

					// Elephant.LevelFailed(InfinityCurrentLevel);
					// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "LevelFail", InfinityCurrentLevel);
					HapticFeedback.Generate(UIFeedbackType.Error);
					onFailedEvent?.Invoke();
					break;
			}
		}


		private void LoadLevelAtStart()
		{
			levelManager.LoadLevelAtStart();
			// Elephant.LevelStarted(InfinityCurrentLevel);
			// GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "LevelStart", InfinityCurrentLevel);
		}


		void NextUnlockLevel()
		{
			levelManager.NextUnlockLevel();
		}


		public void TabToContinue()
		{
			SoundManager.Instance.PlayButtonSound();
			ChangeGameState(GameState.Gameplay);
		}

		public void Restart()
		{
			SoundManager.Instance.PlayButtonSound();
			//followCameraAsset.ResetCameraValue();
			SceneManager.LoadScene(0);
		}


		public void ExperimentLevel()
		{
			levelManager.NextUnlockLevel();
			SoundManager.Instance.PlayButtonSound();
			//followCameraAsset.ResetCameraValue();
			SceneManager.LoadScene(0);
		}

	}


}