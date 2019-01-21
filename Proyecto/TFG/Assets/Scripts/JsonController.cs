using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JsonController {



	//Ruta del archivo donde queremos leer y escribir
	string filePathInventario;

	string filePathFormulasBD;


	string filePathEjercicio;


	//Cadena contenedora del archivo en formato json
	string jsonString;


	public JsonController(){

		filePathInventario = "";

		filePathFormulasBD = "";


		filePathEjercicio = "";


		jsonString = "";


		cargarArcvhivos ();

	}
	public void cargarArcvhivos(){
		//Obtenemos la ruta donde se encuentran los archivos.
		string path = Application.dataPath; 

		//En caso de estar en ejecución, depende del sistema operativo donde se ejecute la aplicación la ruta será una u otra
		if (Application.platform == RuntimePlatform.OSXPlayer) {
			path += "/../../";
		}else if (Application.platform == RuntimePlatform.WindowsPlayer) {
			path += "/../";
		}
		//Obtenemos la ruta del fichero que queremos manejar
		filePathInventario = path + "/BD/Inventario.json";
		filePathFormulasBD = path + "/BD/GruposInorganica/ListaFormulas.json";
		filePathEjercicio = path + "/BD/GruposInorganica/Ejercicio.json";
	}



	//Función encargada de introducir un elemento en el inventario
	public void IntroducirElemento(string tipoAtomo, int numeroUnidades, string label, string grupo){

		//Así sólo se lee cuando vayamos a introducir un elemento
		//Almacenamos en la cadena json el contenido
		jsonString = File.ReadAllText (filePathInventario);


		Inventario inventario;
		//Comprobamos si el fichero contiene algún elemento, en dicho caso obtenemos los elementos, si no inicializamos un nuevo inventario
		if (jsonString.Length > 0) {
			//Decodificamos el json 
			inventario = JsonUtility.FromJson<Inventario> (jsonString);
		} else {
			inventario = new Inventario ();
		}

		//Creamos un elemento nuevo a añadir
		Elemento elemento = new Elemento ();
		elemento.tipoAtomo = tipoAtomo;
		elemento.numeroUnidades = numeroUnidades;
		elemento.label = label;
		elemento.grupo = grupo;


		//Comprobamos si existe el elemento en el inventario, en caso de existir añadimos el número de átomos al hueco que ya teníamos. Limitando a 9 como máximo el número de atomos de un mismo tipo
		if (inventario.listaInventario.Find (e => e.tipoAtomo == elemento.tipoAtomo) != null) {
			int unidades = inventario.listaInventario.Find (e => e.tipoAtomo == elemento.tipoAtomo).numeroUnidades + elemento.numeroUnidades;
			if (unidades > 9) {
				unidades = 9;
			}
			inventario.listaInventario.Find (e => e.tipoAtomo == elemento.tipoAtomo).numeroUnidades = unidades;
		}else if (inventario.listaInventario.Count < inventario.maxSize) {
			//En caso contrario, si hay hueco en el inventario lo introducimos
			//Añadimos a la lista del inventario el nuevo elemento
			inventario.listaInventario.Add (elemento);
		}

		//Escribimos de nuevo en el fichero los elementos del inventario
		jsonString = JsonUtility.ToJson (inventario);
		File.WriteAllText (filePathInventario, jsonString);


	}

	//Eliminamos el contenido del inventario
	public void VaciarInventario(){
		jsonString = "";
		File.WriteAllText (filePathInventario, jsonString);

	}

	// Función encargada de obtener el contenido del archivo inventario serializado en un objeto de la clase Inventario

	public Inventario LeerInventario(){

		//Así sólo se lee cuando vayamos a introducir un elemento
		//Almacenamos en la cadena json el contenido
		jsonString = File.ReadAllText (filePathInventario);


		Inventario inventario;
		//Comprobamos si el fichero contiene algún elemento, en dicho caso obtenemos los elementos, si no inicializamos un nuevo inventario
		if (jsonString.Length > 0) {
			//Decodificamos el json 
			inventario = JsonUtility.FromJson<Inventario> (jsonString);
		} else {
			inventario = null;
		}
		return inventario;
	}


	//Actualizamos el contenido del archivo inventario

	public void ActualizarInventario (Inventario inventario){

		//Escribimos de nuevo en el fichero los elementos del inventario
		jsonString = JsonUtility.ToJson (inventario);
		File.WriteAllText (filePathInventario, jsonString);

	}


	//Leemos el archivo que contiene todas las formulas y las devolvemos en un objeto de la clase Formula
	public Formulas LeerFormulasBaseDatos(){

		//Así sólo se lee cuando vayamos a introducir un elemento
		//Almacenamos en la cadena json el contenido
		jsonString = File.ReadAllText (filePathFormulasBD);


		Formulas formulas;
		//Comprobamos si el fichero contiene algún elemento, en dicho caso obtenemos los elementos, si no inicializamos un nuevo inventario
		if (jsonString.Length > 0) {
			//Decodificamos el json 
			formulas = JsonUtility.FromJson<Formulas> (jsonString);
		} else {
			formulas = null;
		}


		return formulas;
	}

	// Almacenamos en el archivo ejercicio json un ejercicio de una determinada familia
	public void CrearEjercicio(Formulas formula, string familia){
		Formulas ejercicio = new Formulas();


		foreach (Formula form in formula.listaFormulas) {
			if (form.grupoFormula == familia) {

				ejercicio.listaFormulas.Add (form);
			}
		}

		//Escribimos de nuevo en el fichero los elementos del inventario
		jsonString = JsonUtility.ToJson (ejercicio);
		File.WriteAllText (filePathEjercicio, jsonString);

	}

	// Leemos el ejercicio almacenado en el archivo ejercicio json
	public Formulas LeerEjercicio(){
		Formulas ejercicio;

		jsonString = File.ReadAllText (filePathEjercicio);

		//Comprobamos si el fichero contiene algún elemento, en dicho caso obtenemos los elementos, si no inicializamos un nuevo inventario
		if (jsonString.Length > 0) {
			//Decodificamos el json 
			ejercicio = JsonUtility.FromJson<Formulas> (jsonString);
		} else {
			ejercicio = null;
		}


		return ejercicio;


	}


	//Eliminamos el contenido del fichero ejercicio

	public void EliminarEjercicio(){
		jsonString = "";
		File.WriteAllText (filePathEjercicio, jsonString);

	}



}

