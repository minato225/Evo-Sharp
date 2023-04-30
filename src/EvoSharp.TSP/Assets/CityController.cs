using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class CityController : MonoBehaviour
{
    private Vector3 screenPoint;
    private Vector3 offset;

    public TspCity Data { get; set; }

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(CurrentSceenPOint);
        transform.localScale *= 2f;
    }

    void OnMouseUp() => transform.localScale /= 2f;

    void OnMouseDrag()
    {
        var curScreenPoint = CurrentSceenPOint;
        var curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;

        Data.Position = transform.position = curPosition;
    }

    void Update() => transform.position = Data.Position;

    Vector3 CurrentSceenPOint => new(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
}