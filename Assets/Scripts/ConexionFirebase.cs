using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System;

public class ConexionFirebase : MonoBehaviour {

    //Referencia Base de datos
    private DatabaseReference referenciaRemoto;

    public string NombreJuego = "DEMENTES";
    public string IdServidor = "TEST";

    public DatabaseReference ReferenciaVida;
    public DatabaseReference ReferenciaAtaque;
    public DatabaseReference ReferenciaDefensa;
    public DatabaseReference ReferenciaAgilidad;
    public DatabaseReference ReferenciaAtaqueCritico;
    public DatosPersonaje DatosPersonaje;


    public DatabaseReference ReferenciaVidaRemota;
    public DatabaseReference ReferenciaAtaqueRemoto;
    public DatabaseReference ReferenciaDefensaRemota;
    public DatabaseReference ReferenciaAgilidadRemota;
    public DatabaseReference ReferenciaAtaqueCriticoRemoto;


    public DatosPersonaje DatosPersonajeRemoto;

    private Task<DataSnapshot> tareaRemoto;
    private bool personajeRemotoConectado;
    private Task[] tareasInicializacionPersonaje;
    private bool personajeInicializado;

    private string IdPersonajeRemoto;
    private Task primeraTarea;
    private bool primeraTareaFinalizada;

