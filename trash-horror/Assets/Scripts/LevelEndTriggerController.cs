using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndController : MonoBehaviour
{
    public string nextScene;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && InventoryController.Instance.HasKey())
        {
            InventoryController.Instance.RemoveKey();
            SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
        }
    }
}
