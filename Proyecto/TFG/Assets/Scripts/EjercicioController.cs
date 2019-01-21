using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EjercicioController : MonoBehaviour {


	//GambeObject que hace referencia al objeto que tendrá el nombre del ejercicio en caso de estar haciendo uno
	public GameObject nombreEjercicio;

	// GameObject que hace referencia a la Paleta de Formulación
	public GameObject paletaFormulacion;

	//GameObject que hace referencia al panel que mostrará si los resultados son correctos o no
	public GameObject panelSolucion;

	Formulas ejercicio;
	bool click = false;
	int indiceFormula = 0;

	JsonController controladorInventario;

	void Awake(){
		controladorInventario = new JsonController ();
	}

	// Use this for initialization
	void Start () {
		ejercicio = controladorInventario.LeerEjercicio ();

		//Comprobamos si existe un ejercicio seleccionado o por el contrario estamos ante formulación libre
		if (ejercicio != null) {
			if (ejercicio.listaFormulas.Count < 2) {
				this.nombreEjercicio.SetActive (true);
				this.nombreEjercicio.transform.Find ("TitleLabel").GetComponent<Text> ().text = ejercicio.listaFormulas [indiceFormula].nombreFormula;
			}
		} 
	}


	// Función a la que llamaremos cuando queramos comprobar si la estructura quimica que hemos realizado se corresponde con una formula de nuestra base de datos
	public void ComprobarResultado(){


		if (!click ){
			click = true;
			Invoke ("DesbloquearClick", 2.0f);

			//En primer lugar bloqueamos para que solo busque en caso de tener atomos en la paleta de formulación
			if(paletaFormulacion.transform.childCount > 0){


				// Obtengo una lista de los enlaces que tenemos en los atomos dentro de la paleta de formulación
				List<Enlace> enlacesCopia = new List<Enlace>();
				 

				for (int i = 0; i < paletaFormulacion.transform.childCount; i++) {
					Enlace enlace = new Enlace ();
					enlace.atomo = paletaFormulacion.transform.GetChild (i).name;
					paletaFormulacion.transform.GetChild (i).GetComponent<ControlScript> ().enlaces.Sort ();
					enlace.atomosEnlazados = paletaFormulacion.transform.GetChild (i).GetComponent<ControlScript> ().enlaces;
					enlace.atomosEnlazados.Sort ();
					enlacesCopia.Add(enlace);
				}


				//Ahora distinguiremos entre si tenemos un ejercicio disponible o por el contrario si estamos en ejercicios de formulación libre
				if (ejercicio != null) {
					bool errorFormula = false;

					//Comprobamos si hay algún enlace entre nuestros atomos que no se corresponda con alguno de los enlaces necesarios para formar la estructura quimica
					//correspondiente al ejercicio que hemos seleccionado
					//En caso de que todos los enlaces se correspondan, podremos afirmar que hemos realizado correctamente la estructura 
					for (int i = 0; i < ejercicio.listaFormulas [indiceFormula].enlaces.Count && !errorFormula; i++) {

						Enlace enlace = ejercicio.listaFormulas [indiceFormula].enlaces [i];
						enlace.atomosEnlazados.Sort ();

						//Busco el enlace en la lista, si está lo borramos. Si se queda vacía la lista al final y no hemos encontrado errores, habremos dado con la solución
						if (enlacesCopia.Find (e => (e.atomo == enlace.atomo && compararListasString (e.atomosEnlazados, enlace.atomosEnlazados))) != null) {
							enlacesCopia.Remove (enlacesCopia.Find (e => (e.atomo == enlace.atomo && compararListasString (e.atomosEnlazados, enlace.atomosEnlazados))));
						} else {
							errorFormula = true;
						}
					}
					paletaFormulacion.gameObject.SetActive (false);	
					panelSolucion.SetActive (true);
					if (errorFormula) {
						panelSolucion.transform.Find ("SolucionIncorrecta").gameObject.SetActive (true);
						panelSolucion.transform.Find ("SolucionIncorrecta").Find ("TitleLabel").GetComponent<Text> ().text = "La solución es incorrecta";
					
					} else if (enlacesCopia.Count == 0) {
						//Si no nos hemos equivocado en la estructura y todos los enlaces son los mismos, sin haber de más, que el del ejercicio, habremos hecho la solución correcta
						//Activamos por tanto el panel mostrando correcta la solución
						panelSolucion.transform.Find ("SolucionCorrecta").gameObject.SetActive (true);
						panelSolucion.transform.Find ("SolucionCorrecta").Find ("TitleLabel").GetComponent<Text> ().text = "La solución es correcta";

					}
					//Desactivamos el panel automaticamente cuando pasen 5 segundos
					Invoke ("DesactivarPanelSolucion", 5.0f);
				} else {


					//En caso de que no hayamos seleccionado ningún ejercicio, tendremos que repetir un poco lo anterior pero comprobando en todas y cada una de las formulas de la base de datos.
					Formulas formulas = controladorInventario.LeerFormulasBaseDatos ();
				
					bool formulaEncontrada = false;
					string nombreFormula = "";


					//Realizamos una copia de los enlaces de la escena para tenerla guardada, ya que en la lista anterior será donde iremos borrando los enlaces que comprobemos que están correctos
					//Así, podremos recuperar la lista en cualquier momento 
					List<Enlace> copia = new List<Enlace> ();

					for (int i = 0; i < enlacesCopia.Count; i++) {
						copia.Add (enlacesCopia [i]);
					}


					for (int k = 0; k < formulas.listaFormulas.Count && !formulaEncontrada; k++) {

						bool errorFormula = false;
						for (int i = 0; i < formulas.listaFormulas [k].enlaces.Count && !errorFormula; i++) {

							Enlace enlace = new Enlace ();
							enlace.atomo = formulas.listaFormulas [k].enlaces [i].atomo;
							enlace.atomosEnlazados = formulas.listaFormulas [k].enlaces [i].atomosEnlazados;
							enlace.atomosEnlazados.Sort ();


							if (enlacesCopia.Find (e => (e.atomo == enlace.atomo && compararListasString (e.atomosEnlazados, enlace.atomosEnlazados))) != null) {
								enlacesCopia.Remove (enlacesCopia.Find (e => (e.atomo == enlace.atomo && compararListasString (e.atomosEnlazados, enlace.atomosEnlazados))));
							} else {
								errorFormula = true;
							}
						}

						//Si no ha habido error en la formula y la lista de enlaces esta vacia, hemos encontrado la solución
						if (!errorFormula && (enlacesCopia.Count == 0)) {
							formulaEncontrada = true;
							nombreFormula = formulas.listaFormulas [k].nombreFormula;
						} else {
							//En caso contrario reinicio la lista de enlaces
							enlacesCopia = new List<Enlace> ();
							for (int i = 0; i < copia.Count; i++) {
								enlacesCopia.Add (copia [i]);
							}
							errorFormula = false;
						}
					}
					paletaFormulacion.gameObject.SetActive (false);	
					panelSolucion.SetActive (true);

					if (formulaEncontrada) {
						panelSolucion.transform.Find ("SolucionCorrecta").gameObject.SetActive (true);
						panelSolucion.transform.Find ("SolucionCorrecta").Find ("TitleLabel").GetComponent<Text> ().text = "La estructura es correcta, su nomenclatura es: " + nombreFormula;
					} else {
						panelSolucion.transform.Find ("SolucionIncorrecta").gameObject.SetActive (true);
						panelSolucion.transform.Find ("SolucionIncorrecta").Find ("TitleLabel").GetComponent<Text> ().text = "La estructura es incorrecta o no se encuentra en nuestra base de datos";
					
					}
					
						Invoke ("DesactivarPanelSolucion", 5.0f);
					

				}

				
			} else {
				paletaFormulacion.SetActive (false);	

				panelSolucion.SetActive (true);
				panelSolucion.transform.Find ("NoHayAtomos").gameObject.SetActive (true);
				Invoke ("DesactivarPanelSolucion", 5.0f);
			}
		}


	}

	//Función que compara si dos listas ordenadas alfabéticamente son iguales
	bool compararListasString(List<string> lista1, List<string> lista2){
		
		bool iguales = true;

		if(lista1.Count == lista2.Count){
			for (int i = 0; i < lista1.Count && iguales; i++) {

				if (lista1 [i] != lista2 [i])
					iguales = false;
			}
		}else{
			iguales = false;
		}



		return iguales;
	}

	void DesbloquearClick(){
		CancelInvoke ("DesbloquearClick");
		click = false;
	}

	//Función encargada de desactivar el panel que muestra si una solución es o no correcta
	void DesactivarPanelSolucion(){
		panelSolucion.transform.Find ("SolucionCorrecta").gameObject.SetActive (false);
		panelSolucion.transform.Find ("SolucionIncorrecta").gameObject.SetActive (false);
		panelSolucion.transform.Find ("NoHayAtomos").gameObject.SetActive (false);
		panelSolucion.SetActive (false);
		paletaFormulacion.SetActive (true);	


	}
}
