using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Leap;

public class LeapMotionGruposController : LeapMotionTablaAndEjerciciosController {




	public GameObject[] paginas = null;

	bool click = false;

	void Start ()
	{

		botonAtras = this.transform.Find ("BotonesMenu").transform.Find ("Atras").GetComponent<Button> ();

		controller = new Controller();

		Invoke ("HabilitarGestos", 1.0f);


	}


	void Update ()
	{
		Frame frame = controller.Frame();

		//Obtener los gestos que ha detectado LM
		GestureList gesturesInFrame = frame.Gestures();

		DeteccionGestos (gesturesInFrame);

	}




	//En caso de tener más elementos de los que caben en la escena, para controlar los cambios de página
	public void CambiarPagina(){

		if (!click) {
			click = true;
			Invoke ("DesbloquearClick", 1.5f);
			if (paginas.Length > 2) {

				if (paginas [0].active) {
					paginas [1].SetActive (true);
					paginas [0].SetActive (false);

				} else if (paginas [1].active) {
					paginas [2].SetActive (true);
					paginas [1].SetActive (false);

				} else if (paginas [2].active) {
					paginas [0].SetActive (true);
					paginas [2].SetActive (false);

				}

			}
			else if (paginas.Length == 2) {

				if (paginas [0].active) {
					paginas [1].SetActive (true);
					paginas [0].SetActive (false);

				} else if (paginas [1].active) {
					paginas [0].SetActive (true);
					paginas [1].SetActive (false);

				} 

			}
		}


	}

	void DesbloquearClick(){
		CancelInvoke ("DesbloquearClick");
		click = false;
	}

}