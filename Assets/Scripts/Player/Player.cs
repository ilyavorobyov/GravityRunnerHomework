using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction PlayerDied;

    public void Die()
    {
        Debug.Log("Die");
        PlayerDied.Invoke();
    }
}