using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialArrow : MonoBehaviour
{
    // L�gica de los disparos
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "suelo")
        {
            Destroy(gameObject);
        }
    }
}
