using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AtomoInventarioController : MonoBehaviour {


	// Script que nos da acceso a conocer si un objeto está o no agarrado
	GrabbableObject scriptAtomoGrabbable;


	//Hueco del inventario que ocupa el atomo
	public GameObject espacioInventario = null;


	//Limites del inventario con el area de formulación
	GameObject LimiteSuperiorInventario = null;

	//Comprueba si el atomo esta en el inventario on el area de formulación
	public bool estaInventario = true;

	GameObject PaletaFormulacion = null;

	// Use this for initialization
	void Start () {

		//Asingo el espacio del inventario al inicial

		scriptAtomoGrabbable = this.GetComponent<GrabbableObject> ();

		PaletaFormulacion = GameObject.Find ("PaletaFormulacion").gameObject;



	}

	//Establece en el atomo el simbolo correspondiente
	void EstablecerLabel(string label){
		this.transform.Find ("Label").GetComponent<Text> ().text = label;

	}


	// Update is called once per frame
	void Update () {

		//Corrijo la posición del átomo y vuelvo a poner las barreras del inventario
		FixedPosicion ();


		//this.transform.Find ("Label").transform.localRotation = Quaternion.identity;  //
		this.transform.rotation = Quaternion.identity;



	}

	void OnCollisionEnter(Collision colision) { 

		//Cuando colisiona con el limite superior del inventario lo desactivamos para permitir sacar el atomo del inventario.
		if (colision.gameObject.tag == "LimiteSuperiorInventario" && LimiteSuperiorInventario == null) {
			LimiteSuperiorInventario = colision.gameObject;
			colision.gameObject.SetActive (false);
			if (estaInventario) {
				//En caso de estar en el inventario el atomo, desactivamos el BoundingBox del atomo
				this.transform.Find ("BoundingBox").gameObject.SetActive (false);
			}

		}
			
	}




	void OnTriggerEnter(Collider colision){
		//Cuando no estemos en el inventario y colisionemos con las manos, desbloqueamos el atomo
		if (colision.gameObject.tag == "Manos" && !estaInventario) {
			CancelInvoke ("BloquearAtomo");
			this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation; 
		}
	}
		
	void OnTriggerExit(Collider colision){
		if (colision.gameObject.tag == "Manos") {
			CancelInvoke ("BloquearAtomo");
		}
	}

	//Función encargada de bloquear el atomo para que no este volando libre por la escena
	void BloquearAtomo(){

		this.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezeAll;

	}

	void FixedPosicion(){


		//Activar muro al soltar el atomo

		//Si no tenemos el atomo agarrado
		if (!scriptAtomoGrabbable.IsGrabbed ()) {
			//Si estamos en espacio reservado para el inventario y este está activo, activamos el muro para quue no se salga
			if (this.transform.position.y < PaletaFormulacion.transform.parent.position.y + 1.5f) {


				if (this.espacioInventario.transform.parent.gameObject.active) {
					estaInventario = true;

					this.transform.parent = espacioInventario.transform;


					if (LimiteSuperiorInventario != null) {
						LimiteSuperiorInventario.SetActive (true);
						LimiteSuperiorInventario = null;
					}

				}
				//Si estamos en la posición de la papelera, eliminamos el atomo
				if (this.transform.position.x > 3.0f) {
					if (this.espacioInventario.transform.parent.parent.Find ("BotonPapelera").Find ("Background").GetComponent<UnityEngine.UI.Image> ().color == Color.white) {
						EliminarAtomo ();
					}
				}

			} else {
				//Si estamos fuera de la zona del inventario, activamos el limite con el inventario y activamos el script que se encarga de gestionar los enlaces de los atomos
				if (LimiteSuperiorInventario != null) {
					LimiteSuperiorInventario.SetActive (true);
					LimiteSuperiorInventario = null;
				}
					
				estaInventario = false;

				this.GetComponent<ControlScript> ().enabled = true;

				//Establezco el atomo como hijo de la Paleta de formulacion
				this.transform.parent = PaletaFormulacion.transform;
		


			}



			//Si el átomo no ha salido del inventario, lo colocamos en su hueco dentro del inventario
			if (estaInventario) {
				this.transform.localPosition = new Vector3 (0.0f, 0.0f, -1.0f);
				this.GetComponent<ControlScript> ().enabled = false;
				this.transform.Find ("BoundingBox").gameObject.SetActive (false);

			} else {


				//Activo la bounding box para permitir los enlaces al detecetar los triggers de los Bounding Box
				Invoke ("ActivarBoundingBox", 1.0f);

				//Bloqueamos la posicion del atomo
				Invoke ("BloquearAtomo", 0.1f);


			}

		} else {
			//Si tenemos el atomo agarrado y estamos fuera del inventario
			if (this.transform.position.y > PaletaFormulacion.transform.parent.position.y + 2.0f ) {
				
				estaInventario = false;

				this.GetComponent<ControlScript> ().enabled = true;

				//Activo la bounding box para permitir los enlaces al detecetar los triggers de los Bounding Box
				Invoke ("ActivarBoundingBox", 1.0f);


				this.transform.parent = PaletaFormulacion.transform;
			} else if(this.espacioInventario.transform.parent.parent.Find ("Inventario").gameObject.active) {
				CancelInvoke ("ActivarBoundingBox");
				this.transform.Find ("BoundingBox").gameObject.SetActive (false);

				this.GetComponent<ControlScript> ().enabled = false;

			}
		}
			
	}

	//Función encargada de activar el Bounding Box del atomo para permitir colisiones y enalces
	void ActivarBoundingBox(){
		CancelInvoke ("ActivarBoundingBox");
		this.transform.Find ("BoundingBox").gameObject.SetActive (true);

	}

	//Función encargada de eliminar el atomo. Eliminará también el atomo de la lista del inventario y los enlaces que tenga dicho atomo

	void EliminarAtomo(){
		this.espacioInventario.transform.parent.gameObject.GetComponent<InventarioController> ().EliminarAtomoInventario (this.gameObject.name);

		this.GetComponent<ControlScript>().EliminarEnlace();
		DestroyObject (this.gameObject);

	

	}


}
