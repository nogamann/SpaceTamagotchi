   
using UnityEngine;
using System.Collections;

public class DragObject : MonoBehaviour
{

    [SerializeField]
    float _horizontallimit = 2.5f, _verticallimit = -2.5f, dragSpeed = 0.1f;

    Transform cachedTransform;
    Vector3 startingPos;


    // Use this for initialization
    void Start()
    {
        //referencia transform
        cachedTransform = transform;

        //salva a posicao inicial
        startingPos = cachedTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Vector2 deltaPosition = Input.GetTouch(0).deltaPosition;
            switch (Input.GetTouch(0).phase)
            {
                case TouchPhase.Began:
                    break;

                case TouchPhase.Moved:
                    DragObj(deltaPosition);
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
    }

    void DragObj(Vector2 deltaPos)
    {
        cachedTransform.position = new Vector3(Mathf.Clamp((deltaPos.x * dragSpeed) + cachedTransform.position.x,
                                                           startingPos.x - _horizontallimit, startingPos.x + _horizontallimit),
                                                           Mathf.Clamp((deltaPos.y * dragSpeed) + cachedTransform.position.y,
                                                           startingPos.y - _verticallimit, startingPos.y + _verticallimit),
                                                           cachedTransform.position.z);
    }
}