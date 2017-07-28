using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GeneradorEjercicios {

    public string SiguienteEjercicio
    {
        get
        {
            string prob = "...";
            if (index < ejercicios.Count)
            {
                prob = ejercicios[index];
                index++;
            }
            return prob;
        }
    }

    private List<string> ejercicios;
    private int index;
    private EjercicioPrimerGrupo EjerciciosFaciles;
    private EjercicioPrimerGrupo EjerciciosMedios;
    private EjercicioSegundoGrupo EjerciciosDificiles;

    public GeneradorEjercicios()
    {
        index = 0;
        ejercicios = new List<string>();
        EjerciciosFaciles = new EjercicioPrimerGrupo(1, 2, 1, 2, new Operador[] { Operador.Suma, Operador.Resta, Operador.Multiplicacion, Operador.Division });
        EjerciciosMedios = new EjercicioPrimerGrupo(1, 3, 2, 3, new Operador[] { Operador.Suma, Operador.Resta, Operador.Multiplicacion, Operador.Division });
        EjerciciosDificiles = new EjercicioSegundoGrupo();

        for (int i = 0; i < 40; i++)
            ejercicios.Add(EjerciciosFaciles.GenerarEjercicio());
        for (int i = 0; i < 40; i++)
            ejercicios.Add(EjerciciosMedios.GenerarEjercicio());
        for (int i = 0; i < 20; i++)
            ejercicios.Add(EjerciciosDificiles.GenerarEjercicio());
    }
}

public interface Ejercicio
{
    string GenerarEjercicio();
}

public class ListaNumericaAleatoria
{
	private int[] numeros;
	private int index;

	public ListaNumericaAleatoria(int minDig, int maxDig)
	{
		index = 0;
		int numMin = (int)Mathf.Pow(10, minDig - 1);
		int numMax = (int)Mathf.Pow(10, maxDig) - 1;
		numeros = new int[numMax - numMin];
		for(int i = 0; i < numeros.Length; i++)
			numeros[i] = numMin + i;
		DesordenarNumeros();
	}

    public ListaNumericaAleatoria(int maximo)
    {
        index = 0;
        numeros = new int[maximo];
        for (int i = 0; i < numeros.Length; i++)
            numeros[i] = i+2;
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

	public int SiguienteNumero
	{
		get
		{
			if (index >= numeros.Length) 
			{
				DesordenarNumeros ();
				index = 0;
			}
			return numeros [index++];
		}
	}
}

public class EjercicioPrimerGrupo : Ejercicio
{
	private List<ListaNumericaAleatoria> lista;
	private Operador[] operadores;
	private int minimoOperadores;
	private int maximoOperadores;

	public EjercicioPrimerGrupo(int minDig, int maxDig, int minOp, int maxOp, Operador[] operadores)
	{
		this.operadores = operadores;
        lista = new List<ListaNumericaAleatoria>();
		for (int i = 0; i <= maxOp; i++) 
		{
			lista.Add (new ListaNumericaAleatoria (minDig, maxDig));
		}
		minimoOperadores = minOp;
		maximoOperadores = maxOp;
	}

	public string GenerarEjercicio()
	{
		StringBuilder sb = new StringBuilder();
		int numOp = Random.Range (minimoOperadores, maximoOperadores + 1);
        int i;
        DesordenarOperadores();
        int numeroAnterior = -1;
        Operador? operadorAnterior = null;
        for (i = 0; i < numOp; i++)
		{
            if (operadorAnterior != null && (operadorAnterior.Value == Operador.Division || operadorAnterior.Value == Operador.Multiplicacion))
            {
                if (numeroAnterior > 1 && operadorAnterior.Value == Operador.Division)
                {
                    int sig = numeroAnterior - 1;
                    while (numeroAnterior % sig != 0)
                        sig--;
                    numeroAnterior = sig;
                }
                else if(operadorAnterior.Value == Operador.Multiplicacion)
                {
                    numeroAnterior = int.Parse(lista[i].SiguienteNumero.ToString()[0]+"");
                }
            }
            else
            {
                numeroAnterior = lista[i].SiguienteNumero;
            }
            operadorAnterior = operadores[i];
            sb.Append(numeroAnterior);
            sb.Append(OperadorToString(operadorAnterior.Value));
		}
        if (operadorAnterior != null && (operadorAnterior.Value == Operador.Division || operadorAnterior.Value == Operador.Multiplicacion))
        {
            if (numeroAnterior > 1 && operadorAnterior.Value == Operador.Division)
            {
                int sig = numeroAnterior - 1;
                while (numeroAnterior % sig != 0)
                    sig--;
                numeroAnterior = sig;
            }
            else if (operadorAnterior.Value == Operador.Multiplicacion && numeroAnterior.ToString().Length > 1)
            {
                numeroAnterior = int.Parse(lista[i].SiguienteNumero.ToString()[0] + "");
            }
            else if (operadorAnterior.Value == Operador.Multiplicacion)
                numeroAnterior = lista[i].SiguienteNumero;
        }
        else
        {
            numeroAnterior = lista[i].SiguienteNumero;
        }
        sb.Append(numeroAnterior);
        return sb.ToString();
	}

    private string OperadorToString(Operador op)
    {
        string st = "";
        switch (op)
        {
            case Operador.Division:
                st = "/";
                break;
            case Operador.Multiplicacion:
                st = "x";
                break;
            case Operador.Suma:
                st = "+";
                break;
            case Operador.Resta:
                st = "-";
                break;
        }
        return st;
    }

	private void DesordenarOperadores()
	{
		for(int i = 0; i < operadores.Length; i++)
		{
			int selIndex = Random.Range(i, operadores.Length);
			Operador sel = operadores[selIndex];
			Operador aux = operadores[i];
			operadores[i] = sel;
			operadores[selIndex] = aux;
		}
	}
}

public class EjercicioSegundoGrupo : Ejercicio
{
    private ListaNumericaAleatoria numerosRaiz;
    private ListaNumericaAleatoria numerosExp2;
    private ListaNumericaAleatoria numerosExp3;

    public EjercicioSegundoGrupo()
    {
        numerosRaiz = new ListaNumericaAleatoria(30);
        numerosExp2 = new ListaNumericaAleatoria(26);
        numerosExp3 = new ListaNumericaAleatoria(9);
    }

    public string GenerarEjercicio()
    {
        string st = "";
        int num = Random.Range(0, 10000);
        if(num % 2 == 0)
        {
            int numRaiz = (int)Mathf.Pow(numerosRaiz.SiguienteNumero, 2);
            st = "√" + numRaiz;
        }
        else if(num % 3 == 0)
        {
            st = numerosExp2.SiguienteNumero + "²";
        }
        else
        {
            st = numerosExp3.SiguienteNumero + "³";
        }
        return st;
    }
}

public enum Operador
{
	Suma,
	Resta,
	Multiplicacion,
	Division
}

public enum OperadorFuncion
{
    Raiz,
    Exp2,
    Exp3
}