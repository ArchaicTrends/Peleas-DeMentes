using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJuego : MonoBehaviour {

    public Animaciones AnimacionCerebro1;
    public Animaciones AnimacionCerebro2;
    public GameObject TextoCountdown;
    public GameObject BotonComenzar;
    private bool comenzoJuego;
    private float tiempo;
    private int valorTextoCountdown;

	// Use this for initialization
	void Start () {
        comenzoJuego = false;
        valorTextoCountdown = 3;
        TextoCountdown.GetComponent<TextMesh>().text = valorTextoCountdown.ToString();
	}
	
	// Update is called once per frame
	void Update () {
        if (comenzoJuego)
        {
            if (valorTextoCountdown > 1 && (Time.time - tiempo) > 2)
            {
                tiempo = Time.time;
                valorTextoCountdown--;
                TextoCountdown.GetComponent<TextMesh>().text = valorTextoCountdown.ToString();
            }
            else if (valorTextoCountdown <= 1 && (Time.time - tiempo) > 2)
                TextoCountdown.SetActive(false);
        }
	}

    public void BotonComenzar_Click()
    {
        BotonComenzar.SetActive(false);
        TextoCountdown.SetActive(true);
        AnimacionCerebro1.ComenzarAnimacion();
        AnimacionCerebro2.ComenzarAnimacion();
        tiempo = Time.time;
        comenzoJuego = true;
    }
}
