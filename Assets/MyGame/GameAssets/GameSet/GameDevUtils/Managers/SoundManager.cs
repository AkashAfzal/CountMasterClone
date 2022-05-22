using System.Collections;
using UnityEngine;


namespace GameAssets.GameSet.GameDevUtils.Managers
{


	public class SoundManager : MonoBehaviour
	{

		public static    SoundManager Instance { get; private set; }
		[SerializeField] AudioSource  bgSoundSource;
		[SerializeField] AudioSource  sFXSoundSource;
		[SerializeField] AudioSource  stairsUpSource;

		public AudioClip bgClip;
		public AudioClip buttonClip;
		public AudioClip winPanelOpen;
		public AudioClip confettiClip;
		public AudioClip failClip;
		public AudioClip constructPyramid;

		[SerializeField] AudioClip stairSound;
		
		[SerializeField] AudioClip[] growSounds;
		[SerializeField] AudioClip[] deadSounds;
		[SerializeField] AudioClip[] runSounds;
		bool                         IsRunSoundsPlaying;


		void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				DestroyImmediate(gameObject);
			}
		}

		void Start()
		{
			if (bgSoundSource.isPlaying)
				return;
			bgSoundSource.clip = bgClip;
			bgSoundSource.loop = true;
			bgSoundSource.Play();
		}


		public void SetBgSoundSetting(bool toggle)
		{
			bgSoundSource.mute = !toggle;
		}

		public void SetSfxSoundSetting(bool toggle)
		{
			sFXSoundSource.mute = !toggle;
			stairsUpSource.mute = !toggle;
		}

		public void PlayOneShot(AudioClip clip, float volume)
		{
			sFXSoundSource.PlayOneShot(clip, volume);
		}
		
		public void PlayDeadSound()
		{
			sFXSoundSource.PlayOneShot(deadSounds[Random.Range(0, deadSounds.Length)]);
		}
		
		public void PlayGrowSound()
		{
			sFXSoundSource.PlayOneShot(growSounds[Random.Range(0, growSounds.Length)]);
		}

		public void PlayRunSounds(bool play)
		{
			if (play)
			{
				IsRunSoundsPlaying = play;
				StartCoroutine(PlayRunSoundsCo());
			}
			else
			{
				IsRunSoundsPlaying = false;
			}	
		}

		public void PlayStairsUpSound()
		{
			stairsUpSource.PlayOneShot(stairSound);
			stairsUpSource.pitch += 0.1f;
		}

		IEnumerator PlayRunSoundsCo()
		{
			while (IsRunSoundsPlaying)
			{
				sFXSoundSource.PlayOneShot(runSounds[Random.Range(0, runSounds.Length)],0.8f);
				yield return new WaitForSeconds(0.25f);
			}
		}

		public void PlayButtonSound() => sFXSoundSource.PlayOneShot(buttonClip, 1);

	}


}