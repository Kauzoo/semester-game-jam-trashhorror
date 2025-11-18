using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public StringVariable levelNameData;
    public GameEvent onRespawn;
    
    public void Respawn()
    {
        SceneManager.LoadScene(levelNameData.value);
        onRespawn.Raise();
    }

    public void Quit()
    {
        Application.Quit();
    }
}