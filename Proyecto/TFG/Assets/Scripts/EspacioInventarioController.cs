using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EspacioInventarioController : MonoBehaviour {



	//Guarda la información del elemento que se almacenará en el hueco correspondiente
	public Elemento elementoEspacio = null;

	//Game Object que establece el nombre del atomo que se guarda en dicho hueco
	GameObject nombreElemento;

	//Game Object que establece el numero de atomos que se guardan en dicho hueco
	GameObject numeroUnidades;

	//Indica si hay atomos asignados en dicho espacio o no
	public bool espacioAsignado = false;


	//Prefab que nos servirá para instanciar los atomos
	public GameObject AtomoPrefab = null;

	// Use this for initialization
	void Start () {
		//Cargamos el nombre y el numero de unidades del elemento
		nombreElemento = this.transform.Find ("NombreElemento").gameObject.transform.Find ("Background").gameObject.transform.Find ("Label").gameObject;
		numeroUnidades = this.transform.Find ("NumeroUnidades").gameObject.transform.Find ("Background").gameObject.transform.Find ("Label").gameObject;

	}

	// Update is called once per frame
	void Update () {

		//Si el espacio no está asginado, si hemos establecido anteriormente el elemento, instanciamos el número de átomos del tipo correspondiente, asignando el color correspondiente.
		if (!espacioAsignado) {
			if (elementoEspacio != null) {
				InstanciarAtomosEspacio ();
			}

		} 

		//Actualizamos el espacio, esencial a la hora de llevar el contador de atomos diponibles en el espacio
		ActualizarEspacio ();
	}

	public void ActualizarEspacio(){
		// el numero de elementos del espacio será el numero de hijos que tiene el espacio menos 2, estos dos hacen referencia al nombre del elemento y el numero de atomos. 
		if (elementoEspacio != null) {
			numeroUnidades.GetComponent<Text> ().text = (this.transform.childCount - 2).ToString ();
		} 
	}

	void InstanciarAtomosEspacio(){
		espacioAsignado = true;
		nombreElemento.GetComponent<Text> ().text = elementoEspacio.tipoAtomo;


		for (int i = 0; i < elementoEspacio.numeroUnidades; i++) {

			GameObject atomo = Instantiate (AtomoPrefab, this.transform.position, Quaternion.identity);
			atomo.name = elementoEspacio.tipoAtomo;
			atomo.SendMessage ("EstablecerLabel", elementoEspacio.label);
			atomo.GetComponent<AtomoInventarioController> ().espacioInventario = this.gameObject;


			//Establezco el color del átomo en función del grupo
			Material newMat = Resources.Load("Materials/"+ elementoEspacio.grupo, typeof(Material)) as Material;
			atomo.GetComponent<Renderer>().material = newMat;
		}
	}
}

