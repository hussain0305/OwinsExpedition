using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldElementMarker : MonoBehaviour
{
    private string OriginalSortingLayerName;
    private int OriginalSortingLayerOrder;

    // Start is called before the first frame update
    void Awake()
    {
        OriginalSortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
        OriginalSortingLayerOrder = GetComponent<SpriteRenderer>().sortingOrder;
    }

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().sortingLayerName = OriginalSortingLayerName;
        GetComponent<SpriteRenderer>().sortingOrder = OriginalSortingLayerOrder;
    }

    public string GetWorldElementName()
    {
        return this.gameObject.name;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<WorldElementMarker>() && GetWorldElementName().Contains("Icicle"))
        {
            if(Vector2.Distance(this.gameObject.transform.position, collision.gameObject.transform.position) > 0.4f && Vector2.Distance(this.gameObject.transform.position, collision.gameObject.transform.position) < 1.25f && this.gameObject.transform.position.y > collision.gameObject.transform.position.y)
            {
                collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
            }
        }
    }
}
