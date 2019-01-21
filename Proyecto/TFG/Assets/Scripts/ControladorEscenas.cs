using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorEscenas : MonoBehaviour {

	public GameObject calculadora = null;
	JsonController controladorInventario;


	void Awake(){
		controladorInventario = new JsonController ();

	}
	void Start(){


		//En caso de estar en el Menu inicial, borraremos los archivos json del inventario y del ejercicio que habiamos seleccionado
		if (SceneManager.GetActiveScene().name == "Menu") {
			controladorInventario.VaciarInventario ();
			controladorInventario.EliminarEjercicio ();
		}

	}
	// Función encargada de cambiar la escena actual por la de nombreEscena
	public void cambiarEscena(string nombreEscena){
		SceneManager.LoadScene (nombreEscena);
	}

	// Función utilizada para salir del juego. Al salir borrará también el contenido de los archivos json de inventario y el ejercicio
	public void salir(){
		controladorInventario.VaciarInventario ();
		controladorInventario.EliminarEjercicio ();
		Application.Quit ();



	}

	// Función utilizada para abrir la calculadora
	public void abrirCalculadora(string elemento){
		calculadora.GetComponent<ControladorCalculadora> ().IniciarCalculadora (elemento);

	}

	// Función utilizada para cerrar la calculadora
	public void cerrarCalculadora(){
		calculadora.GetComponent<ControladorCalculadora> ().CerrarCalculadora ();
	}


	// Función utilizada para ir a la escena de formulación cargando un ejercicio

	public void IniciarEjercicioFormulacion(string Familia){
		Formulas formulas = controladorInventario.LeerFormulasBaseDatos();

		controladorInventario.CrearEjercicio (formulas, Familia);

		SceneManager.LoadScene ("Formulacion");
	}
}
