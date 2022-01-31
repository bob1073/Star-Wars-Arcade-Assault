using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyVFX : MonoBehaviour
{
    public float lifeTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= 1.0f*Time.deltaTime;
        if(lifeTime <= 0.0f)
        {
            Destroy(gameObject);
        }
    }
}
