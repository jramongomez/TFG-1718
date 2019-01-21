using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;


public class LeapMotionTablaAndEjerciciosController : LeapMotionControllerClass {



	protected Button botonAtras;

	void Start ()
	{
		//Inicializamos el controlador
		controller = new Controller();

		//Habilitamos los gestos para detectar la posible selección
		Invoke ("HabilitarGestos", 1.0f);

		botonAtras = this.transform.Find ("BotonesMenu").transform.Find ("Atras").GetComponent<Button> ();
	}


	void Update ()
	{
		//Obtenemos la información de cada frame del LeapMotion
		Frame frame = controller.Frame();

		//Obtener los gestos que ha detectado LM
		GestureList gesturesInFrame = frame.Gestures();

		// Comprobamos en cada frame que gestos se han detectado para una posible selección
		DeteccionGestos (gesturesInFrame);


	}

	override protected void HabilitarGestos(){

		controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE);
		controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);


	}

	override protected void DeshabilitarGestos(){

		controller.EnableGesture(Gesture.GestureType.TYPE_SWIPE, false);
		controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE, false);

	}


	override protected void DeteccionGestos(GestureList gesturesInFrame){


		if (!gesturesInFrame.IsEmpty) {
			foreach (Gesture gesture in gesturesInFrame) {
				switch (gesture.Type) {
				case Gesture.GestureType.TYPE_SWIPE:
					// Si detectamos el gesto swipe, si es de izquierda a derecha iremos hacia atrás
					SwipeGesture swipeGesture = new SwipeGesture (gesture);

					if (swipeGesture.StartPosition.x < swipeGesture.Position.x && ( Mathf.Abs((swipeGesture.StartPosition.x - swipeGesture.Position.x)) > 150.0f  )) { 
						botonAtras.onClick.Invoke();
						DeshabilitarGestos ();
					}

					break;
				case Gesture.GestureType.TYPECIRCLE:
					//si hemos detectado un gesto del tipo círculo

					//Buscamos si alguno de nuestros botones se corresponde con el seleccionado en la escena
					if (EventSystem.current.currentSelectedGameObject != null){

						GameObject ObjetoBoton = EventSystem.current.currentSelectedGameObject;
						Button boton = ObjetoBoton.GetComponent<Button> ();

						if (ObjetoBoton.tag == "BotonEjercicio") {

							boton.onClick.Invoke ();
						}
						else if (ObjetoBoton.tag == "BotonInterfaz") {

							boton.onClick.Invoke ();
						}else if(ObjetoBoton.tag == "Elemento") {

							boton.onClick.Invoke ();
						}

					}

					break;
				}
			}
		}
	}


}
