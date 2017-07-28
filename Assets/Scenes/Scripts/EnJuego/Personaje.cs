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
    private Vector3 campoDeFuerzaReferencia = new Vector3()
    {
        x = -7.01f,
        y = -3.03f,
        z = -5.859772f
    };
    public GameObject campoDeFuerza;

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
            if(Time.time - tiempo >= segundosPorAtaque - (0.035f * firebase.DatosPersonaje.Agilidad))
            {
                //firebase.CambiarValor(TipoAtributo.VIDA, vidaOponente);
                float probabilidadReflejo = 0.4f * firebase.DatosPersonajeRemoto.Defensa;
                int rand = Random.Range(1, 101);
                Debug.Log(probabilidadReflejo+" VS "+ rand);

                if(probabilidadReflejo >= rand)
                {
                    //REFLEJO
                    float def = firebase.DatosPersonaje.Ataque * (0.005f * firebase.DatosPersonaje.Defensa);
                    float vida = firebase.DatosPersonaje.Vida - (firebase.DatosPersonaje.Ataque - def);
                    firebase.CambiarValor(TipoAtributo.VIDA, vida);
                    DispararConReflejo();
                    var campo = Instantiate(campoDeFuerza, campoDeFuerzaReferencia, Quaternion.identity);
                    Vector3 posicionCampo = new Vector3()
                    {
                        x = campoDeFuerzaReferencia.x * (transform.position.x < 0 ? 1 : -1),
                        y = campoDeFuerzaReferencia.y,
                        z = campoDeFuerzaReferencia.z
                    };
                    Vector3 escala = new Vector3()
                    {
                        x = (transform.position.x < 0 ? -1 : 1),
                        y = 1,
                        z = 1
                    };
                    campo.transform.position = posicionCampo;
                    campo.transform.localScale = escala;
                    Destroy(campo, 1);
                }
                else
                {
                    //DEFENSA
                    float def = firebase.DatosPersonaje.Ataque * (0.005f * firebase.DatosPersonajeRemoto.Defensa);
                    float vidaOponente = firebase.DatosPersonajeRemoto.Vida - (firebase.DatosPersonaje.Ataque - def);
                    firebase.CambiarValorRemoto(TipoAtributo.VIDA, vidaOponente);
                    DispararBase();
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
        Color color = transform.position.x < 0 ? Color.black : new Color(0.91f, 0.08f, 0.08f);
        instancia.GetComponent<Proyectil>().color = color;
    }

    private void DispararBase()
    {
        var instancia = Instantiate(proyectil, origenDisparo.position, Quaternion.identity);
        instancia.GetComponent<Proyectil>().Destino = destinoBase.position;
        instancia.GetComponent<Proyectil>().IniciarMovimiento = true;
        instancia.GetComponent<Proyectil>().reflejar = false;
        Color color = transform.position.x < 0 ? Color.black : new Color(0.91f, 0.08f, 0.08f);
        instancia.GetComponent<Proyectil>().color = color;
    }
}
