using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScript : MonoBehaviour {


	// Atributo que nos indicará si un átomo esta enlazado con otro
	public bool enlazado = false;



	// Objeto que hará referencia al enlace con otro átomo
	GameObject enlace = null;


	GameObject atomoEnlace = null;

	public GameObject atomoCreadorEnlace = null;


	public List<string> enlaces;

	// Use this for initialization
	void Start () {
		enlaces = new List<string> ();
	}
	
	// Update is called once per frame
	void Update () {

		ActualizarPosicionesEnlace ();


		//Si tenemos un atomo asignado al enlace
		if (atomoEnlace != null) {
			//Si la distancia euclidea entre este atomo y con el que esta enlazado es de más de 3, rompemos el enlace
			float dist = Vector3.Distance (this.transform.position, atomoEnlace.transform.position);
			if (dist > 3.0f) {
				EliminarEnlace ();
			}
		}




	}
		
	void ActualizarPosicionesEnlace (){

		//Si hemos enlazado este atomo
		if (enlazado && atomoEnlace != null) {

			//Actualizamos la posicion del enlace (la linea entre los centros de los atomos)
			//asignarPosicionesEnlace (this.transform.position, this.transform.parent.position);
			asignarPosicionesEnlace (this.transform.position, atomoEnlace.transform.position);

		} else if (!enlazado) {
			// Si el atomo no esta enlazado no tiene un atomo asociado a dicho enlace
			atomoEnlace = null;
		}
	}


	void OnTriggerEnter(Collider colision) { 


		//Si detectamos la colisión contra un atomo ( atomo o bounding box, ya que al ser su hijo tendrá la etiqueta del padre)
		if (colision.gameObject.tag == "Atomo" ){

			//print ("DetectadaColisionAtomo");
			//Si no somos el padre de otro enlace (es decir, no nos hemos enlazado anteriormente, esto es para evitar que cuando se choquen dos se manden dos enlaces) y no nos hemos enlazado ya
			if (this.transform.Find ("Enlace") == null && !enlazado) {
				enlazarAtomos (colision.gameObject);
			}
			//Debug.Log (colision.gameObject.tag);
		}




	}

	// Nos permitirá enlazar dos atomos mediante un enlace
	void enlazarAtomos (GameObject colisionAtomo){

	

		atomoEnlace = colisionAtomo.transform.gameObject;
		//Creo una linea (enlace) entre ambos atomos
		crearEnlace ();


		//Establezco las posiciones de los vértices que componen el enlace
		asignarPosicionesEnlace (this.transform.position, colisionAtomo.transform.position);


		//Asigno la linea como hijo del atomo con el que hemos colisionado
		this.enlace.transform.parent = colisionAtomo.transform;
		colisionAtomo.GetComponent<ControlScript> ().atomoCreadorEnlace = this.gameObject;

		enlaces.Add (colisionAtomo.name);

		colisionAtomo.GetComponent<ControlScript> ().enlaces.Add (this.name);

		//Indico que ya hemos enlazado el atomo
		enlazado = true;



	} 

	// Método que nos permitirá crear una línea que nos servirá de enlace
	void crearEnlace(){


		//Inicializamos el objeto enlace
		enlace = new GameObject ();

		//Añadimos a nuestro objeto nombre y etiqueta
		enlace.name = "Enlace";
		enlace.tag = "Enlace";

		//Añadimos a nuestro objeto el componente LineRenderer que es el que nos permitirá lanzar una linea entre x posiciones
		enlace.AddComponent<LineRenderer> ();

		LineRenderer lineRenderer = enlace.GetComponent<LineRenderer> ();

		//Establecemos el tamaño de la línea (enlace)
		lineRenderer.startWidth= 0.1f;
		lineRenderer.endWidth = 0.1f;

		//Asignamos el material del que estará hecho nuestro enlace
		Material material = Resources.Load("MaterialLine", typeof(Material)) as Material;
		lineRenderer.material = material;

	}

	// Método que nos permitirá indicar las posiciones entre las que se lanzará la línea o se creará el enlace
	void asignarPosicionesEnlace(Vector3 posicion0, Vector3 posicion1){
		Vector3[] posiciones = new Vector3[2];
		posiciones [0] = posicion0;
		posiciones [1] = posicion1;
		enlace.GetComponent<LineRenderer> ().SetPositions (posiciones);
	}


	public void EliminarEnlace(){

		if (enlazado) {
			enlazado = false;

			if (atomoEnlace != null) {
				this.enlaces.Remove (atomoEnlace.name);
				if (atomoEnlace.transform.Find ("Enlace") != null) {
					atomoEnlace.GetComponent<ControlScript> ().enlaces.Remove (this.name);
					DestroyObject (enlace);
					atomoEnlace.GetComponent<ControlScript> ().atomoCreadorEnlace = null;
				}
			}
			atomoEnlace = null;

		} 
	}
}
