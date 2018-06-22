//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerTablero
{
    private List<Ficha> listaFichas;

    public ManagerTablero()
    {
        listaFichas = new List<Ficha>();
    }

	public void AgregarFicha(Ficha ficha)
    {
        listaFichas.Add(ficha);
    }

    public int getExtremos()
    {
        int n = 0;

        for (int i = 0; i < listaFichas.Count; i++)
        {
            //if ((listaFichas[i].terceraFicha.name == "Dummy") || (listaFichas[i].terceraFicha == null)) //Si no tiene 2da ficha conectada
            if(!listaFichas[i].fichaAdyacenteAux)
            {
                int pos = listaFichas[i].posConexion; //tomamos la posición de la que no estamos conectados

                if ((pos == 1) || (pos == 2) || (pos == 8))
                    n = int.Parse(listaFichas[i].gameObject.name.Substring(2, 1));
                else
                {
                    if ((pos == 4) || (pos == 5) || (pos == 6))
                        n = int.Parse(listaFichas[i].gameObject.name.Substring(0, 1));
                    else
                        n = int.Parse(listaFichas[i].gameObject.name.Substring(0, 1)) + int.Parse(listaFichas[i].gameObject.name.Substring(2, 1));
                }
            }
        }
        return n;
    }
}
