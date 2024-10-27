using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disparo : MonoBehaviour
{
    // Lógica de los disparos
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "suelo")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("Cazado");
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "PantherEnemy")
        {
            Debug.Log("Objetivo especial cazado");
            Destroy(gameObject);
        }
    }
}
