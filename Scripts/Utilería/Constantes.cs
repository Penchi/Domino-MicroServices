//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;

public class Constantes
{
    public string ipLocal { get; set; }
    public string ipServidor { get; set; }
    public bool conectado { get; set; }
    public bool preparado { get; set; }
    public bool comenzado { get; set; }
    public string path { get; set; }
    public string jugador { get; set; }
    public bool miTurno { get; set; }
    public ManagerTablero tablero;

    private static Constantes instance;

    private Constantes()
    {
        miTurno = false;
        ipLocal = Network.player.ipAddress;
        ipServidor = "127.0.0.1";
        conectado = false;
        preparado = false;
        comenzado = false;
        jugador = "J0";

        path = Application.persistentDataPath + "/";
        //path = Application.dataPath + "/";
        tablero = new ManagerTablero();
        Debug.Log(path);
    }

    public static Constantes Instance
    {
        get
        {
            if (instance == null)
                instance = new Constantes();
            return instance;
        }
    }
}
