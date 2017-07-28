using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generador : MonoBehaviour {

    public Text TextoEjercicio;
    private GeneradorEjercicios generador;
    private int index = 0;
	// Use this for initialization
	void Start () {
        generador = new GeneradorEjercicios();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ActualizarEjercicio()
    {
        TextoEjercicio.text = "EJERCICIO " + (++index) + "\n" + generador.SiguienteEjercicio;
    }
}
