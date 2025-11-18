using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public StringVariable levelNameData;
    
    public void Respawn()
    {
        SceneManager.LoadScene(levelNameData.value);
    }

    public void Quit()
    {
        Application.Quit();
    }
}