using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBorder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        /*else
        {
            Destroy(collision.gameObject);
        }*/
    }
}