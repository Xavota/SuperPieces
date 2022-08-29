using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CS_Arrow : MonoBehaviour
{
    public CS_BowShot mBowShotParent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
        {
            mBowShotParent.ArrowCollides(other.gameObject);
        }
    }
}
