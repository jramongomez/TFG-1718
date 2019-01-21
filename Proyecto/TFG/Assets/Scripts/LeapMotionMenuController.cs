using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;





public class LeapMotionMenuController : LeapMotionControllerClass {




	void Start ()
	{
		//Inicializamos el controlador	
		controller = new Controller();
	
		//Habilitamos los gestos para detectar la posible selección
		Invoke ("HabilitarGestos", 1.0f);
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

		controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE);


	}

	override protected void DeshabilitarGestos(){

		controller.EnableGesture(Gesture.GestureType.TYPE_CIRCLE,false);

	}

}
