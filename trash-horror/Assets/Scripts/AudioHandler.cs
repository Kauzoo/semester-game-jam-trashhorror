using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    public AudioSource trap_damage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayTrapDamage()
    {
        trap_damage.PlayOneShot(trap_damage.clip);
    }
}
