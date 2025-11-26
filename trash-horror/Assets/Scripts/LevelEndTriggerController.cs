using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelEndController : MonoBehaviour
{
    public string nextScene;
    public float fadeOutDelay = 1f;
    public FloatVariable lightFadeModifier;
    
    private AudioSource audioSource;
    private bool fadingOut = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InventoryController.Instance.HasKey())
        {
            InventoryController.Instance.RemoveKey();
            audioSource.Play(0);
            Time.timeScale = 0.1f;
            fadingOut = true;
            StartCoroutine(SceneLoadAfterDelay());
            StartCoroutine(FadeOutOverTime());
        }
    }

    private IEnumerator SceneLoadAfterDelay()
    {
        yield return new WaitForSeconds(fadeOutDelay * Time.timeScale);
        Time.timeScale = 1f;
        fadingOut = false;
        lightFadeModifier.value = 10;
        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    private IEnumerator FadeOutOverTime()
    {
        float initial = lightFadeModifier.value;
        for (float t = 0f; t < fadeOutDelay; t += Time.deltaTime / Time.timeScale)
        {
            lightFadeModifier.value = Mathf.Lerp(initial, 0, t / fadeOutDelay);
            yield return null;
        }
    }
}
