using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConexionFirebase : MonoBehaviour {

    //Referencia Base de datos
    private DatabaseReference referencia;

    // Use this for initialization
    void Awake()
    {
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://peleas-dementes.firebaseio.com/");
        referencia = FirebaseDatabase.DefaultInstance.RootReference;
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
