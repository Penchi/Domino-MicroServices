//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public class Listener
{
    public string url;

    public Listener(string url)
    {
        this.url = url;
    }

    public void PrepararListener()
    {
        // Crear listener
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add(url);
        listener.Start();
        // GetContext nos freezea el programa si no está dentro de un hilo
        HttpListenerContext context = listener.GetContext();
        HttpListenerRequest request = context.Request;
        // Obtenemos la respuesta de la pagina que solicitamos
        HttpListenerResponse response = context.Response;
        // Construimos la respuesta
        string responseString = "<HTML><BODY> Recibido </BODY></HTML>";
        byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
        // Escribimos la respuesta en un buffer
        response.ContentLength64 = buffer.Length;
        Stream output = response.OutputStream;
        output.Write(buffer, 0, buffer.Length);
        // cerramos el buffer
        output.Close();
        listener.Stop();
        
        ControlREST.urlNotificacion = request.RawUrl.ToString();
    }


}
