using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Clases serializables


//Clase que especificará la estructura de un elemento del inventario. Contiene el tipo de atomo, el numero de unidades de dicho tipo, su nombre y el grupo al que pertenece
[System.Serializable]
public class Elemento{
	public string tipoAtomo;
	public int numeroUnidades;
	public string label;
	public string grupo;
}

//Clase que hará referencia al inventario. Contiene el tamaño máximo de atomos que caben en el inventario y una lista de elementos.
[System.Serializable]
public class Inventario{

	public int maxSize = 4;
	public List<Elemento> listaInventario = null;

	public Inventario(){
		listaInventario = new List<Elemento>();
	}
}



//Clase que especifica la estructura de una formula. Contiene el grupo al que pertenece la formula, su nombre y la lista de enlaces necesarios para componerla.

[System.Serializable]
public class Formula {
	public string grupoFormula;
	public string nombreFormula; 
	public List<Enlace> enlaces;

	public Formula(){
		grupoFormula = "";
		nombreFormula = "";
		enlaces = new List<Enlace>();
	}
}

//Clase que especificará una lista de formulas, aqui almacenaremos todas las formulas al leer la base de datos.
[System.Serializable]
public class Formulas {
	public List<Formula> listaFormulas;
	public Formulas(){
		listaFormulas = new List<Formula>();
	}

} 

//Clase que especifica la estructura de un enlace de una formula. Contiene primero un atomo y despues una lista con los atomos con los que esta enlazado
[System.Serializable]
public class Enlace {
	public string atomo;
	public List<string> atomosEnlazados;

	public Enlace(){
		atomo = "";
		atomosEnlazados = new List<string>();
	}
}
