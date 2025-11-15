using UnityEngine;
using System.Collections;

public class ButtonController : InteractableDevice
{
    public float deactivationDelay = 10f;
    private Coroutine deactivationRoutine;

    public override void Interact(PlayerBehaviour player)
    {
        ChangeState(true);

        if (deactivationRoutine != null)
        {
            StopCoroutine(deactivationRoutine);
        }

        deactivationRoutine = StartCoroutine("DeactivateAfterDelay");
    }

    private IEnumerator DeactivateAfterDelay()
    {
        yield return new WaitForSeconds(deactivationDelay);
        ChangeState(false);
    }
}
