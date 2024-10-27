using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BigSlimeController : MonoBehaviour
{
    public float tiempoDeMuerte = 0.5f;
    public float speed;
    public float tiempoEspera = 2f;
    public int Salud;
    public GameObject Albin;
    public GameObject AlbinSlime;
    public HuntController Controller;
    public AlbinControl Player1;
    public AlbinSlimesControl Player2;
    public Animator Anims;
    public float Magnitud;

    private void Start()
    {
        Controller = GameObject.Find("Controlador").GetComponent<HuntController>();
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        Player1 = GameObject.Find("Albin").GetComponent<AlbinControl>();
        Vector3 v = Player1.transform.position - transform.position;
        Magnitud = v.magnitude;
    }
    // Update is called once per frame
    void Update()
    {
        // Mueve al slime a la ubicación requerida
        if (Albin != null && Albin.activeSelf)
        {
            Player1.Ubic();
            Vector3 v = Player1.transform.position - transform.position;
            Magnitud = v.magnitude;
            if (Player1.objetivo.x != transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player1.objetivo, speed * Time.deltaTime);
            }

            if (Magnitud < 1.5f)
            {
                speed = 0;
                Anims.SetBool("Attack", true);
            }
            else if (Magnitud > 1.5f)
            {
                speed = 0.4f;
                Anims.SetBool("Attack", false);
            }
        }
        if (!Albin.activeSelf)
        {
            Debug.Log("Slime activo");
            Player2.Ubic();
            Vector3 v = Player1.transform.position - transform.position;
            Magnitud = v.magnitude;
            if (Player2.objetivo.x != transform.position.x)
            {
                transform.position = Vector3.MoveTowards(transform.position, Player2.objetivo, speed * Time.deltaTime);
            }

            if (Magnitud < 1.5f)
            {
                speed = 0;
                Anims.SetBool("Attack", true);
            }
            else if (Magnitud > 1.5f)
            {
                speed = 0.5f;
                Anims.SetBool("Attack", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Disparo")
        {
            Salud--;
            if (Salud == 0)
            {
                speed = 0;
                Anims.SetBool("Death", true);
                Invoke("DestruirGameObject", tiempoDeMuerte);
                Controller.Cazar();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DisparoEspecial")
        {
            speed = 0;
            Anims.SetBool("Death", true);
            Invoke("DestruirGameObject", tiempoDeMuerte);
            Controller.Cazar();
        }
    }

    void DestruirGameObject()
    {
        Destroy(gameObject);
    }
}
