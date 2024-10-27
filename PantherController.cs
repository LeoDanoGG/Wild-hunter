using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PantherController : MonoBehaviour
{
    public float tiempoDeMuerte = 0.5f;
    public float speed;
    public float tiempoEspera = 2f;
    public GameObject Albin;
    public GameObject AlbinSlime;
    public HuntController Controller;
    public AlbinControl AlbinPlayer;
    public AlbinSlimesControl SlimePlayer;
    public Button Player;
    public Button Player2;
    public Animator Anims;
    public float Magnitud;
    // Update is called once per frame
    private void Start()
    {
        Controller = GameObject.Find("Controlador").GetComponent<HuntController>();
        if (transform.position.x > 0)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        AlbinPlayer = GameObject.Find("Albin").GetComponent<AlbinControl>();
        Vector3 v = AlbinPlayer.transform.position - transform.position;
        Magnitud = v.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        // Mueve a la pantera a la ubicación requerida
        Vector3 v = AlbinPlayer.transform.position - transform.position;
        Magnitud = v.magnitude;
        AlbinPlayer.Ubic();
        if (AlbinPlayer.objetivo.x != transform.position.x)
        {
            transform.position = Vector3.MoveTowards(transform.position, AlbinPlayer.objetivo, speed * Time.deltaTime);
        }

        if (Magnitud < 1.6f)
        {
            speed = 0;
            Anims.SetBool("Attack", true);
        }
        else if (Magnitud > 1.6f)
        {
            speed = 5;
            Anims.SetBool("Attack", false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Disparo")
        {
            speed = 0;
            Anims.SetBool("Death", true);
            Invoke("DestruirGameObject", tiempoDeMuerte);
            Controller.Cazar();
        }
    }

    // Llama al método para acabar la partida
    void DestruirGameObject()
    {
        Destroy(gameObject);
        Controller.Text[1].text = "Objetivo: Cazado";
        Controller.Pause.text = "¡Caza exitosa!";
        Controller.Final();
    }
}
