using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class HuntController : MonoBehaviour
{
    public float Timer = 0; // Reloj interno
    public float intervalo; // Intervalo en segundos
    public float cooldown; // Enfriamiento entre intervalos

    public TextMeshProUGUI[] Text;
    public TextMeshProUGUI Inicio;
    public TextMeshProUGUI Pause;
    public TextMeshProUGUI Reloj;
    public GameObject MenuInGame;
    public GameObject Albin;
    public AlbinControl Health;
    public GameObject AlbinSlime;
    public AlbinSlimesControl ASHealth;
    public GameObject Suelo;
    public int Cazados;
    public GameObject Slime;
    public GameObject BigSlime;
    public GameObject Panther;
    public Animator SlimeAnim;
    public Animator BigSlimeAnim;
    public Animator PantherAnim;
    public bool Menu;
    public Button Player;
    public Button Player2;
    public Button Restart;
    public GameObject[] Cartel;
    private bool StartGame = false;

    // Start is called before the first frame update
    void Start()
    {
        // Comienza el programa con el menú de inicio
        Albin.SetActive(false);
        AlbinSlime.SetActive(false);
        Menu = true;
        Time.timeScale = 0;
        MenuInGame.SetActive(true);
        Inicio.text = "Wild hunter";
        Pause.enabled = false;
        Text[0].text = "Slimes cazados: ";
        Text[1].text = "Objetivo: aún por encontrar";
        StartGame = true;
        Cazados = 0;
        Restart.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        float Tiempo = (Timer += Time.deltaTime);
        Text[0].text = "Slimes cazados: " + Cazados;
        string minutes = ((int)Tiempo / 60).ToString();
        string seconds = (Tiempo % 60).ToString("00");
        Reloj.text = minutes + ":" + seconds;
        // Si está activo coordina el controller
        if (!Albin.activeSelf && !AlbinSlime.activeSelf)
        {
            StartGame = false;
            Menu = true;
        }
        if (Albin.activeSelf || AlbinSlime.activeSelf)
        {
            StartGame = true;
        }
        if (StartGame)
        {
            int Horda = Random.Range(1, 5);
            int GenerarEnemigo = Random.Range(0, 51);
            if (GenerarEnemigo < 50 && GenerarEnemigo > 20)
            {
                // Si ha pasado suficiente tiempo desde la última generación de enemigos
                if (Time.time - cooldown > intervalo)
                {
                    Debug.Log("Aparece(n) " + Horda);
                    for (int i = 0; i < Horda; i++)
                    {
                        GetSlime();
                    }
                    // Actualiza el tiempo de la última generación de enemigos
                    cooldown = Time.time;
                }
            }
            if (Cazados > 50) intervalo = 10;
            // Probabilidad de que un slime grande aparezca al superar los 50 cazados
            if (GenerarEnemigo <= 20 && Cazados > 50)
            {
                // Si ha pasado suficiente tiempo desde la última generación de enemigos
                if (Time.time - cooldown > intervalo)
                {
                    Debug.Log("Gordo(s)");
                    for (int i = 0; i < Horda-1; i++)
                    {
                        GetBigSlime();
                    }
                    // Actualiza el tiempo de la última generación de enemigos
                    cooldown = Time.time;
                }
            }
            // Probabilidad de que el objetivo especial se genere
            if (GenerarEnemigo == 50 && Cazados > 50)
            {
                // Si ha pasado suficiente tiempo desde la última generación de enemigos
                if (Time.time - cooldown > intervalo)
                {
                    Text[1].text = "Objetivo: ¡Ya ha aparecido!";
                    Debug.Log("Aparece el enemigo especial");
                    GetPanther();
                    // Actualiza el tiempo de la última generación de enemigos
                    cooldown = Time.time;
                }
            }
            if (Cazados > 50) Suelo.GetComponent<SpriteRenderer>().color = Color.yellow;
            // Cada 25 slimes cazados
            if (Cazados % 25 == 0 && Cazados != 0)
            {
                Health.Curar();
            }
            // Toggle para activar/desactivar el menú
            if (Input.GetKeyDown(KeyCode.Escape) && !Menu)
            {
                Menu = true;
                Time.timeScale = 0;
                Inicio.enabled = false;
                Pause.enabled = true;
                Player.enabled = false;
                Player2.enabled = false;
                Pause.text = "Pausa";
                MenuInGame.SetActive(true);
                Restart.interactable = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && Menu != false)
            {
                Menu = false;
                Time.timeScale = 1;
                MenuInGame.SetActive(false);
            }
        }
    }
    // Cierra el programa
    public void Salir() {
        Debug.Log("Salir");
        Application.Quit();
    }
    // Reinicia la escena
    public void Reinicio()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene(0);
    }

    public void GetSlime()
    {
        // Genera a los slimes en una posición aleatoria dentro de un rango
        bool Derecha = Random.value < 0.5f;
        float posicion = Random.Range(10f, 13f);
        if (Derecha)
        {
            GameObject SlimeEnemy = Instantiate(Slime, new Vector3(posicion, gameObject.transform.position.y, 0), Quaternion.identity);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else
        {
            GameObject SlimeEnemy = Instantiate(Slime, new Vector3(-posicion, gameObject.transform.position.y, 0), Quaternion.identity);
        }
    }
    // Genera un slime grande en una posición aleatoria dentro de un rango
    public void GetBigSlime()
    {
        // Genera a los slimes en una posición aleatoria dentro de un rango
        bool Derecha = Random.value < 0.5f;
        float posicion = Random.Range(10f, 13f);
        if (Derecha)
        {
            GameObject BigEnemy = Instantiate(BigSlime, new Vector3(posicion, gameObject.transform.position.y, 0), Quaternion.identity);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else
        {
            GameObject BigEnemy = Instantiate(BigSlime, new Vector3(-posicion, gameObject.transform.position.y, 0), Quaternion.identity);
        }
    }
    // Genera al objetivo especial en una posición aleatoria dentro de un rango
    public void GetPanther()
    {
        bool Derecha = Random.value < 0.5f;
        float posicion = Random.Range(10f, 13f);
        if (Derecha)
        {
            GameObject PantherEnemy = Instantiate(Panther, new Vector3(posicion, gameObject.transform.position.y, 0), Quaternion.identity);
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
        }
        else
        {
            GameObject PantherEnemy = Instantiate(Panther, new Vector3(-posicion, gameObject.transform.position.y, 0), Quaternion.identity);
        }
        
    }
    // Toggle para activar/desactivar a Albin
    public void GetAlbin()
    {
        AlbinSlime.SetActive(false);
        Albin.SetActive(true);
        Albin.transform.position = new Vector3(0, 0, 0);
    }
    // Toggle para activar/desactivar a Albin (con Limo)
    public void GetAlbinSlime()
    {
        Albin.SetActive(false);
        AlbinSlime.SetActive(true);
        AlbinSlime.transform.position = new Vector3(0, 0, 0);
    }

    public void Cazar()
    {
        Cazados++;
    }
    // Finaliza la partida y apaga el coordinador del controller
    public void Final()
    {
        StartGame = false;
        Menu = true;
        Time.timeScale = 0;
        Inicio.enabled = false;
        Pause.enabled = true;
        MenuInGame.SetActive(true);
        Cartel[0].SetActive(false);
        Cartel[1].SetActive(false);
        Player.interactable = false;
        Player2.interactable = false;
        Restart.interactable = true;
    }
}