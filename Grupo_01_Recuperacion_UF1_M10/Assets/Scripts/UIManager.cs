using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private List<Item> items_objetos = new List<Item>();
    [SerializeField] private GameObject content_lista_Objetos;
    [SerializeField] private Item_objeto_btn _itemObjetoBtn;
    [SerializeField] private GameObject Mostrador_de_objetos;
    [SerializeField] private GameObject Mostrador_de_funciones;
    [SerializeField] private GameObject slider_escalar;
    [SerializeField] private GameObject slider_rotar;
    
    
    void Start()
    {
        Crear_lista_objetos();
    }


    public void Crear_lista_objetos()
    {
        Limpiar_lista_objetos();
        
        foreach (var item in items_objetos)
        {
            Item_objeto_btn itemObjetoBtn;
            itemObjetoBtn = Instantiate(_itemObjetoBtn, content_lista_Objetos.transform);
            itemObjetoBtn.ItemName = item.ItemName;
            itemObjetoBtn.ItemImage = item.ItemImage;
            itemObjetoBtn.ItemPrefab = item.ItemPrefab;
            itemObjetoBtn.name = item.name;
        }
    }

    public void Limpiar_lista_objetos()
    {
        foreach (Transform child in content_lista_Objetos.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void activarAPP()
    {
        Mostrador_de_funciones.active = !Mostrador_de_funciones.active;
        Mostrador_de_objetos.active = !Mostrador_de_objetos.active;
        slider_escalar.active = !slider_escalar.active;
        slider_rotar.active = !slider_rotar.active;
    }
}
