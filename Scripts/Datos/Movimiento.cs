//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;

public class Movimiento
{
    public string ficha { get; set; }
    public int grados { get; set; }
    public string jugador { get; set; }
    public int posicion { get; set; }
    public int x { get; set; }
    public int y { get; set; }


    public Movimiento(string ficha, int grados, string jugador, int posicion, int x, int y)
    {
        this.ficha = ficha;
        this.grados = grados;
        this.jugador = jugador;
        this.posicion = posicion;
        this.x = x;
        this.y = y;
    }
}
