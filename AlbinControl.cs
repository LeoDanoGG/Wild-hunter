using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class AlbinControl : MonoBehaviour
{
    public float intervalo = 0.5f; // Intervalo en segundos
    public float Cooldown = 2f; // Tiempo en segundos
    public float UltimoDisparo;
    public float UltimoImpulso;
    public TextMeshProUGUI Alerta;

    public float Velocidad;
    public int Salud;
    public int Ammun;
    public Transform PlayerMove;
    public GameObject Flecha;
    public GameObject FlechaEspecial;
    public HuntController Controller;
    public HUD hud;
    public bool PlayerMoving;
    private bool Alive = true;
    private bool PuntaR = false;
    private bool PuntaL = false;
    public bool Special = false;
    public Animator Anims;
    public GameObject HeartContainer;
    public Vector3 objetivo;
    // Start is called before the first frame update
    void Start()
    {
        Salud = 3;
        Ammun = 3;
        Alerta.enabled = false;
    }
    public void Ubic()
    {
        objetivo = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
       float Moving = Input.GetAxis("Horizontal");
        // Si Albin (con Limo) está vivo coordina las acciones
        if (Alive)
        {
            objetivo = transform.position;
            // Movimiento
            if (Input.GetKey(KeyCode.D))
            {
                PlayerMoving = true;
                PlayerMove.Translate(new Vector2(Velocidad, 0) * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.D))
            {
                PlayerMoving = false;
            }
            if (Input.GetKey(KeyCode.A))
            {
                PlayerMoving = true;
                PlayerMove.Translate(new Vector2(-Velocidad, 0) * Time.deltaTime);
            }
            else if (Input.GetKeyUp(KeyCode.A))
            {
                PlayerMoving = false;
            }
            if (Moving > 0f && Input.GetKey(KeyCode.D))
            {
                Anims.SetBool("MoveR", true);
            }
            if (Moving < 0f && Input.GetKey(KeyCode.D))
            {
                Anims.SetBool("MoveR", true);
                Anims.SetBool("MoveL", true);
                Anims.CrossFade("moveR", 0);
            }
            if (Moving < 0f && Input.GetKey(KeyCode.A))
            {
                Anims.SetBool("MoveL", true);
            }
            if (Moving > 0f && Input.GetKey(KeyCode.A))
            {
                Anims.SetBool("MoveL", true);
                Anims.SetBool("MoveR", true);
                Anims.CrossFade("moveL", 0);
            }
            else if (Moving == 0f)
            {
                Anims.SetBool("MoveR", false);
                Anims.SetBool("MoveL", false);
            }
            // Impulso
            if (Time.time - UltimoImpulso >= intervalo * 10)
            {
                if (Moving != 0f && Input.GetKeyDown(KeyCode.Space))
                {
                    Impulso();
                }
            }
            // Disparo especial
            if (Input.GetKeyDown(KeyCode.Space) && Moving == 0f && Special == false)
            {
                Special = true;
                hud.SpecialAmmun[Ammun-1].GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && Moving == 0f && Special != false)
            {
                Special = false;
                hud.SpecialAmmun[Ammun-1].GetComponent<SpriteRenderer>().color = Color.white;
            }
            // Limitación de avance
            if (PlayerMove.transform.position.x > 7)
            {
                Anims.SetBool("MoveR", false);
                PlayerMove.transform.position = (new Vector2(7, transform.position.y));
            }
            if (PlayerMove.transform.position.x < -7)
            {
                Anims.SetBool("MoveL", false);
                PlayerMove.transform.position = (new Vector2(-7, transform.position.y));
            }

            // Verificar si ha pasado el cooldown para coordinar los disparos
            if (Time.time - UltimoDisparo >= intervalo)
            {
                // Verificar si ha pasado el cooldown para volver a disparar
                if (Anims.GetCurrentAnimatorStateInfo(0).IsName("attackR"))
                {
                    if (Time.time - Cooldown + 0.5f >= UltimoDisparo)
                    {
                        PuntaR = true;
                    }
                }
                if (Anims.GetCurrentAnimatorStateInfo(0).IsName("attackL"))
                {
                    if (Time.time - Cooldown + 0.5f >= UltimoDisparo)
                    {
                        PuntaL = true;
                    }
                }
                if (Anims.GetCurrentAnimatorStateInfo(0).IsName("attackL") && Input.GetKey(KeyCode.RightArrow))
                {
                    Anims.SetBool("AttackL", false);
                    PuntaL = false;
                    Anims.SetBool("AttackR", true);
                }
                if (Anims.GetCurrentAnimatorStateInfo(0).IsName("attackR") && Input.GetKey(KeyCode.LeftArrow))
                {
                    Anims.SetBool("AttackR", false);
                    PuntaR = false;
                    Anims.SetBool("AttackL", true);
                }
                if (Anims.GetCurrentAnimatorStateInfo(0).IsName("finishL") || Anims.GetCurrentAnimatorStateInfo(0).IsName("finishR"))
                {
                    PuntaR = false;
                    PuntaL = false;
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    Velocidad = 0.8f;
                    Anims.SetBool("AttackL", false);
                    Anims.SetBool("AttackR", false);
                }
                // Control de los disparos
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    Anims.SetBool("MoveR", false);
                    Anims.SetBool("MoveL", false);
                    Velocidad = 0;
                    Anims.SetBool("AttackR", true);
                    PlayerMoving = false;
                }

                if (Input.GetKeyUp(KeyCode.RightArrow) && PuntaR)
                {
                    Velocidad = 0.5f;
                    Anims.SetBool("AttackR", false);
                    PuntaR = false;
                    if (Special) DispararEspecialR(Vector2.right * 1000f);
                    else DispararR(Vector2.right * 1000f);
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Anims.SetBool("MoveR", false);
                    Anims.SetBool("MoveL", false);
                    Velocidad = 0;
                    Anims.SetBool("AttackL", true);
                    PlayerMoving = false;
                }

                if (Input.GetKeyUp(KeyCode.LeftArrow) && PuntaL)
                {
                    Velocidad = 0.5f;
                    Anims.SetBool("AttackL", false);
                    PuntaL = false;
                    if (Special) DispararEspecialL(Vector2.left * 1000f);
                    else DispararL(Vector2.left * 1000f);
                }
            }
            // Lógica de la salud
            if (Salud == 1)
            {
                Alerta.enabled = true;
                Alerta.text = "¡Cuidado! no dejes que te hagan daño.";
                GetComponent<SpriteRenderer>().color = Color.red;
            }
            else
            {
                Alerta.enabled = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }
            if (Salud < 1)
            {
                Anims.SetBool("AttackR", false);
                Anims.SetBool("AttackL", false);
                Anims.SetBool("MoveR", false);
                Anims.SetBool("MoveL", false);
                Anims.SetBool("Death", true);
                Alive = false;
            }
            if (Salud < 1)
            {
                Salud = 0;
            }
            if (Salud > 2)
            {
                Salud = 3;
            }
            if (Ammun < 0) Ammun = 0;
            if (Ammun == 0) Special = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Enemy") || (collision.tag == "PantherEnemy") || (collision.tag == "Slime"))
        {
            Debug.Log("Sufres daño");
            Salud--;
            int Vida = Salud;
            hud.PerderVida(Vida);
        }
    }

    public void Curar()
    {
        Salud++;
        int Vida = Salud;
        Alerta.enabled = true;
        Alerta.text = "Bien hecho, has recuperado vida";
        if (Time.time - 3f >= Controller.intervalo)
        {
            Alerta.enabled = false;
        }
        hud.GanarVida(Vida);
    }
    void Impulso()
    {
        float Moving = Input.GetAxis("Horizontal");
        if (Moving > 0f)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(5f, transform.position.y, transform.position.z);
        }
        if (Moving < 0f)
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector3(-5f, transform.position.y, transform.position.z);
        }
        // Actualizar el tiempo del último impulso
        UltimoImpulso = Time.time;
    }
    void DispararR(Vector2 fuerza)
    {

        // Lógica de instanciar y disparar el prefab de la flecha
        GameObject Disparo = Instantiate(Flecha, new Vector3(gameObject.transform.position.x + 0.75f, gameObject.transform.position.y + 0.7f, 0), Quaternion.identity);
        Disparo.transform.parent = gameObject.transform;
        Disparo.GetComponent<Rigidbody2D>().AddForce(new Vector2(fuerza.x, 10f));

        // Actualizar el tiempo del último disparo
        UltimoDisparo = Time.time;
    }
    void DispararEspecialR(Vector2 fuerza)
    {

        // Lógica de instanciar y disparar el prefab de la flecha
        GameObject Disparo = Instantiate(FlechaEspecial, new Vector3(gameObject.transform.position.x + 0.75f, gameObject.transform.position.y + 0.7f, 0), Quaternion.identity);
        Disparo.transform.parent = gameObject.transform;
        Disparo.GetComponent<Rigidbody2D>().AddForce(new Vector2(fuerza.x, 10f));
        Special = false;
        Ammun--;
        int Flecha = Ammun;
        hud.GastarFlecha(Flecha);
        // Actualizar el tiempo del último disparo
        UltimoDisparo = Time.time;
    }
    void DispararL(Vector2 fuerza)
    {
        // Lógica de instanciar y disparar el prefab de la flecha
        GameObject Disparo = Instantiate(Flecha, new Vector3(gameObject.transform.position.x - 0.75f, gameObject.transform.position.y + 0.7f, 0), Quaternion.identity);
        Disparo.transform.parent = gameObject.transform;
        Disparo.GetComponent<Rigidbody2D>().AddForce(new Vector2(fuerza.x, 10f));
        Disparo.GetComponent<SpriteRenderer>().flipX = true;

        // Actualizar el tiempo del último disparo
        UltimoDisparo = Time.time;
    }
    void DispararEspecialL(Vector2 fuerza)
    {
        // Lógica de instanciar y disparar el prefab de la flecha
        GameObject Disparo = Instantiate(FlechaEspecial, new Vector3(gameObject.transform.position.x - 0.75f, gameObject.transform.position.y + 0.7f, 0), Quaternion.identity);
        Disparo.transform.parent = gameObject.transform;
        Disparo.GetComponent<Rigidbody2D>().AddForce(new Vector2(fuerza.x, 10f));
        Disparo.GetComponent<SpriteRenderer>().flipX = true;
        Special = false;
        Ammun--;
        int Flecha = Ammun;
        hud.GastarFlecha(Flecha);
        // Actualizar el tiempo del último disparo
        UltimoDisparo = Time.time;
    }
}
