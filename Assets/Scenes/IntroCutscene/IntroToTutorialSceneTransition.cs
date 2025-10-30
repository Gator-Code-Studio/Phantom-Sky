using UnityEngine;

public class IntroToTutorialSceneTransition : MonoBehaviour
{
    private void Start()
    {
        // See? Simple. No objects, no instances.
        SceneLoader.LoadNextScene();
    }
}