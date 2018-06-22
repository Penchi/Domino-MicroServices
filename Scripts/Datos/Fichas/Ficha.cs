//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;

public class Ficha : MonoBehaviour
{
    public bool conexion;
    public bool colocada;
    public bool fichaAdyacenteAux;
    public int posConexion;
    public int posConexionAdyacente;
    public GameObject fichaAdyacente;
    public GameObject terceraFicha;

    /*
    public bool conexion { get; set; }
    public bool colocada { get; set; }
    public int posConexion { get; set; }
    public int posConexionAdyacente { get; set; }
    public GameObject fichaAdyacente { get; set; }
    public bool fichaAdyacenteAux { get; set; }
    */

    private ConexionFicha[] conexiones;
    private ParticleSystem particleSystem;

	// Use this for initialization
	void Start ()
    {
        conexion = false;
        conexiones = new ConexionFicha[8];
        if(transform.name != "null")
        for (int i = 0; i < conexiones.Length; i++)
        {
            conexiones[i] = transform.Find("conexion" + (i + 1)).GetComponent<ConexionFicha>();
                if(transform.parent.name == "DominoHolder")
                    conexiones[i].ActivarDesactivar();
        }

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        particleSystem = GetComponent<ParticleSystem>();
        particleSystem.startColor = sprite.color;
        particleSystem.Play();
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 0);
        terceraFicha = GameObject.Find("Dummy");
        StartCoroutine("Aparecer");
    }

    IEnumerator Aparecer()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        while (sprite.color.a < 1)
        {
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 0.075f);
            yield return new WaitForSeconds(0.01f);
        }
    }

    IEnumerator Devolver(Vector3 posicionFinal)
    {
        activarDesactivarConexiones("null");
        Vector3 posInicial = transform.position;
        float t = 0f;
        float velAnimacion = 2.5f;
        
        while (t < 1f)
        {
            t += velAnimacion * Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, posicionFinal, t);

            yield return new WaitForSeconds(0);
        }
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void activarDesactivarConexiones(string excepto)
    {
        for (int i = 0; i < conexiones.Length; i++)
            if(conexiones[i].gameObject.name != excepto)
                conexiones[i].ActivarDesactivar();
    }

    public void CambiarConexion(bool conectado, int posConexion, int posConexionAdyacente, GameObject fichaAdyacente)
    {
        conexion = conectado;
        this.posConexion = posConexion;
        this.posConexionAdyacente = posConexionAdyacente;
        this.fichaAdyacente = fichaAdyacente;
    }

    public void ResetearFicha()
    {
        conexion = false;
        terceraFicha = GameObject.Find("Dummy");
        posConexion = 0;
        posConexionAdyacente = 0;
        fichaAdyacente = null;
        fichaAdyacenteAux = false;
    }
}