    // Use this for initialization
    void Awake()
    {
        primeraTareaFinalizada = false;
        personajeInicializado = false;
        tareasInicializacionPersonaje = new Task[5];

        DatosPersonaje = new DatosPersonaje()
        {
            Vida = 100.0f,
            Agilidad = 0.0f,
            Ataque = 1.0f,
            AtaqueCritico = 0.0f,
            Defensa = 0.0f
        };
        DatosPersonajeRemoto = new DatosPersonaje()
        {
            Vida = 100.0f,
            Agilidad = 0.0f,
            Ataque = 1.0f,
            AtaqueCritico = 0.0f,
            Defensa = 0.0f
        };

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://peleas-dementes.firebaseio.com/");
        ReferenciaVida = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + SystemInfo.deviceUniqueIdentifier + "/"+TipoAtributo.VIDA.ToString());
        ReferenciaAtaque = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + SystemInfo.deviceUniqueIdentifier + "/"+TipoAtributo.ATAQUE.ToString());
        ReferenciaDefensa = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + SystemInfo.deviceUniqueIdentifier + "/"+TipoAtributo.DEFENSA.ToString());
        ReferenciaAgilidad = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + SystemInfo.deviceUniqueIdentifier + "/"+TipoAtributo.AGILIDAD.ToString());
        ReferenciaAtaqueCritico = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + SystemInfo.deviceUniqueIdentifier + "/"+TipoAtributo.ATAQUE_CRITICO.ToString());

        referenciaRemoto = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor);

        primeraTarea = referenciaRemoto.RemoveValueAsync();
    }

    private void EventoCambioAtributoRemoto(object sender, ValueChangedEventArgs e)
    {
        try
        {
            float valor = float.Parse(e.Snapshot.Value.ToString());
            TipoAtributo tipo = (TipoAtributo)Enum.Parse(typeof(TipoAtributo), e.Snapshot.Key);
            switch (tipo)
            {
                case TipoAtributo.VIDA:
                    DatosPersonajeRemoto.Vida = valor;
                    break;
                case TipoAtributo.AGILIDAD:
                    DatosPersonajeRemoto.Agilidad = valor;
                    break;
                case TipoAtributo.ATAQUE:
                    DatosPersonajeRemoto.Ataque = valor;
                    break;
                case TipoAtributo.DEFENSA:
                    DatosPersonajeRemoto.Defensa = valor;
                    break;
                case TipoAtributo.ATAQUE_CRITICO:
                    DatosPersonajeRemoto.AtaqueCritico = valor;
                    break;
            }
        }
        catch (Exception)
        {

        }
    }

    private void EventoCambioAtributo(object sender, ValueChangedEventArgs e)
    {
        try {
            float valor = float.Parse(e.Snapshot.Value.ToString());
            TipoAtributo tipo = (TipoAtributo)Enum.Parse(typeof(TipoAtributo), e.Snapshot.Key);
            switch (tipo)
            {
                case TipoAtributo.VIDA:
                    DatosPersonaje.Vida = valor;
                    break;
                case TipoAtributo.AGILIDAD:
                    DatosPersonaje.Agilidad = valor;
                    break;
                case TipoAtributo.ATAQUE:
                    DatosPersonaje.Ataque = valor;
                    break;
                case TipoAtributo.DEFENSA:
                    DatosPersonaje.Defensa = valor;
                    break;
                case TipoAtributo.ATAQUE_CRITICO:
                    DatosPersonaje.AtaqueCritico = valor;
                    break;
            }
        }
        catch (Exception)
        {

        }
    }

    void Start() {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (!primeraTareaFinalizada && primeraTarea != null && primeraTarea.IsCompleted)
        {
            primeraTareaFinalizada = true;
            tareaRemoto = referenciaRemoto.GetValueAsync();

            tareasInicializacionPersonaje[0] = ReferenciaVida.SetValueAsync(DatosPersonaje.Vida);
            tareasInicializacionPersonaje[1] = ReferenciaAtaque.SetValueAsync(DatosPersonaje.Ataque);
            tareasInicializacionPersonaje[2] = ReferenciaDefensa.SetValueAsync(DatosPersonaje.Defensa);
            tareasInicializacionPersonaje[3] = ReferenciaAgilidad.SetValueAsync(DatosPersonaje.Agilidad);
            tareasInicializacionPersonaje[4] = ReferenciaAtaqueCritico.SetValueAsync(DatosPersonaje.AtaqueCritico);

            Debug.Log("Primera tarea finalizada");
        }
        else if (primeraTareaFinalizada)
        {
            if (tareaRemoto != null && tareaRemoto.IsCompleted && !personajeRemotoConectado)
            {
                try
                {
                    Dictionary<string, object> personajeRemoto = (Dictionary<string, object>)tareaRemoto.Result.Value;
                    IdPersonajeRemoto = personajeRemoto.Where(val => !val.Key.Equals(SystemInfo.deviceUniqueIdentifier)).First().Key;
                    personajeRemotoConectado = true;

                    ReferenciaVidaRemota = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + IdPersonajeRemoto + "/" + TipoAtributo.VIDA.ToString());
                    ReferenciaAtaqueRemoto = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + IdPersonajeRemoto + "/" + TipoAtributo.ATAQUE.ToString());
                    ReferenciaDefensaRemota = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + IdPersonajeRemoto + "/" + TipoAtributo.DEFENSA.ToString());
                    ReferenciaAgilidadRemota = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + IdPersonajeRemoto + "/" + TipoAtributo.AGILIDAD.ToString());
                    ReferenciaAtaqueCriticoRemoto = FirebaseDatabase.DefaultInstance.GetReference("/" + NombreJuego + "/" + IdServidor + "/" + IdPersonajeRemoto + "/" + TipoAtributo.ATAQUE_CRITICO.ToString());

                    ReferenciaVidaRemota.ValueChanged += EventoCambioAtributoRemoto;
                    ReferenciaAtaqueRemoto.ValueChanged += EventoCambioAtributoRemoto;
                    ReferenciaAgilidadRemota.ValueChanged += EventoCambioAtributoRemoto;
                    ReferenciaDefensaRemota.ValueChanged += EventoCambioAtributoRemoto;
                    ReferenciaAtaqueCriticoRemoto.ValueChanged += EventoCambioAtributoRemoto;
                }
                catch (Exception)
                {
                    //Debug.Log("Personaje Remoto No Conectado");
                    tareaRemoto = referenciaRemoto.GetValueAsync();
                }
            }

            if (!personajeInicializado)
            {
                personajeInicializado = true;
                foreach (Task tarea in tareasInicializacionPersonaje)
                {
                    if (tarea == null || !tarea.IsCompleted)
                    {
                        personajeInicializado = false;
                        break;
                    }
                }
                if (personajeInicializado)
                {
                    ReferenciaVida.ValueChanged += EventoCambioAtributo;
                    ReferenciaAtaque.ValueChanged += EventoCambioAtributo;
                    ReferenciaAgilidad.ValueChanged += EventoCambioAtributo;
                    ReferenciaDefensa.ValueChanged += EventoCambioAtributo;
                    ReferenciaAtaqueCritico.ValueChanged += EventoCambioAtributo;
                }
            }
        }
	}

    public Task CambiarValorRemoto(TipoAtributo tipo, float valor)
    {
        switch (tipo)
        {
            case TipoAtributo.VIDA:
                DatosPersonajeRemoto.Vida = valor;
                break;
            case TipoAtributo.AGILIDAD:
                DatosPersonajeRemoto.Agilidad = valor;
                break;
            case TipoAtributo.ATAQUE:
                DatosPersonajeRemoto.Ataque = valor;
                break;
            case TipoAtributo.DEFENSA:
                DatosPersonajeRemoto.Defensa = valor;
                break;
            case TipoAtributo.ATAQUE_CRITICO:
                DatosPersonajeRemoto.AtaqueCritico = valor;
                break;
        }
        return referenciaRemoto.Child(IdPersonajeRemoto + "/" + tipo).SetValueAsync(valor);
    }

    public Task CambiarValor(TipoAtributo tipo, float valor)
    {
        switch (tipo)
        {
            case TipoAtributo.VIDA:
                DatosPersonaje.Vida = valor;
                break;
            case TipoAtributo.AGILIDAD:
                DatosPersonaje.Agilidad = valor;
                break;
            case TipoAtributo.ATAQUE:
                DatosPersonaje.Ataque = valor;
                break;
            case TipoAtributo.DEFENSA:
                DatosPersonaje.Defensa = valor;
                break;
            case TipoAtributo.ATAQUE_CRITICO:
                DatosPersonaje.AtaqueCritico = valor;
                break;
        }
        return referenciaRemoto.Child(SystemInfo.deviceUniqueIdentifier + "/" + tipo).SetValueAsync(valor);
    }
}

public class DatosPersonaje
{
    public float Ataque;
    public float Defensa;
    public float Agilidad;
    public float AtaqueCritico;
    public float Vida;
}

public enum TipoAtributo
{
    VIDA,
    AGILIDAD,
    ATAQUE,
    DEFENSA,
    ATAQUE_CRITICO
}