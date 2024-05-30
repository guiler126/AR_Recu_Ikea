using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item_objeto_btn : MonoBehaviour
{
    public string ItemName;
    public Sprite ItemImage;
    public GameObject ItemPrefab;
    
    void Start()
    {
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ItemName;
        transform.GetChild(1).GetComponent<Image>().sprite = ItemImage;

        // Añadimos un listener para cuando pulsemos el elemento de la UI que tiene un componente button
        var button = GetComponent<Button>();
        button.onClick.AddListener(CreateItem);
    }

    
    /// <summary>
    /// Método para crear un GameObject cuando pulsamos en la lista de objetos (muebles) de la UI
    /// </summary>
    private void CreateItem()
    {
        ARInteractionsManager.instance.DeleteItem();
        // Creamos un gameObject donde lo pondremos como hijo del ARPointer de la clase ARInteractionsManager
        GameObject itemToInstantiate = Instantiate(ItemPrefab);
        itemToInstantiate.transform.position = ARInteractionsManager.instance.ARPointer.transform.position;
        itemToInstantiate.transform.parent = ARInteractionsManager.instance.ARPointer.transform;
        
        // Asignamos como valor de la variable ItemObjetoPrefab el gameObject que acabamos de crear
        ARInteractionsManager.instance.ItemObjetoPrefab = itemToInstantiate;

        ARInteractionsManager.instance.IsInitialPosition = true;



    }
}
