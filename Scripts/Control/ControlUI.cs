//Código creado por Aarón Angulo

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ControlUI : MonoBehaviour
{
    private string path;

    private GameObject dominoHolder;
    private GameObject tablero;
    private GameObject panelScore;
    private GameObject panelConfig;
    private GameObject jugador;
    private Button btnConectar;
    private Text txtConectar;
    private Button btnPreparado;
    private Text txtPreparado;
    private Button btnComenzar;

    private Text TxtMensaje;
    private Text[] TxtFichas;
    private Text[] TxtPuntuaciones;
    private Text TxtBtnScore;
    private Text TxtBtnConfig;

    private LectorJSON json;

    void Awake ()
    {
        TxtMensaje = GameObject.Find("TxtMensaje").GetComponent<Text>();
        json = new LectorJSON();
        dominoHolder = GameObject.Find("DominoHolder");
        tablero = GameObject.Find("Tablero");
        panelScore = GameObject.Find("PanelScore");
        panelConfig = GameObject.Find("PanelConfig");
        TxtBtnScore = GameObject.Find("TxtBtnScore").GetComponent<Text>();
        TxtBtnConfig = GameObject.Find("TxtBtnConfig").GetComponent<Text>();
        BtnCerrarPanelScore();
        BtnCerrarPanelConfig();

        btnConectar = GameObject.Find("BtnConectar").GetComponent<Button>();
        txtConectar = GameObject.Find("TxtConectar").GetComponent<Text>();
        btnPreparado = GameObject.Find("BtnPreparado").GetComponent<Button>();
        txtPreparado = GameObject.Find("TxtPreparado").GetComponent<Text>();
        btnComenzar = GameObject.Find("BtnComenzar").GetComponent<Button>();
        
        ////////////////////////////
        /*
        ActualizarJugadores();
        CrearFichas();
        ColocarFicha();
        CambioTurno("J1");
        */
    }

    public void setMensaje(string mensaje)
    {
        TxtMensaje.text = mensaje;
    }

    public void CrearFichas()
    {
        json.LeerJSON("Fichas.txt");

        string ficha;
        float posicion;
        float espaciador;

        if (json.getNodos(0) == 4)
        {
            posicion = -22;
            espaciador = 7f / (json.getNodos(1) / 7);
        }
        else
        {
            posicion = -30;
            espaciador = 9f / (json.getNodos(1) / 7);
        }

        Color color = Color.white;

        switch (Constantes.Instance.jugador)
        {
            case "J1": color = Color.blue; break;
            case "J2": color = Color.red; break;
            case "J3": color = Color.yellow; break;
            case "J4": color = Color.green; break;
        }

        for (int i = 0; i < json.getNodos(1); i++)
        {
            ficha = json.ObtenerDato(Constantes.Instance.ipLocal, i);
            GameObject gbFicha = Resources.Load("Prefabs/Ficha", typeof(GameObject)) as GameObject;
            gbFicha.GetComponent<SpriteRenderer>().sprite = Resources.Load("Imagenes/Fichas/" + ficha, typeof(Sprite)) as Sprite;
            gbFicha.GetComponent<ParticleSystem>().startColor = color;
            gbFicha = Instantiate(gbFicha);
            gbFicha.GetComponent<SpriteRenderer>().color = color;
            gbFicha.transform.SetParent(dominoHolder.transform);
            gbFicha.transform.localPosition = new Vector3(posicion, 0, 0);
            gbFicha.transform.localScale = new Vector3(1.15f, 1.3f, 1);
            gbFicha.name = ficha;
            posicion += espaciador;
        }

        setMensaje("Se han repartido las fichas");
    }

    public void DefinirJugador()
    {
        jugador = Resources.Load("Prefabs/Jugador", typeof(GameObject)) as GameObject;
        jugador = Instantiate(jugador);
        for (int i = 0; i < json.getNodos(0); i++)
        {
            if (json.ObtenerDato(i, "ip") == Constantes.Instance.ipLocal)
            {
                Constantes.Instance.jugador = "J" + (i + 1);
                jugador.transform.SetParent(GameObject.Find("J" + (i + 1)).transform);
                jugador.transform.localPosition = new Vector3(-10, 20, 0);
                if (json.ObtenerDato(Constantes.Instance.jugador, "turno") == "1")
                    Constantes.Instance.miTurno = true;
                break;
            }
        }
        setMensaje("Eres el jugador " + Constantes.Instance.jugador.Substring(1, 1));
        
    }

    public void ActualizarJugadores()
    {
        json.LeerJSON("Jugadores.txt");

        if (jugador == null)
        {
            DefinirJugador();
        }

        if (TxtFichas == null)
        {
            TxtFichas = new Text[json.getNodos(0)];
            TxtPuntuaciones = new Text[json.getNodos(0)];

            for (int i = 0; i < TxtFichas.Length; i++)
            {
                TxtFichas[i] = GameObject.Find("TxtJ" + (i + 1) + "Fichas").GetComponent<Text>();
                TxtPuntuaciones[i] = GameObject.Find("TxtJ" + (i + 1) + "Puntuacion").GetComponent<Text>();
            }
        }

        for (int i = 0; i < TxtFichas.Length; i++)
        {
            TxtFichas[i].text = json.ObtenerDato("J" + (i + 1), "fichas");
            TxtPuntuaciones[i].text = json.ObtenerDato("J" + (i + 1), "puntuacion");
        }

        setMensaje("Se actualizó la información de los jugadores");
    }

    public void ColocarFicha()
    {
        json.LeerJSON("Movimiento.txt");
        Color color = Color.white;
        string ficha = json.ObtenerDato("AColocar", "ficha");

        if (ficha == "null") //El del ultimo movimiento pasó turno
            return;

        if (GameObject.Find(ficha) != null)
            Destroy(GameObject.Find(ficha));

        switch (json.ObtenerDato("AColocar", "jugador"))
        {
            case "J1": color = Color.blue; break;
            case "J2": color = Color.red; break;
            case "J3": color = Color.yellow; break;
            case "J4": color = Color.green; break;
        }

        GameObject gbFicha = Resources.Load("Prefabs/Ficha", typeof(GameObject)) as GameObject;
        gbFicha.GetComponent<SpriteRenderer>().sprite = Resources.Load("Imagenes/Fichas/" + ficha, typeof(Sprite)) as Sprite;
        gbFicha.GetComponent<ParticleSystem>().startColor = color;
        gbFicha = Instantiate(gbFicha);
        gbFicha.GetComponent<SpriteRenderer>().color = color;
        gbFicha.GetComponent<BoxCollider2D>().enabled = false;
        gbFicha.transform.SetParent(tablero.transform);
        gbFicha.transform.localScale = new Vector3(1.15f, 1.3f, 1);
        gbFicha.transform.eulerAngles = new Vector3(0, 0, float.Parse(json.ObtenerDato("AColocar", "gradosGirada")));
        gbFicha.name = ficha;

        if (json.ObtenerDato("Precolocada", "ficha") != "null")//si es una ficha de verdad
            gbFicha.transform.position = new Vector3(float.Parse(json.ObtenerDato("AColocar", "x")), float.Parse(json.ObtenerDato("AColocar", "y")), 0f);
        else//Si es una ficha de "mentira"
            gbFicha.transform.position = GameObject.Find("null").transform.position;

        setMensaje("El jugador " + json.ObtenerDato("AColocar", "jugador").Substring(1, 1) + " ha colocado una ficha");

        Ficha script = gbFicha.GetComponent<Ficha>();

        script.colocada = true;
        Constantes.Instance.tablero.AgregarFicha(script);
    }

    public void CambioTurno(string jugador)
    {
        if (Constantes.Instance.jugador == jugador)
        {
            Constantes.Instance.miTurno = true;
            setMensaje("Tu turno");
        }
        else
        {
            Constantes.Instance.miTurno = false;
            setMensaje("Turno del jugador " + jugador.Substring(1, 1));
        }
    }

    public void DesactivarControles()
    {
        btnComenzar.interactable = false;
        btnPreparado.interactable = false;
        btnComenzar.interactable = false;
    }

    public void MostrarGanador(string jugador)
    {
        if(jugador == "empate")
            setMensaje("Hubo un empate");
        else
            setMensaje("El jugador " + jugador + " ha ganado la partida.");
        Destroy(GameObject.Find("DominoHolder"));

        Constantes.Instance.miTurno = false;
    }

    public void BtnCerrarPanelScore()
    {
        if (panelScore.transform.localScale == Vector3.zero)
        {
            panelScore.transform.localScale = Vector3.one;
            TxtBtnScore.text = "Esconder score";
        }
        else
        {
            panelScore.transform.localScale = Vector3.zero;
            TxtBtnScore.text = "Mostrar score";
        }
    }

    public void BtnCerrarPanelConfig()
    {
        if (panelConfig.transform.localScale == Vector3.zero)
        {
            panelConfig.transform.localScale = Vector3.one;
            TxtBtnConfig.text = "Esconder opciones";
        }
        else
        {
            panelConfig.transform.localScale = Vector3.zero;
            TxtBtnConfig.text = "Mostrar opciones";
        }
    }
}
