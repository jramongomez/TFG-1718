using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;

public class LeapMotionCalculadoraController : LeapMotionTablaAndEjerciciosController {


	bool activado = false;


	void Start ()
	{
		controller = new Controller();
		activado = false;

		botonAtras = this.transform.Find ("BotonesMenu").transform.Find ("Atras").GetComponent<Button> ();


	}


	void Update ()
	{
		
		Frame frame = controller.Frame ();
		//Obtener los gestos que ha detectado LM
		GestureList gesturesInFrame = frame.Gestures();

		DeteccionGestos (gesturesInFrame);
	



	}

	public void Activado(){
		activado = true;

	}
	public void Desactivado(){
		activado = false;

	}



	override protected void DeteccionGestos(GestureList gesturesInFrame){


		if (!gesturesInFrame.IsEmpty) {
			
			foreach (Gesture gesture in gesturesInFrame) {
				switch (gesture.Type) {
				case Gesture.GestureType.TYPE_SWIPE:
					// Si detectamos el gesto swipe, si es de izquierda a derecha iremos hacia atrás
					SwipeGesture swipeGesture = new SwipeGesture (gesture);
					if (swipeGesture.StartPosition.x < swipeGesture.Position.x && (Mathf.Abs ((swipeGesture.StartPosition.x - swipeGesture.Position.x)) > 150.0f)) { 
						botonAtras.onClick.Invoke ();
					}
					break;
				case Gesture.GestureType.TYPECIRCLE:
					//si hemos detectado un gesto del tipo círculo
				
					if (activado) {
						//Buscamos si alguno de nuestros botones se corresponde con el seleccionado en la escena
						if (EventSystem.current.currentSelectedGameObject != null) {

							GameObject ObjetoBoton = EventSystem.current.currentSelectedGameObject;

							if (ObjetoBoton.tag == "Tecla") {


								Button boton = ObjetoBoton.GetComponent<Button> ();

								//Llamamos a su método onClick.
								boton.onClick.Invoke ();

							
							}

						}
					}


					break;
				}
			}
		} 
	}
		




}
