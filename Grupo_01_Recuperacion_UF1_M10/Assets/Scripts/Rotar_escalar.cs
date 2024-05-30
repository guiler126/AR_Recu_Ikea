using UnityEngine.UI;
using UnityEngine;


public class Rotar_escalar : MonoBehaviour

{ [SerializeField] private Slider _sliderRotate;
[SerializeField] private Slider _sliderScale;

[SerializeField] private GameObject _gameObject;


void Start()
{
    _sliderRotate.onValueChanged.AddListener(delegate {Rotate();});
    _sliderScale.onValueChanged.AddListener(delegate {Scale();});
}

private void Update()
{
    _gameObject = ARInteractionsManager.instance.itemObjeto_selected;
}

private void Rotate()
{
    _gameObject.transform.rotation = Quaternion.Euler(_gameObject.transform.localRotation.x, _sliderScale.value, _gameObject.transform.localRotation.z);
}

private void Scale()
{
    _gameObject.transform.localScale = new Vector3(_sliderScale.value, _sliderScale.value, _sliderScale.value);
}
}

