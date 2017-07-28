using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJuego : MonoBehaviour {

    public Animaciones AnimacionCerebro1;
    public Animaciones AnimacionCerebro2;
    public GameObject TextoCountdown;
    public GameObject BotonComenzar;
    public static bool comenzoJuego = false;
    private float tiempo;
    private int valorTextoCountdown;
    private int botonSeleccionado;
    public Text textoEjercicios;
    public GameObject[] botones;
    private TipoAtributo tipoSeleccionado;

    public GameObject ventanaEjercicio;

    private ConexionFirebase firebase;
    GeneradorEjercicios gen;

	// Use this for initialization
	void Start () {
        firebase = GameObject.FindObjectOfType<ConexionFirebase>();
        gen = new GeneradorEjercicios();
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

    private void PasarSiguienteEjercicio(TipoAtributo atributo)
    {
        tipoSeleccionado = atributo;
        ventanaEjercicio.transform.position = new Vector3()
        {
            x = ventanaEjercicio.transform.position.x,
            y = 0,
            z = ventanaEjercicio.transform.position.z
        };
        string ejercicio = gen.SiguienteEjercicio;
        textoEjercicios.text = ejercicio;
        botonSeleccionado = Random.Range(0, botones.Length);
        int sol = int.Parse(gen.SiguienteSolucion);
        int inv = 1;
        int mul = 1;
        for (int i = 0; i < botones.Length; i++)
        {
            if (i != botonSeleccionado)
            {
                botones[i].GetComponentInChildren<Text>().text = (sol + mul * inv).ToString();
                inv *= -1;
                mul += 3;
            }
            else
                botones[i].GetComponentInChildren<Text>().text = sol.ToString();
        }
    }

    public void BotonMejorarAtaque_Click()
    {
        if (ventanaEjercicio.transform.position.y != 0)
        {
            PasarSiguienteEjercicio(TipoAtributo.ATAQUE);
        }
        else
        {
            ventanaEjercicio.transform.position = new Vector3()
            {
                x = ventanaEjercicio.transform.position.x,
                y = 298,
                z = ventanaEjercicio.transform.position.z
            };
        }
    }

    public void BotonMejorarDefensa_Click()
    {
        if (ventanaEjercicio.transform.position.y != 0)
        {
            PasarSiguienteEjercicio(TipoAtributo.DEFENSA);
        }
        else
        {
            ventanaEjercicio.transform.position = new Vector3()
            {
                x = ventanaEjercicio.transform.position.x,
                y = 298,
                z = ventanaEjercicio.transform.position.z
            };
        }
    }

    public void SeleccionarRespuesta(int i)
    {
        if(i == botonSeleccionado)
        {
            float valor = 0;
            switch (tipoSeleccionado)
            {
                case TipoAtributo.ATAQUE:
                    valor = firebase.DatosPersonaje.Ataque;
                    break;
                case TipoAtributo.AGILIDAD:
                    valor = firebase.DatosPersonaje.Agilidad;
                    break;
                case TipoAtributo.ATAQUE_CRITICO:
                    valor = firebase.DatosPersonaje.AtaqueCritico;
                    break;
                case TipoAtributo.DEFENSA:
                    valor = firebase.DatosPersonaje.Defensa;
                    break;
                default:
                    valor = -1;
                    break;
            }
            if(valor != -1)
                firebase.CambiarValor(tipoSeleccionado, valor + 1);
        }
        PasarSiguienteEjercicio(tipoSeleccionado);
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
