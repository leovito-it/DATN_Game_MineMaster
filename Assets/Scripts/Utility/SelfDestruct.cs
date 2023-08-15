using SFX;
using System.Collections;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // After this time, the object will be destroyed
    public float timeToDestruction;
    public AudioClip destructionClip;

    void Start()
    {
        this.gameObject.PlayEF(destructionClip);
        Destroy(gameObject, timeToDestruction);
    }
}
