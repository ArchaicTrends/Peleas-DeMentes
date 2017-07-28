using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour {

    private float tiempo;
    private bool comenzarControl;
    private int segundosPorAtaque = 4;
    private ConexionFirebase firebase;

    public Transform origenDisparo;

    public Transform destinoBase;
    public Transform destinoEscudo;

    public Transform destinoPropioEscudo;

    public GameObject proyectil;

	// Use this for initialization
	void Start () {
        comenzarControl = false;
        firebase = FindObjectOfType<ConexionFirebase>();
	}
	
	// Update is called once per frame
	void Update () {
        if (UIJuego.comenzoJuego && !comenzarControl)
        {
            tiempo = Time.time;
            comenzarControl = true;
        }
        else if (comenzarControl)
        {
            if(Time.time - tiempo >= segundosPorAtaque - (0.03f * firebase.DatosPersonaje.Agilidad))
            {
                //firebase.CambiarValor(TipoAtributo.VIDA, vidaOponente);
                float probabilidadReflejo = 0.2f * firebase.DatosPersonajeRemoto.Defensa;
                int rand = Random.Range(1, 101);
                if(probabilidadReflejo >= rand)
                {
                    //REFLEJO
                }
                else
                {
                    //DEFENSA
                    float def = firebase.DatosPersonaje.Ataque * (0.005f * firebase.DatosPersonajeRemoto.Defensa);
                    float vidaOponente = firebase.DatosPersonajeRemoto.Vida - (firebase.DatosPersonaje.Ataque - def);
                }

                tiempo = Time.time;
            }
        }
	}

    private void DispararConReflejo()
    {
        var instancia = Instantiate(proyectil, origenDisparo.position, Quaternion.identity);
        instancia.GetComponent<Proyectil>().Destino = destinoEscudo.position;
        instancia.GetComponent<Proyectil>().Destino2 = destinoPropioEscudo.position;
        instancia.GetComponent<Proyectil>().IniciarMovimiento = true;
        instancia.GetComponent<Proyectil>().reflejar = true;
    }

    private void DispararBase()
    {
        var instancia = Instantiate(proyectil, origenDisparo.position, Quaternion.identity);
        instancia.GetComponent<Proyectil>().Destino = destinoBase.position;
        instancia.GetComponent<Proyectil>().IniciarMovimiento = true;
        instancia.GetComponent<Proyectil>().reflejar = false;
    }
}
