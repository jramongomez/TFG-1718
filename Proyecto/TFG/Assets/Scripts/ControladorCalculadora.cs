using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControladorCalculadora : MonoBehaviour {


	public GameObject resultado;
	public GameObject tipoAtomo;

	JsonController controladorInventario;
	string elemento = null;
	int acumulado = 0;
	bool click = false;


	void Awake(){

		controladorInventario = new JsonController ();
	}


	//Función encargada de añadir una cifra al panel de la calculadora. Limitamos la interacción con el bool click para evitar que muchos gestos circle hagan que se dispare la función
	public void añadirCifra(string cifra){
		
		if (!click) {
			click = true;
			Invoke ("DesbloquearClick", 1.5f);
			resultado.GetComponent<Text> ().text = cifra;
		}
	}

	//Función encargada de permitir volver a añadir una cifra o pulsar una tecla de la calculadora
	void DesbloquearClick(){
		CancelInvoke ("DesbloquearClick");
		click = false;
	}

	//Función encargada de borrar el resultado del panel de la calculadora
	public void clear(){
		resultado.GetComponent<Text> ().text = "0";
	}

	//Función encargada de almacenar el número de atomos seleccionado en la calculadora en el inventario
	public void Ok(){
		
		string simboloQuimico = ObtenerSimbolo ();

			
		string grupo = SceneManager.GetActiveScene().name;
		acumulado = int.Parse(resultado.GetComponent<Text> ().text);
		if (acumulado > 0) {
			string nombreElemento = ObtenerNombreElemento ();
			controladorInventario.IntroducirElemento (nombreElemento, acumulado, simboloQuimico, grupo);
		}
		CerrarCalculadora ();
	}


	//Función que se encarga de obtener el símbolo del elemento que hemos seleccionado, para después usarlo para ponerselo en la esfera
	string ObtenerSimbolo(){

		string simboloQuimico = "";

		if(this.transform.parent.parent.Find ("Grupo").Find (elemento) != null)
			simboloQuimico = this.transform.parent.parent.Find ("Grupo").Find (elemento).Find ("Background").Find("SimboloQuimico").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("PrimeraPagina").Find (elemento) != null)
			simboloQuimico = this.transform.parent.parent.Find ("Grupo").Find ("PrimeraPagina").Find (elemento).Find ("Background").Find("SimboloQuimico").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("SegundaPagina").Find (elemento) != null)
			simboloQuimico = this.transform.parent.parent.Find ("Grupo").Find ("SegundaPagina").Find (elemento).Find ("Background").Find("SimboloQuimico").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("TerceraPagina").Find (elemento) != null)
			simboloQuimico = this.transform.parent.parent.Find ("Grupo").Find ("TerceraPagina").Find (elemento).Find ("Background").Find("SimboloQuimico").GetComponent<Text>().text;
		
		return simboloQuimico;
	}


	string ObtenerNombreElemento(){

		string nombreElemento = "";

		if(this.transform.parent.parent.Find ("Grupo").Find (elemento) != null)
			nombreElemento = this.transform.parent.parent.Find ("Grupo").Find (elemento).Find ("Background").Find("Elemento").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("PrimeraPagina").Find (elemento) != null)
			nombreElemento = this.transform.parent.parent.Find ("Grupo").Find ("PrimeraPagina").Find (elemento).Find ("Background").Find("Elemento").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("SegundaPagina").Find (elemento) != null)
			nombreElemento = this.transform.parent.parent.Find ("Grupo").Find ("SegundaPagina").Find (elemento).Find ("Background").Find("Elemento").GetComponent<Text>().text;
		else if(this.transform.parent.parent.Find ("Grupo").Find ("TerceraPagina").Find (elemento) != null)
			nombreElemento = this.transform.parent.parent.Find ("Grupo").Find ("TerceraPagina").Find (elemento).Find ("Background").Find("Elemento").GetComponent<Text>().text;

		return nombreElemento;
	}

	//Función que despliega la calculadora
	public void IniciarCalculadora(string elemento){


		this.elemento = elemento;
		acumulado = 0;
		clear ();
		string nombreElemento = ObtenerNombreElemento ();
		tipoAtomo.GetComponent<Text> ().text = "Unidades " + nombreElemento;


		DeshabilitarComponenteLeapGrupos ();
		this.transform.parent.GetComponent<LeapMotionCalculadoraController>().enabled = true;
		this.transform.parent.GetComponent<LeapMotionCalculadoraController> ().Invoke ("Activado", 1.5f);

		this.transform.parent.gameObject.SetActive (true);


	}


	//Función que cierra la calculadora

	public void CerrarCalculadora(){



		Invoke ("HabilitarComponenteLeapGrupos", 2.0f);
		this.transform.parent.GetComponent<LeapMotionCalculadoraController> ().Desactivado ();

		this.transform.parent.GetComponent<LeapMotionCalculadoraController>().enabled = false;

		this.transform.parent.gameObject.SetActive (false);



	}


	//Para que la escena de los grupos empiece a detectar los gestos realizados con el Leap
	void HabilitarComponenteLeapGrupos(){


		this.transform.parent.parent.GetComponent<LeapMotionGruposController> ().enabled = true;
	

			
	}

	//Para que la escena de los grupos deje de detectar los gestos realizados con el Leap para poder tratar los de la calculadora
	void DeshabilitarComponenteLeapGrupos(){

		this.transform.parent.parent.GetComponent<LeapMotionGruposController> ().enabled = false;

	}

}
