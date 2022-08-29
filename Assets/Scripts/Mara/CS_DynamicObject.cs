using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_DynamicObject : MonoBehaviour
{
    public float mWeight = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PullObject(Vector3 pullForce)
    {
        //transform.position += pullForce;
        if (GetComponent<Rigidbody2D>())
        {
            Vector2 pullDir2 = new Vector2(pullForce.normalized.x, 0.0f);
            GetComponent<Rigidbody2D>().velocity /= 2.0f;// new Vector2(0.0f, 0.0f);
            GetComponent<Rigidbody2D>().AddForce(pullDir2 * 100.0f / mWeight);
        }
    }
}
