using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    void RandomMe()
    {
        if (gameObject.TryGetComponent(out Image img))
        {
            img.color = new Color(Random.value, Random.value, Random.value, 1f);
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        RandomMe();
    }
#endif
}
