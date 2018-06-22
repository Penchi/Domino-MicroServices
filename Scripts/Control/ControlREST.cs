//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System.Threading;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System;

public class ControlREST : MonoBehaviour
{
    public static string urlNotificacion = null;

    private string host = "http://"; //localhost:8081/";
    private string servidor;
    /*
    private string urlNotiFichas = host + "Fichas"; //Notifica que se asignaron las fichas, llamamos a RecuperarFichas
    private string urlNotiTurno = host + "Turno"; //Notifica que se asignaron los turnos, llamamos a RecuperarJugadores
    private string urlNotiMovimiento; //Notifica que hubo cambios en el tablero, llamamos a RecuperarJugadores y RecuperarTablero
    */
    private string urlConectarAServidor; //Notificamos al servidor que queremos unirnos, recibimos respuesta de si hubo exito
    private string urlPreparado; //Informamos que estamos listos para empezar la partida, recibimos respuesta de si hubo exito
    private string urlVerificarMovimiento;
    private string urlCalcularPuntos;
    private string urlComenzarPartida; //Pedimos al servidor que empiece la partida
    private string urlAsignarTurnos;
    private string urlRegistrarMovimiento; //Manda la ficha que se planea jugar y sobre donde se quiere jugar
    private string urlRecuperarFichas; //Recupera un JSON con las fichas
    private string urlRecuperarJugadores; //Turnos de jugadores, puntuaciones de jugadores y número de fichas
    private string urlRecuperarMovimiento; //Recupera el tablero de juego.
    private string urlReiniciar;

    private LectorJSON lectorJSON;
    private WebClient client;

    private ControlUI cu;

    private string respuesta;

    void Awake()
    {
        Debug.Log(Constantes.Instance.path);
        Debug.Log(Constantes.Instance.ipLocal);
        host += Constantes.Instance.ipLocal + ":8081/";
        Debug.Log("host:" + host);
    }

    void Start()
    {
        cu = GetComponent<ControlUI>();
        client = new WebClient();

        lectorJSON = new LectorJSON();

        ReiniciarListener();
        respuesta = "";
        //servidor = "http://localhost:8080/domino/";
        //DefinirUrls();
    }

    void Update()
    {
        if(urlNotificacion != null)
        {
            string url = urlNotificacion;
            urlNotificacion = null;
            Notificacion(url);
        }
    }

    public void DefinirUrls()
    {
        urlPreparado = servidor + "preparado/" + (!Constantes.Instance.preparado).ToString().ToUpper() + "/jugador/" + Constantes.Instance.ipLocal + "/"; //Informamos que estamos listos para empezar la partida, recibimos respuesta de si hubo exito
        urlComenzarPartida = servidor + "iniciar/TRUE/"; //Pedimos al servidor que empiece la partida
        urlVerificarMovimiento = servidor + "verificarMov/";
        urlRegistrarMovimiento = servidor + "registrarMov"; //Manda la ficha que se planea jugar y sobre donde se quiere jugar
        urlCalcularPuntos = servidor + "calcular/";
        urlAsignarTurnos = servidor + "asignarTurnos";
        urlRecuperarFichas = servidor + "recuperarF"; //Recupera un JSON con las fichas
        urlRecuperarJugadores = servidor + "recuperarJ"; //Turnos de jugadores, puntuaciones de jugadores y número de fichas
        urlRecuperarMovimiento = servidor + "recuperarM";
        urlReiniciar = servidor + "limpiar/";
        /*
        Debug.Log(urlConectarAServidor);
        Debug.Log(urlPreparado);
        Debug.Log(urlComenzarPartida);
        Debug.Log(urlRegistrarMovimiento);
        Debug.Log(urlRecuperarFichas);
        Debug.Log(urlRecuperarJugadores);
        Debug.Log(urlRecuperarMovimiento);
        */
    }

    public void ConectarServidor()
    {
        if (!Constantes.Instance.conectado)
        {
            client = new WebClient();
            respuesta = "";

            string ipServidorTemporal = GameObject.Find("TxtIpServidor").GetComponent<Text>().text;

            ///////////////////////////////////
            //ipServidorTemporal = "192.168.1.108";


            cu.setMensaje("Conectando a servidor con ip:" + ipServidorTemporal);
            servidor = "http://" + ipServidorTemporal + ":8080/domino/";
            Constantes.Instance.ipServidor = servidor;
            urlConectarAServidor = servidor + "conectar/" + Constantes.Instance.ipLocal + "/";

            try
            {
                respuesta = client.DownloadString(urlConectarAServidor);

                if (respuesta == "Conexion exitosa")
                {
                    Constantes.Instance.conectado = true;
                    cu.setMensaje(respuesta);
                    DefinirUrls();
                }
            }
            catch (Exception ex)
            {
                cu.setMensaje("Ocurrió un error al conectarse al servidor");
                Console.WriteLine(ex.Message);
            }
        }
    }

    public void Preparado()
    {
        if (!Constantes.Instance.preparado)
        {
            client = new WebClient();
            respuesta = "";
            cu.setMensaje("Enviando estado a servidor");

            try
            {
                respuesta = client.DownloadString(urlPreparado);
                cu.setMensaje(respuesta);
                Constantes.Instance.preparado = !Constantes.Instance.preparado;
            }
            catch (Exception ex)
            {
                cu.setMensaje("Ocurrió un error al mandar estado");
                Console.WriteLine(ex.Message);
            }
        }
    }

    public void ComenzarJuego()
    {
        client = new WebClient();
        respuesta = "";
        cu.setMensaje("Enviando petición para comenzar la partida");
        try
        {
            respuesta = client.DownloadString(urlComenzarPartida);
            cu.setMensaje(respuesta);
        }
        catch(Exception ex)
        {
            cu.setMensaje("Ocurrió un error al comenzar la partida");
            Console.WriteLine(ex.Message);
        }
    }

