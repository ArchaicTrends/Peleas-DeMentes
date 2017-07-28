using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Personaje : MonoBehaviour {

    private float tiempo;
    private bool comenzarControl;
    private int segundosPorAtaque = 4;
    private ConexionFirebase firebase;

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
                float vidaOponente = firebase.DatosPersonajeRemoto.Vida - firebase.DatosPersonaje.Ataque;
                firebase.CambiarValor(TipoAtributo.VIDA, vidaOponente);
                tiempo = Time.time;
            }
        }
	}
}
