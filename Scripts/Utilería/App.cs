//Código creado por Aarón Angulo

using UnityEngine;
using System.Collections;
using System.Net;
using System;

public class App : MonoBehaviour
{
    private AudioSource audio;
    private GameObject fondo;
    private SpriteRenderer spriteFondo;

	// Use this for initialization
	void Start ()
    {
        audio = GameObject.Find("Camara").GetComponent<AudioSource>();
        fondo = GameObject.Find("Fondo");
        spriteFondo = fondo.GetComponent<SpriteRenderer>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (audio.isPlaying)
                audio.Stop();
            else
                audio.Play();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            fondo.transform.localScale = new Vector3(16, 14, 14);
            spriteFondo.sprite = Resources.Load("Imagenes/Fondos/X-MEN", typeof(Sprite)) as Sprite;
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            fondo.transform.localScale = new Vector3(22, 20, 14);
            spriteFondo.sprite = Resources.Load("Imagenes/Fondos/Nuclear", typeof(Sprite)) as Sprite;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            fondo.transform.localScale = new Vector3(50, 32, 14);
            spriteFondo.sprite = Resources.Load("Imagenes/Fondos/Default", typeof(Sprite)) as Sprite;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            fondo.transform.localScale = new Vector3(25, 16, 14);
            spriteFondo.sprite = Resources.Load("Imagenes/Fondos/Sicario", typeof(Sprite)) as Sprite;
        }
    }
}
