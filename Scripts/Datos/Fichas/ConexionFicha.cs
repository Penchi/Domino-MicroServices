//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System;

public class ConexionFicha : MonoBehaviour
{
    private bool tocando;
    private Ficha ficha;

    // Use this for initialization
    void Start()
    {
        ficha = transform.parent.GetComponent<Ficha>();
        tocando = false;
    }

    public void ActivarDesactivar()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Conexion")
        {
            if (ficha.colocada)
            {
                if (ficha.fichaAdyacenteAux)
                    try
                    {
                        if ((ficha.terceraFicha.name == "Dummy") || (ficha.terceraFicha == null))
                            ficha.terceraFicha = other.transform.parent.gameObject;
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
            }
            else
                ficha.activarDesactivarConexiones(name);

            if (ficha.fichaAdyacente == null)
                ficha.CambiarConexion(true, int.Parse(name.Substring(name.Length - 1, 1)), int.Parse(other.gameObject.name.Substring(name.Length - 1, 1)), other.transform.parent.gameObject);
            else
                ficha.fichaAdyacenteAux = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Conexion")
        {
            
            if(ficha.colocada)
            {
                if (ficha.terceraFicha.name == other.transform.parent.gameObject.name)
                    ficha.terceraFicha = GameObject.Find("Dummy");
            }
            else
                ficha.ResetearFicha();
                
            if (!ficha.colocada)
                ficha.activarDesactivarConexiones(name);
        }
    }
}
