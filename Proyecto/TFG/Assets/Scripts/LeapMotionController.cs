using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using Leap;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

using UnityEngine.EventSystems;


public class LeapMotionController : LeapMotionTablaAndEjerciciosController {



	// Script controlador que hace referencia al objeto que controlará los cambios de escena
	public ControladorEscenas controlador = null;


	//Game objects correspondientes  a los botones con los que vamos a controlar el despliegue del inventario, de la tabla, comprobar solución o eliminar atomos
	public GameObject BotonInventario = null;
	public GameObject BotonTabla = null;
	public GameObject BotonPapelera = null;
	public GameObject BotonSolucion = null;


	//Game Object que hace referencia al inventario
	public GameObject Inventario = null;

	//Atributos que nos indica si tenemos los botones seleccionados o no
	bool estadoBotonInventario = false;
	bool estadoBotonTabla = false;
	bool estadoBotonSolucion = false;
	public bool estadoBotonPapelera = false;


	//Color inicial de los botones antes de estar seleccionados
	Color colorInicialBotones;

	void Start ()
	{
		//Inicializo el controlador del Leap Motion
		controller = new Controller();

		//Habilitamos los gestos un segundo después de la carga de la escena para que no haya cambios involuntarios por algun gesto sin querer
		Invoke ("HabilitarGestos", 1.0f);


		colorInicialBotones = BotonInventario.transform.Find ("Background").GetComponent<UnityEngine.UI.Image> ().color;

	}


	void Update ()
	{
		//Obtenemos la información captada por el Leap Motion en cada Frame
		Frame frame = controller.Frame();

		//Obtener los gestos que ha detectado LM
		GestureList gesturesInFrame = frame.Gestures();



		// Obtenemos las manos detectadas en el frame actual
		HandList hands = frame.Hands;


		// Si hemos detectado alguna mano
		if (!hands.IsEmpty) {

			//Seleccionamos la primera mano detectada
			Hand hand = hands [0];

			//Si no tenemos el panel de la solución activo, permitimos el manejo de los botones
			if (!GameObject.Find ("CanvasFormulacion").transform.Find ("PanelSolucion").gameObject.active) {
				
				ManejarInventario (hand);
				ManejarTabla (hand);
				ManejarSolucion (hand);
				ManejarPapelera (hand);
			}
		} else {

			//El inventario se ocultará automaticamente a los 5 minutos de sacar las manos de la pantalla
			if (Inventario.active) {
				Invoke ("OcultarInventario", 5.0f);
			}
		}

		//Llamamos a la función que se encargará de tratar los gestos
		DeteccionGestos (gesturesInFrame);


	}



	//Función que se encargará de tratar los gestos
	override protected void DeteccionGestos(GestureList gesturesInFrame){


		// Si hemos detectado algún gesto
		if (!gesturesInFrame.IsEmpty) {
			//Recorremos la lista de gestos detectados
			foreach (Gesture gesture in gesturesInFrame) {
				switch (gesture.Type) {
				//Si el gesto detectado es del tipo swipe
				case Gesture.GestureType.TYPE_SWIPE:
					
					//Obtenemos una copia del gesto para obtener sus propiedades
					SwipeGesture swipeGesture = new SwipeGesture (gesture);

					// Si hemos hecho un movimiento de izquierda a derecha con un recorrido de unos 150 en la escala del sensor de leap
					// Cambiaremos la escena a la anterior, como si fueramos hacia atrás.

					if (swipeGesture.StartPosition.x < swipeGesture.Position.x && ( Mathf.Abs((swipeGesture.StartPosition.x - swipeGesture.Position.x)) > 150.0f  )) { 
						controlador.cambiarEscena ("Menu");


					}

					break;
				case Gesture.GestureType.TYPE_CIRCLE:

					//Si no tenemos activos el panel de solución, permitimos seleccionar los demás botones
					if (!GameObject.Find ("CanvasFormulacion").transform.Find ("PanelSolucion").gameObject.active) {
						if (BotonInventario.active) {
							if (estadoBotonInventario) {

								BotonInventario.GetComponent<Button> ().onClick.Invoke ();


							} else if (estadoBotonTabla) {

								BotonTabla.GetComponent<Button> ().onClick.Invoke ();

							} else if (estadoBotonSolucion) {

								BotonSolucion.GetComponent<Button> ().onClick.Invoke ();

							}
						}
					}
					break;

				}
			}
		}
	}
		

	//Función que ocultará el inventario cuando la llamemos
	void OcultarInventario (){

		CancelInvoke ("OcultarInventario");
		BotonInventario.GetComponent<Button> ().onClick.Invoke ();

	}


	//Función encargada de manejar el botón inventario 
	void ManejarInventario(Hand hand){

		Vector handCenter = hand.PalmPosition;


		if (BotonInventario.active) {
			if ((handCenter.x > -100.0f && handCenter.x < -35.0f ) && (handCenter.y > 60.0f && handCenter.y < 160.0f)) {

				BotonInventario.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
				estadoBotonInventario = true;

			} else {
				if (estadoBotonInventario) {
					BotonInventario.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = this.colorInicialBotones;
					estadoBotonInventario = false;
				}

			}
		}
		//El menu se ocultará automáticamente a los 3 seg de sacar la mano del área del inventario
		else if (handCenter.y > 160.0f) {
			Invoke ("OcultarInventario", 5.0f);

		} else if (handCenter.y < 150.0f) {
			CancelInvoke ("OcultarInventario");

		}

	}

	//Función encargada de manejar el botón del despliegue de tabla 
	void ManejarTabla(Hand hand){

		Vector handCenter = hand.PalmPosition;


		if (BotonTabla.active) {
			if ((handCenter.x > -30.0f && handCenter.x < 35.0f ) && (handCenter.y > 60.0f && handCenter.y < 160.0f)) {

				BotonTabla.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
				estadoBotonTabla = true;

			} else {
				if (estadoBotonTabla) {
					BotonTabla.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = this.colorInicialBotones;
					estadoBotonTabla = false;
				}

			}
		}

	}


	//Función encargada de manejar el botón encontrar solución 

	void ManejarSolucion(Hand hand){

		Vector handCenter = hand.PalmPosition;


		if (BotonSolucion.active) {
			if ((handCenter.x > 40.0f && handCenter.x < 95.0f ) && (handCenter.y > 60.0f && handCenter.y < 160.0f)) {

				BotonSolucion.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
				estadoBotonSolucion = true;

			} else {
				if (estadoBotonSolucion) {
					BotonSolucion.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = this.colorInicialBotones;
					estadoBotonSolucion = false;
				}

			}
		}

	}

	//Función encargada de manejar el botón papelera

	void ManejarPapelera(Hand hand){

		Vector handCenter = hand.PalmPosition;


		if (BotonPapelera.active) {
			if ((handCenter.x > 100.0f && handCenter.x < 250.0f ) && (handCenter.y > 60.0f && handCenter.y < 160.0f)) {

				BotonPapelera.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = Color.white;
				estadoBotonPapelera = true;

			} else {
				if (estadoBotonPapelera) {
					BotonPapelera.transform.Find ("Background").gameObject.GetComponent<UnityEngine.UI.Image> ().color = this.colorInicialBotones;
					estadoBotonPapelera = false;
				}

			}
		}

	}



}