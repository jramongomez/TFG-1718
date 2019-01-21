using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap;
using UnityEngine.UI;

using System;
using System.Diagnostics;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;

public class PunteroController : LeapMotionControllerClass {



	//Para controlar después las selecciones del puntero
	GameObject calculadora = null;



	void Start ()
	{
		//Inicializo el controlador del Leap Motion
		controller = new Controller();

		//Busco si existe un panel de calculadora en la escena para usar
		if (this.transform.parent.Find ("PanelCalculadora") != null) {
			calculadora = this.transform.parent.Find ("PanelCalculadora").gameObject;
		}

	}


	void Update ()
	{


		//manosInvisibles.transform.childCount != 0
		Frame frame = controller.Frame();
	
		// Obtenemos las manos detectadas en el frame actual
		HandList hands = frame.Hands;


		// Si hemos detectado alguna mano
		if (! hands.IsEmpty ) {

			//Seleccionamos la primera mano detectada
			Hand hand = hands [0];

			NormalizarCoordenadasPuntero (hand);


		}

	}

	void NormalizarCoordenadasPuntero(Hand hand){
		//Obtenemos la posición del centro de la mano
		Vector handCenter = hand.PalmPosition;


		//Normalizamos la posición del centro de la mano para el movimiento del puntero
		float newX = 0.0f;
		float newY = 0.0f;


		//El 18 se debe a que la escena mide 18 unidades aproximadamente de ancho (-9,9)
		newX = (handCenter.x / 18.0f);

		//El 10 se debe a que la escena mide 10 unidades aproximadamente de alto (-5,5)
		//El 250 se debe a la posición del leap inicial, ya que el gameobject que contiene el Hand Controller Script lo hemos posicionado inicialmente en esta posición (-250 en y).
		newY = (handCenter.y - 250.0f) /10.0f;

		//Movemos el puntero a la nueva posicion
		this.transform.position = new Vector3 (newX, newY, 1.0f);
	}
		
	void OnTriggerEnter2D(Collider2D collision) {

		// Si existe una calculadora activada, solo trataremos las colisiones con sus teclas
		if(calculadora != null && calculadora.active ){
			if (collision.tag == "Tecla") {

				collision.gameObject.GetComponent<Button> ().Select ();


			}
		}
		else if (collision.tag == "Elemento") {

			collision.gameObject.GetComponent<Button> ().Select ();


		}
		else if (collision.tag == "BotonInterfaz") {

			collision.gameObject.GetComponent<Button> ().Select ();


		}
		else if (collision.tag == "BotonEjercicio") {

			collision.gameObject.GetComponent<Button> ().Select ();


		}


	}




}

