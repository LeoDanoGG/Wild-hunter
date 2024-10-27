using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class HUD : MonoBehaviour
{
    public GameObject[] HeartContainer;
    public GameObject[] SpecialAmmun;
    public GameObject Albin;
    public GameObject AlbinSlime;
    public AlbinControl AlbinHealth;
    public AlbinSlimesControl AlbinSlimeHealth;

    private void Start()
    {
        AlbinHealth = GameObject.Find("Albin").GetComponent<AlbinControl>();
        AlbinSlimeHealth = GameObject.Find("AlbinSlime").GetComponent<AlbinSlimesControl>();
    }

    private void Update()
    {
        //Si Albin está activo modifica la barra de salud
        if (Albin.activeSelf)
        {
            HeartContainer[4].SetActive(false);
            HeartContainer[3].SetActive(false);
        }
        // Si Albin (con Limo) se activa modifica la barra de salud
        if (AlbinSlime.activeSelf)
        {
            HeartContainer[4].SetActive(true);
            HeartContainer[3].SetActive(true);
        }
    }
    // Si uno de los personajes pierde vida, modifica la  barra de salud
    public void PerderVida(int Vida)
    {
        HeartContainer[Vida].SetActive(false);
    }
    public void GanarVida(int Vida)
    {
        HeartContainer[Vida-1].SetActive(true);
    }
    public void PerderVidaSlime(int VidaS)
    {
        HeartContainer[VidaS].SetActive(false);
    }
    public void GanarVidaSlime(int VidaS)
    {
        HeartContainer[VidaS - 1].SetActive(true);
    }
    public void GastarFlecha(int Flecha)
    {
        SpecialAmmun[Flecha].SetActive(false);
    }
}
