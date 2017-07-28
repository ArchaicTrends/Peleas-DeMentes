using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarraVida : MonoBehaviour {

    private float LongitudBarra;
    private float ValorPunto;
    private int orientacion;
    private float MaximaVida;
    private float VidaAnterior;
    public Animaciones animacion1;
    public Animaciones animacion2;

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
            if (remoto)
                animacion2.HerirCerebro();
            else
                animacion1.HerirCerebro();

            if(firebase.DatosPersonaje.Vida > firebase.DatosPersonajeRemoto.Vida)
            {
                animacion1.ActivarAnimacionPerder(true);
                animacion2.ActivarAnimacionPerder(false);
            }
            else if(firebase.DatosPersonaje.Vida < firebase.DatosPersonajeRemoto.Vida)
            {
                animacion1.ActivarAnimacionPerder(false);
                animacion2.ActivarAnimacionPerder(true);
            }
            else
            {
                animacion1.ActivarAnimacionPerder(false);
                animacion2.ActivarAnimacionPerder(false);
            }

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
