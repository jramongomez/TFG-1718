using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;


public class LeapMotionControllerClass : MonoBehaviour {
	//Controlador que nos permitirá acceder a la información del Leap Motion
	protected Controller controller;


	public LeapMotionControllerClass(){
		controller = new Controller ();
	}


	//Función encargada de habilitar gestos
	virtual protected void HabilitarGestos(){


	}

	//Función encargada de deshabilitar gestos
	virtual protected void DeshabilitarGestos(){


	}


	//Función encargada de tratar los gestos detectados
	virtual protected void DeteccionGestos(GestureList gesturesInFrame){


		if (!gesturesInFrame.IsEmpty) {
			foreach (Gesture gesture in gesturesInFrame) {
				switch (gesture.Type) {
				case Gesture.GestureType.TYPECIRCLE:
					//si hemos detectado un gesto del tipo círculo

					if (EventSystem.current.currentSelectedGameObject != null){

						GameObject ObjetoBoton = EventSystem.current.currentSelectedGameObject;
						Button boton = ObjetoBoton.GetComponent<Button> ();

						if (ObjetoBoton.tag == "BotonInterfaz") {

							boton.onClick.Invoke ();
						}

					}

					break;
				}
			}
		}
	}




}
