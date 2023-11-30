using Unity.VisualScripting;
using UnityEngine;

public class FullScreenDetector : MonoBehaviour
{

    //private Canvas _canvas;

    //private InputManager _inputManager;
    //private float _targetAspectRatio = 16f / 9f;
    //private Camera _mainCamera;
    

    //private void Start()
    //{
    //    _canvas = GetComponent<Canvas>();

    //    _inputManager = FindObjectOfType<InputManager>();
        
    //    _mainCamera = Camera.main;

        //_canvas.worldCamera = _mainCamera;
        //_canvas.sortingLayerName = "Canvas";
    //}

    //private void FixedUpdate()
    //{
        //// determine the game window's current aspect ratio
        //float windowAspect = (float)Screen.width / (float)Screen.height;

        //// current viewport height should be scaled by this amount
        //float scaleHeight = windowAspect / _targetAspectRatio;

        //// if scaled height is less than current height, add letterbox
        //if (scaleHeight < 1.0f)
        //{
        //    Rect rect = _mainCamera.rect;

        //    rect.width = 1.0f;
        //    rect.height = scaleHeight;
        //    rect.x = 0;
        //    rect.y = (1.0f - scaleHeight) / 2.0f;

        //    _mainCamera.rect = rect;
        //    Debug.Log("if");
        //}
        //else // add pillarbox
        //{
        //    Debug.Log("else");
            
        //    float scaleWidth = 1.0f / scaleHeight;

        //    Rect rect = _mainCamera.rect;

        //    rect.width = scaleWidth;
        //    rect.height = 1.0f;
        //    rect.x = (1.0f - scaleWidth) / 2.0f;
        //    rect.y = 0;

        //    _mainCamera.rect = rect;
        //}
    //}
}
