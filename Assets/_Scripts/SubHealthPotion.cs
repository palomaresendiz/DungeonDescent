using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubHealthPotion : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Destroy(gameObject);

            if (HealthTracker.instance != null)
            {
                HealthTracker.instance.decrementHearts();
            }
        }
    }
}
