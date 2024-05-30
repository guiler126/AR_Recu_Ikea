using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionsManager : MonoBehaviour
{
    public static ARInteractionsManager instance;
    
    [SerializeField, Tooltip("Camera de AR que se encuentra dentro de AR Session Origin")] 
    private Camera arCamera;
    
    [SerializeField, Tooltip("Componente que se tiene el AR Session Origin, en caso de no existir" +
                             " se tiene que añadir")] 
    private ARRaycastManager _arRaycastManager;

    [SerializeField, Tooltip("GameObject que respresenta el pointer que aparece debajo de los objetos que creamos")]
    private GameObject arPointer;
    
    
    // PRIVATE VARTIABLES
    [Tooltip("Lista donde guardaremos las pulsaciones del usuario")]
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [Tooltip("GameObject que representa el objeto que hemos creado")]
    private GameObject itemObjeto_prefab;

    [Tooltip("GameObject que representará la selección del objeto donde el usuario ha pulsado")]
    public GameObject itemObjeto_selected;

    private bool isInitialPosition;

    private bool isOverUI;

    private bool isOverItemObjeto_prefab;


    // GETTERS
    public ARInteractionsManager Instance => instance;
    
    public GameObject ARPointer => arPointer;

    public GameObject ItemObjetoPrefab
    {
        set => itemObjeto_prefab = value;
    }

    public bool IsInitialPosition
    {
        set => isInitialPosition = value;
    }


    // METHODS
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    void Update()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            _arRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes);

            // Comprobamos que existe un plano que se encuentra en la mitad de la pantalla
            if (hits.Count > 0)
            {
                // Si pulsamos sobre un plano moveremos este GameObjetct que tiene el script y como hijo el AR Pointer
                // a la posición donde se encuentra el plano
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;
                arPointer.SetActive(true);
                isInitialPosition = false;
            }
        }
        
        // Lógica para mover objetos
        // Verificamos que hemos pulsado la pantalla
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                var touchPosition = touch.position;
                isOverUI = IsTapOverUI(touchPosition);
                isOverItemObjeto_prefab = IsTapOverItemObjeto_prefab(touchPosition);
            }

            if (touch.phase == TouchPhase.Moved)
            {
                // Si movemos el dedo tenemos que verificar que estamos moviendo dentro de los planos detectados
                if (_arRaycastManager.Raycast(touch.position, hits, TrackableType.Planes))
                {
                    Pose hitPose = hits[0].pose;

                    if (!isOverUI && isOverItemObjeto_prefab)
                    {
                        transform.position = hitPose.position;
                    }
                }
            }

            if (isOverItemObjeto_prefab && itemObjeto_prefab == null && !isOverUI)
            {
                itemObjeto_prefab = itemObjeto_selected;
                itemObjeto_selected = null;
                arPointer.SetActive(true);
                transform.position = itemObjeto_prefab.transform.position;
                itemObjeto_prefab.transform.parent = arPointer.transform;

            }
        }
    }

    
    /// <summary>
    /// Método para comprobar si estamos pulsando sobre un elemento creado en pantalla
    /// </summary>
    private bool IsTapOverItemObjeto_prefab(Vector2 touchPosition)
    {
        // Creamos un rayo desde la camara al punto donde hemos pulsado
        Ray ray = arCamera.ScreenPointToRay(touchPosition);

        // Comprobamos si ha colisionado con algo
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // En caso de colisionar con un prefab instanciado con el tag Objeto, devolveremos true, de lo contrario false
            if (hit.collider.CompareTag("Objeto"))
            {
                // Dejamos guardado el objeto seleccionado
                itemObjeto_selected = hit.transform.gameObject;
                return true;
            }

        }

        return false;
    }


    /// <summary>
    /// Método para comprobar que donde estamos pulsando sobre un elemento de la UI
    /// </summary>
    private bool IsTapOverUI(Vector2 touchPosition)
    {
        // Assignamos el EventSystem de la interfaz de nuestra app al Pointer
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        
        // Asignamos la posición del TOUCH
        pointerEventData.position = new Vector2(touchPosition.x, touchPosition.y);

        // Verificamos si hay algún evento en la posición donde hemos pulsado
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, result);

        return result.Count > 0;
    }


    /// <summary>
    /// Métedo para dejar el objeto en el lugar donde se encuentra, de esta manera por si pulsamos en otro lugar de la
    /// pantalla no seguirá
    /// </summary>
    public void AssignItemPosition()
    {
        if (itemObjeto_prefab != null)
        {
            // Ya no hace falta que el objeto que hemos creado sea hijo del pointer por lo que le quitamos el parent
            itemObjeto_prefab.transform.parent = null;
            
            arPointer.SetActive(false);

            itemObjeto_prefab = null;
        }
    }

    /// <summary>
    /// Método para borrar el objeto que hemos seleccionado
    /// </summary>
    public void DeleteItem()
    {
        Destroy(itemObjeto_prefab);
        arPointer.SetActive(false);
    }
    
}
