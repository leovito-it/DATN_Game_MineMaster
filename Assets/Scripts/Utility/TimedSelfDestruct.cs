using UnityEngine;
using System.Collections;

public class TimedSelfDestruct : MonoBehaviour
{
	// After this time, the object will be destroyed
	public float timeToDestruction;

	void Start ()
	{
		Destroy(gameObject, timeToDestruction);
	}
}
