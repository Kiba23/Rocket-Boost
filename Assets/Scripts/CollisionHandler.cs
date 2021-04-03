using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] float levelLoadDelay = 0.6f;
    [SerializeField] AudioClip obstacleCrashSound;
    [SerializeField] AudioClip successFinishSound;

    AudioSource audioSource;

    bool isSoundPlayed = false; // for not to playing Finish and Crash sounds together

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                Debug.Log("You've hit the friendly object");
                break;
            case "Finish":
                LoadSound(successFinishSound);
                StartLevelLoadSequence("LoadNextLevel");
                break;
            default:
                LoadSound(obstacleCrashSound);
                StartLevelLoadSequence("ReloadScene");
                break;
        }
    }

    void LoadSound(AudioClip sound)
    {
        if (!isSoundPlayed)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(sound, 0.3f);
            isSoundPlayed = true;
        }
    }

    void StartLevelLoadSequence(string scene)
    {
        GetComponent<Movement>().enabled = false;
        Invoke(scene, levelLoadDelay);
    }

    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadSceneAsync(nextSceneIndex);
    }

    void ReloadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadSceneAsync(currentSceneIndex);
    }
}
