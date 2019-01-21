using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioController : MonoBehaviour {

	// Objeto de la clase inventario del que leeremos los atomos que hemos seleccionado anteriormente
	Inventario inventario = null;

	//Vector de Game Objects referentes a los huecos disponibles en el inventario
	public GameObject[] huecosInventario;

	bool click = false;

	JsonController controladorInventario;

	void Awake(){
		controladorInventario = new JsonController ();

	}
	// Use this for initialization
	void Start () {
		//Al iniciar la escena, leemos el archivo del inventario y asignamos a cada hueco del inventario los atomos correspondientes
		ActualizarInventario ();
	}

	//Función encargada de leer el archivo de inventario y asignar a cada hueco del inventario los atomos correspondientes
	public void ActualizarInventario(){
		
		inventario = controladorInventario.LeerInventario ();

		if (inventario != null){
			for (int i = 0; i < inventario.listaInventario.Count; i++) {
				huecosInventario [i].GetComponent<EspacioInventarioController> ().elementoEspacio = inventario.listaInventario [i];
			}
		}
			

	}


	//Función que se encarga de abrir/cerrar el inventario y de desactivar/activar los botones 
	public void ActivarInventario(){


		if (click) {
			if (this.gameObject.active) {
				click = false;
				this.transform.parent.Find ("BotonInventario").gameObject.SetActive (true);
				this.transform.parent.Find ("BotonTablaPeriodica").gameObject.SetActive (true);
				this.transform.parent.Find ("BotonResolver").gameObject.SetActive (true);
				this.transform.parent.Find ("BotonPapelera").gameObject.SetActive (true);

				this.gameObject.SetActive (false);
			}
		} else {

			if (this.transform.parent.Find ("BotonInventario").gameObject.active) {
				click = true;
				this.transform.parent.Find ("BotonInventario").gameObject.SetActive (false);
				this.transform.parent.Find ("BotonTablaPeriodica").gameObject.SetActive (false);
				this.transform.parent.Find ("BotonResolver").gameObject.SetActive (false);
				this.transform.parent.Find ("BotonPapelera").gameObject.SetActive (false);
				this.gameObject.SetActive (true);

			}
		}


	}

	//Función encargada de eliminar un átomo del inventario y lo actualiza en el archivo
	public void EliminarAtomoInventario(string nombreAtomo){
		bool encontrado = false;
		for (int i = 0; i < inventario.listaInventario.Count && !encontrado; i++) {
			if (inventario.listaInventario [i].tipoAtomo == nombreAtomo) {
				encontrado = true;
				if (inventario.listaInventario [i].numeroUnidades > 1) {
					inventario.listaInventario [i].numeroUnidades -= 1;

				} else {
					inventario.listaInventario.RemoveAt (i);
				}
			}

		}


		controladorInventario.ActualizarInventario (inventario);

	}
}




