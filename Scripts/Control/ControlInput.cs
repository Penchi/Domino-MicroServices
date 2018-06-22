//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System;

public class ControlInput : MonoBehaviour
{
    private Ficha scriptFicha;
    private GameObject ficha;
    private Vector3 posInicialFicha; //La posición inicial de la ficha que se seleccionó
    private Vector3 curPosition;
    private RaycastHit2D hit;
    private int grados;

    private ControlREST cr;
    private ControlUI cu;

    private ConexionFicha cf;
    private bool girada;

    // Use this for initialization
    void Start()
    {
        cr = GetComponent<ControlREST>();
        cu = GetComponent<ControlUI>();
        grados = 0;
        girada = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Constantes.Instance.miTurno)
        {
                #if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    if ((ficha != null) && (Input.GetMouseButton(0)))
                    {
                        curPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        ficha.transform.position = new Vector2(curPosition.x, curPosition.y);
                    }
                #elif UNITY_ANDROID
                    if ((ficha != null) && (Input.touchCount == 1))
                    {
                        curPosition = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
                        ficha.transform.position = new Vector2(curPosition.x, curPosition.y);
                    }
                #endif
        

            if (Input.GetMouseButtonDown(0))
            {
                hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

                if (hit.collider != null)
                {
                    if (hit.transform.tag == "Ficha") //Tomamos una ficha
                    {
                        SoltarFicha();
                        TomarFicha();
                    }
                }
            }

            if(Input.GetMouseButtonUp(0))
            {
                if (ficha != null) //Si no tenemos una ficha, no hay nada que hacer
                {
                    if (scriptFicha.fichaAdyacente != null) //Si tenemos una ficha pegada intentamos colocarla
                        ColocarFicha();
                    else
                        SoltarFicha();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (ficha != null)
                {
                    grados += 90;
                    if (grados == 360)
                        grados = 0;
                    ficha.transform.eulerAngles = new Vector3(ficha.transform.eulerAngles.x, ficha.transform.eulerAngles.y, grados);
                }
            }

            if((Input.touchCount == 2) && (girada))
            {
                if (ficha != null)
                {
                    girada = true;
                    grados += 90;
                    if (grados == 360)
                        grados = 0;
                    ficha.transform.eulerAngles = new Vector3(ficha.transform.eulerAngles.x, ficha.transform.eulerAngles.y, grados);
                }
            }
            if (Input.touchCount == 1)
                girada = false;
        }
    }

    public void TomarFicha()
    {
        ficha = hit.transform.gameObject;
        posInicialFicha = ficha.transform.position;
        ficha.GetComponent<BoxCollider2D>().enabled = false;
        grados = 0;
        scriptFicha = ficha.GetComponent<Ficha>();
        scriptFicha.activarDesactivarConexiones("null");
        scriptFicha.ResetearFicha();
    }

    public void SoltarFicha()
    {
        if(ficha != null)
        {
            scriptFicha.StartCoroutine("Devolver", posInicialFicha);
            ficha.transform.eulerAngles = Vector3.zero;
            ficha = null;
            scriptFicha = null;
            grados = 0;
            cu.setMensaje("Ficha colocada incorrectamente");
        }
    }

    public void ColocarFicha()
    {
            //if ((scriptFicha.fichaAdyacente.GetComponent<Ficha>().fichaAdyacente) || (scriptFicha.fichaAdyacente.GetComponent<Ficha>().terceraFicha.name == ficha.name) || (scriptFicha.fichaAdyacente.name == "null"))
            {
                Movimiento[] movimiento = new Movimiento[2];

                if (scriptFicha.fichaAdyacente.transform.name == "null")
                    movimiento[0] = new Movimiento("null", 0, "null", 0, 0, 0);
                else
                    movimiento[0] = new Movimiento(scriptFicha.fichaAdyacente.transform.name, (int)scriptFicha.fichaAdyacente.transform.eulerAngles.z, "null", scriptFicha.posConexionAdyacente, (int)scriptFicha.fichaAdyacente.transform.position.x, (int)scriptFicha.fichaAdyacente.transform.position.y);

                movimiento[1] = new Movimiento(ficha.transform.name, grados, Constantes.Instance.jugador, scriptFicha.posConexion, (int)ficha.transform.position.x, (int)ficha.transform.position.y);
                Debug.Log("Ficha a precolocalada:" + movimiento[0].ficha + " " + movimiento[0].grados + " " + movimiento[0].jugador + " " + movimiento[0].posicion + " " + movimiento[0].x + " " + movimiento[0].y);
                Debug.Log("Ficha a colocar:" + movimiento[1].ficha + " " + movimiento[1].grados + " " + movimiento[1].jugador + " " + movimiento[1].posicion + " " + movimiento[0].x + " " + movimiento[0].y);
                //string ficha, int grados, string jugador, int posicion, float x, float y

                SoltarFicha();
                cr.EnviarMovimiento(movimiento);
            }
    }

    public void PasarTurno()
    {
        if(Constantes.Instance.miTurno)
        {
            Movimiento[] movimiento = new Movimiento[2];

            movimiento[0] = new Movimiento("null", 0, Constantes.Instance.jugador, 0, 0, 0);
            movimiento[1] = new Movimiento("null", 0, Constantes.Instance.jugador, 0, 0, 0);

            Debug.Log("Ficha a precolocalada:" + movimiento[0].ficha + " " + movimiento[0].grados + " " + movimiento[0].jugador + " " + movimiento[0].posicion + " " + movimiento[0].x + " " + movimiento[0].y);
            Debug.Log("Ficha a colocar:" + movimiento[1].ficha + " " + movimiento[1].grados + " " + movimiento[1].jugador + " " + movimiento[1].posicion + " " + movimiento[0].x + " " + movimiento[0].y);

            cr.EnviarMovimiento(movimiento);
        }
    }
}