    public void EnviarMovimiento(Movimiento[] movimiento)
    {
        client = new WebClient();
        // /verificarMov/Precolocada/{ficha1}/{grados1}/{jugador1}/{x1}/{y1}/AColocar/{ficha2}/{grados2}/{jugador2}/{x2}/{y2}/
        cu.setMensaje("Enviando movimiento:");

        urlVerificarMovimiento = servidor + "verificarMov/";
        urlCalcularPuntos = servidor + "calcular/";

        urlVerificarMovimiento += "Precolocada/" + movimiento[0].ficha + "/" + movimiento[0].grados + "/" + Constantes.Instance.jugador + "/" + movimiento[0].posicion + "/" + movimiento[0].x + "/" + movimiento[0].y;
        urlVerificarMovimiento += "/AColocar/" + movimiento[1].ficha + "/" + movimiento[1].grados + "/" + movimiento[1].jugador + "/" + movimiento[0].posicion + "/" + + movimiento[1].x + "/" + movimiento[1].y + "/";

        Debug.Log("Movimiento:" + urlVerificarMovimiento);

        try
        {
            respuesta = client.DownloadString(urlVerificarMovimiento);
            cu.setMensaje(respuesta);
        }
        catch(Exception ex)
        {
            cu.setMensaje("Ocurrió un error al enviar el movimiento");
            Console.WriteLine(ex.Message);
            return; //terminamos porque no pudimos enviar el movimiento
        }

        if (respuesta == "valido")
        {
            cu.setMensaje("Movimiento no permitido");
            client = new WebClient();
            //ReBuildeamos las urls porque cambiaron
            int pos = movimiento[1].posicion;
            int i = 0;

            if ((pos == 1) || (pos == 2) || (pos == 8))
                i = int.Parse(movimiento[1].ficha.Substring(2, 1));
            else
            {
                if ((pos == 4) || (pos == 5) || (pos == 6))
                    i = int.Parse(movimiento[1].ficha.Substring(0, 1));
                else
                    i = int.Parse(movimiento[1].ficha.Substring(0, 1)) + int.Parse(movimiento[1].ficha.Substring(2, 1));
            }

            int extremo = 0;
            try
            {
                 extremo = Constantes.Instance.tablero.getExtremos();
            }
            catch(Exception ex)
            {

            }
            urlCalcularPuntos += extremo + "/" + i + "/" + Constantes.Instance.ipLocal + "/";
            Debug.Log("Extremo de tablero:" + Constantes.Instance.tablero.getExtremos() + " extremo a colocar:" + i);
            Debug.Log("Url calcular puntos:" + urlCalcularPuntos);

            try
            {
                client.DownloadString(urlCalcularPuntos);
            }
            catch(Exception ex)
            {
                cu.setMensaje("Ocurrió un error al calcular el puntaje");
                Console.WriteLine(ex.Message);
            }

            client = new WebClient();
            try
            {
                client.DownloadString(urlRegistrarMovimiento);
            }
            catch (Exception ex)
            {
                cu.setMensaje("Ocurrió un error al registrar el movimiento");
                Console.WriteLine(ex.Message);
            }
        }
        else
            if(respuesta == "paso")
            {
                cu.setMensaje("Turno cedido");
                client = new WebClient();
                try
                {
                    client.DownloadString(urlRegistrarMovimiento);
                }
                catch (Exception ex)
                {
                    cu.setMensaje("Ocurrió un error al registrar el movimiento");
                    Console.WriteLine(ex.Message);
                }
        }
    }

    public void EjecutarNotificador(string url)
    {
        Notificacion(url);
    }

    public void Notificacion(string url)
    {
        url = url.Substring(1, url.Length - 1);

        int n = url.IndexOf("/");
        
        string metodo = url.Substring(0, n);
        string valor = url.Substring(n + 1, url.Length - n - 1);

        string s;
        
        client = new WebClient();
        switch (metodo)
        {
            case "Fichas":
                s = client.DownloadString(urlRecuperarJugadores);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Jugadores.txt");
                cu.ActualizarJugadores();
                
                s = client.DownloadString(urlRecuperarFichas);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Fichas.txt");
                cu.CrearFichas();
                cu.DesactivarControles();
                break;

            case "Turno":
                s = client.DownloadString(urlRecuperarMovimiento);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Movimiento.txt");
                cu.ColocarFicha();
                
                s = client.DownloadString(urlRecuperarJugadores);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Jugadores.txt");
                cu.ActualizarJugadores();
                
                cu.CambioTurno(valor);
                break;

            case "Ganador":
                s = client.DownloadString(urlRecuperarMovimiento);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Movimiento.txt");
                cu.ColocarFicha();

                s = client.DownloadString(urlRecuperarJugadores);
                lectorJSON.ConvertirJSON(s);
                lectorJSON.GuardarJSON("Jugadores.txt");
                cu.ActualizarJugadores();

                cu.MostrarGanador(valor);
                break;

            default:
                Debug.Log("Método desconocido, ¿url mal escrita?");
                break;
        }
        ReiniciarListener();
    }

    public void Reiniciar()
    {
        client = new WebClient();
        try
        {
            client.DownloadString(urlReiniciar);
        }
        catch (Exception ex)
        {
            cu.setMensaje("Ocurrió un error al reiniciar");
            Console.WriteLine(ex.Message);
        }
        Application.LoadLevel(0);
    }

    public void ReiniciarListener()
    {
        Listener listenerNotMovimiento = new Listener(host);
        Thread oThread = new Thread(new ThreadStart(listenerNotMovimiento.PrepararListener));
        
        oThread.Start();
        Thread.Sleep(1);
    }
}