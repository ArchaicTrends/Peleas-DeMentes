using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animaciones : MonoBehaviour {

    public GameObject LaptopGeneral;

	// Use this for initialization
	void Start () {
        
    }
	
    // personaje principal
    public void AnimacionLaptopDeshabilitar()
    {
        LaptopGeneral.SetActive(false);
    }

    public void AnimacionLaptopHabilitar()
    {
        LaptopGeneral.SetActive(true);
        Debug.Log("hmmm");
    }

    public void ComenzarAnimacion()
    {
        GetComponent<Animator>().SetBool("Comenzar", true);
    }
    // Fin personaje principal
}
