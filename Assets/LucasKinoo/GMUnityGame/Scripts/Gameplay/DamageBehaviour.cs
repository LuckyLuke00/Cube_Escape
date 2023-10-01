using UnityEngine;

public class DamageBehaviour : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();

        if (player != null) player.Kill();
    }
}