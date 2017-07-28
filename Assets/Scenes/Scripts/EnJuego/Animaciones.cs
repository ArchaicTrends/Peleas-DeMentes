using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour {

    public GameObject LaptopGeneral;
    public Animator animacionCerebelo;
    public Animator animacionNeurona;

    public Animator animacionCerebro;
    private bool animacionCerebroActivada;
    private float tiempo;
	// Use this for initialization
	void Start () {
        animacionCerebroActivada = false;
    }

    void Update()
    {
        if (animacionCerebroActivada && (Time.time - tiempo) >= 1)
        {
            animacionCerebro.SetBool("EsHerido", false);
            animacionCerebroActivada = false;
        }
    }

    public void HerirCerebro()
    {
        animacionCerebro.SetBool("EsHerido", true);
        animacionCerebroActivada = true;
        tiempo = Time.time;
    }
	
    // personaje principal
    public void AnimacionLaptopDeshabilitar()
    {
        LaptopGeneral.SetActive(false);
    }

    public int ActivarAnimacionPerder(bool tipo)
    {
        int res = Random.Range(1, 4);
        animacionNeurona.SetInteger("Opcion", res);
        animacionCerebelo.SetBool("Victoria", !tipo);
        GetComponent<Animator>().SetBool("Victoria", !tipo);
        GetComponent<Animator>().SetInteger("OpcionAleatoria", res);
        return res;
    }

    public void AnimacionLaptopHabilitar()
    {
        LaptopGeneral.SetActive(true);
        Debug.Log("hmmm");
    }

    public void ComenzarAnimacion()
    {
        GetComponent<Animator>().SetBool("Comenzar", true);
    }
    // Fin personaje principal
}
