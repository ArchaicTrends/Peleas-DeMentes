using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorEjercicios {

    private int[] numeros;

    public GeneradorEjercicios(int minDig, int maxDig)
    {
        int numMin = (int)Mathf.Pow(10, minDig - 1);
        int numMax = (int)Mathf.Pow(10, maxDig) - 1;
        numeros = new int[numMax - numMin];
        for(int i = 0; i < numeros.Length; i++)
            numeros[i] = numMin + i;
        DesordenarNumeros();
    }

    private void DesordenarNumeros()
    {
        for(int i = 0; i < numeros.Length; i++)
        {
            int selIndex = Random.Range(i, numeros.Length);
            int sel = numeros[selIndex];
            int aux = numeros[i];
            numeros[i] = sel;
            numeros[selIndex] = aux;
        }
    }

    public Ejercicio NuevoEjercicio()
    {
        return null;
    }
}

public class Ejercicio
{
    public string Texto;
    public float Solucion;
}
