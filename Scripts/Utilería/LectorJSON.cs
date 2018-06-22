//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System.IO;
using LitJson;

public class LectorJSON
{
    private JsonData jd;
    
    /**
     * Se carga el archivo json para su posterior uso
     * Dependiendo de si se está usando el editor o Android se desarrolla el path
     */
    public LectorJSON()
    {
        jd = new JsonData();
    }

    public void ConvertirJSON(string json)
    {
        jd = JsonMapper.ToObject(json);
    }

    public void LeerJSON(string nombre)
    {
        jd = JsonMapper.ToObject(File.ReadAllText(Constantes.Instance.path + nombre));
    }

    public string ObtenerDato(string nodo, string variable)
    {
        return jd[nodo][variable].ToString();
    }

    public string ObtenerDato(string nodo, int indice)
    {
        return jd[nodo][indice].ToString();
    }

    public string ObtenerDato(int nodo, string indice)
    {
        return jd[nodo][indice].ToString();
    }

    public int getNodos(int profundidad)
    {
        switch(profundidad)
        {
            case 0: return jd.Count;
            case 1: return jd[0].Count;
            default: return 0;
        }
    }

    public void GuardarJSON(string nombre)
    {
        File.WriteAllText(Constantes.Instance.path + nombre, jd.ToJson().ToString());
    }
}
