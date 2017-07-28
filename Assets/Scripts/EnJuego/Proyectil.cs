using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proyectil : MonoBehaviour {

    public bool IniciarMovimiento = false;
    public bool reflejar = true;
    public Vector3 Destino;
    public Vector3 Destino2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (IniciarMovimiento && transform.position.x != Destino.x && transform.position.y != Destino.y)
        {
           transform.position = Vector3.MoveTowards(transform.position, Destino, 50 * Time.deltaTime);
        }
        else if (IniciarMovimiento)
        {
            if (reflejar)
            {
                transform.position = Vector3.MoveTowards(transform.position, Destino2, 50 * Time.deltaTime);
            }
            if(reflejar && transform.position.x == Destino2.x && transform.position.y == Destino2.y)
                Destroy(this.gameObject, 1);
        }
	}
}
