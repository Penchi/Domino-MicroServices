//Código creado por Aarón Angulo

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Clase que permite implementar las interfaces para arrastrar objetos en Unity.
/// </summary>
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject objetoArrastrado;

    private Vector3 posicionInicial;
    private Vector3 posicionCursor;
    private RaycastHit2D hit;

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Iniciando a arrastrar OnBeginDrag");
        //GetComponentInParent<GridLayoutGroup>().enabled = false;
        objetoArrastrado = hit.transform.gameObject;
        posicionInicial = Camera.main.ScreenToWorldPoint(transform.position);
        posicionInicial = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Arrastrando OnDrag");
        posicionCursor = Input.mousePosition;
        posicionCursor.z = 1f;
        transform.position = Camera.main.ScreenToWorldPoint(posicionCursor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("Terminando de arrastrar OnEndDrag");
        //GetComponentInParent<GridLayoutGroup>().enabled = true;
        objetoArrastrado = null;
        transform.position = posicionInicial;
        transform.position = Camera.main.ScreenToWorldPoint(posicionInicial);
    }
}
