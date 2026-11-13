using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using STOP_MODE = FMOD.Studio.STOP_MODE;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("FMOD Music Event")]
    [SerializeField] private EventReference musicEvent;

    [Header("FMOD Parameter")]
    [SerializeField] private string musicParameter = "Music State";

    [Header("Parameter Values")]
    [SerializeField] private float titleScreenMusic = 0f;
    [SerializeField] private float gameplayMusic = 1f;

    private EventInstance currentMusic;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        // Create one music instance
        currentMusic = RuntimeManager.CreateInstance(musicEvent);

        // Set the initial music before starting
        SetMusicForScene(SceneManager.GetActiveScene().name);

        currentMusic.start();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetMusicForScene(scene.name);
    }

    private void SetMusicForScene(string sceneName)
    {
        if (!currentMusic.isValid())
            return;

        if (sceneName == "0 (1)")
        {
            currentMusic.setParameterByName(
                musicParameter,
                titleScreenMusic
            );

            Debug.Log("Music changed to Title Screen");
        }
        else
        {
            currentMusic.setParameterByName(
                musicParameter,
                gameplayMusic
            );

            Debug.Log("Music changed to Gameplay");
        }
    }

    private void OnDestroy()
    {
        if (Instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (currentMusic.isValid())
        {
            currentMusic.stop(STOP_MODE.ALLOWFADEOUT);
            currentMusic.release();
        }

        Instance = null;
    }
}