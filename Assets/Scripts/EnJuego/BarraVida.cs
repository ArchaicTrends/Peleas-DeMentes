using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraVida : MonoBehaviour {

    private float LongitudBarra;
    private float ValorPunto;
    private int orientacion;
    private float MaximaVida;
    private float VidaAnterior;

    public bool remoto;
    private ConexionFirebase firebase;

	// Use this for initialization
	void Start () {
        firebase = GameObject.FindObjectOfType<ConexionFirebase>();
        LongitudBarra = transform.localScale.x;
        VidaAnterior = MaximaVida = 400;
        ValorPunto = LongitudBarra / 400;
        orientacion = transform.position.x < 0 ? 1 : -1;
	}

    void Update()
    {
        if(VidaAnterior != (remoto ? firebase.DatosPersonajeRemoto.Vida : firebase.DatosPersonaje.Vida))
        {
            float dif = VidaAnterior - (remoto ? firebase.DatosPersonajeRemoto.Vida : firebase.DatosPersonaje.Vida);
            Debug.Log(dif);
            VidaAnterior = remoto ? firebase.DatosPersonajeRemoto.Vida : firebase.DatosPersonaje.Vida;
            transform.localScale = new Vector3()
            {
                x = transform.localScale.x - ValorPunto * dif,
                y = transform.localScale.y,
                z = transform.localScale.z
            };
            transform.position = new Vector3()
            {
                x = transform.position.x - ((ValorPunto / 2.0f) * orientacion) * dif,
                y = transform.position.y,
                z = transform.position.z
            };
        }
    }
}
