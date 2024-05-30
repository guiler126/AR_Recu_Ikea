using UnityEngine.UI;
using UnityEngine;


public class Rotar_escalar : MonoBehaviour

{ [SerializeField] private Slider _sliderRotate;
[SerializeField] private Slider _sliderScale;

[SerializeField] private GameObject _gameObject;

private float y_RLimit = 180f;

private float s_Limit = 10f;

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
    _gameObject.transform.localEulerAngles = new Vector3(_sliderScale.value * y_RLimit, _gameObject.transform.localEulerAngles.y, _gameObject.transform.localEulerAngles.z);
}

private void Scale()
{
    _gameObject.transform.localScale = new Vector3(_sliderScale.value * s_Limit, _sliderScale.value * s_Limit, _sliderScale.value * s_Limit);
}
}

