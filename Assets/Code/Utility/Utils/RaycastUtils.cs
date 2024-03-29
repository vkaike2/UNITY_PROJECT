using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastUtils : MonoBehaviour
{
    #region GAME
    public static RaycastHit2D[] ObjectsUnderPosition(Vector3 position)
    {
        Ray2D ray = new Ray2D(position, Vector2.zero);
        return Physics2D.RaycastAll(ray.origin, ray.direction, 10f);
    }

    public static RaycastHit2D[] ObjectsUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics2D.GetRayIntersectionAll(ray);
    }

    public static List<T> GetComponentsUnderMouse<T>(List<Excluding> excluding = null) where T : MonoBehaviour
    {
        RaycastHit2D[] hits = ObjectsUnderMouse();
        return GetComponentsFromRayCastHits<T>(hits, excluding);
    }

    public static List<T> GetComponentsUnderPosition<T>(Vector3 position) where T : MonoBehaviour
    {
        RaycastHit2D[] hits = ObjectsUnderPosition(position);
        return GetComponentsFromRayCastHits<T>(hits);
    }

    private static List<T> GetComponentsFromRayCastHits<T>(RaycastHit2D[] hits, List<Excluding> excluding = null) where T : MonoBehaviour
    {
        excluding ??= new List<Excluding>();

        List<T> components = hits.Where(e => e.collider.GetComponent<T>() != null).Select(e => e.collider.GetComponent<T>()).ToList();
        if (components.Any()) return components;

        if (!excluding.Contains(Excluding.Parent))
        {
            components = hits.Where(e => e.collider.GetComponentInParent<T>() != null).Select(e => e.collider.GetComponentInParent<T>()).ToList();
            if (components.Any()) return components;
        }

        if (!excluding.Contains(Excluding.Children))
        {
            components = hits.Where(e => e.collider.GetComponentInChildren<T>() != null).Select(e => e.collider.GetComponentInChildren<T>()).ToList();
            return components;
        }

        return components;

    }
    #endregion

    #region UI
    public static bool HitSomethingUnderMouseUI(bool validateOnlyTagUI = false)
    {
        List<RaycastResult> hits = GetRayCastUnderPositionUI(Input.mousePosition);

        if (validateOnlyTagUI)
        {
            return hits.Any(e => e.gameObject.CompareTag("UI"));
        }
        return hits.Any();
    }

    public static List<T> GetComponentsUnderPositionUI<T>(Vector2 position, List<Excluding> excluding = null) where T : MonoBehaviour
    {
        List<RaycastResult> hits = GetRayCastUnderPositionUI(Camera.main.WorldToScreenPoint(position));


        return GetComponentsUnderRayCastUI<T>(hits, excluding);
    }

    public static List<T> GetComponentsUnderMouseUI<T>(List<Excluding> excluding = null) where T : MonoBehaviour
    {
        List<RaycastResult> hits = GetRayCastUnderPositionUI(Input.mousePosition);

        return GetComponentsUnderRayCastUI<T>(hits, excluding);
    }

    private static List<RaycastResult> GetRayCastUnderPositionUI(Vector2 position)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            pointerId = -1,
        };

        pointerData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        return results;
    }

    private static List<T> GetComponentsUnderRayCastUI<T>(List<RaycastResult> hits, List<Excluding> excluding = null) where T : MonoBehaviour
    {
        excluding ??= new List<Excluding>();

        List<T> components = hits.Where(e => e.gameObject.GetComponent<T>() != null).Select(e => e.gameObject.GetComponent<T>()).ToList();
        if (components.Any()) return components;

        if (!excluding.Contains(Excluding.Parent))
        {
            components = hits.Where(e => e.gameObject.GetComponentInParent<T>() != null).Select(e => e.gameObject.GetComponentInParent<T>()).ToList();
            if (components.Any()) return components;
        }

        if (!excluding.Contains(Excluding.Children))
        {
            components = hits.Where(e => e.gameObject.GetComponentInChildren<T>() != null).Select(e => e.gameObject.GetComponentInChildren<T>()).ToList();
            return components;
        }

        return components;
    }

    #endregion



    public enum Excluding
    {
        Parent,
        Children
    }
}
