using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    public GameObject TagToDelete;

    private Vector2 ClosingScales;
    private Vector2 BigScale;

    private Vector2 MidPosition;
    private Vector2 EndPosition;
    private GameObject MessengerParent;
    private SpriteRenderer MessengerBG;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().sortingLayerName = "Messages";
        GetComponent<MeshRenderer>().sortingOrder = 1;

        MessengerBG = GetComponentInParent<SpriteRenderer>();
    }

    public void SetPositionMarkers(Vector2 Mid, Vector2 End)
    {
        MidPosition = Mid;
        EndPosition = End;
        MessengerParent = transform.parent.gameObject;

        if(!MessengerParent)
            return;

        StartCoroutine(MessengerSidewaysMotion());
    }

    IEnumerator MessengerSidewaysMotion()
    {
        while (Vector2.Distance(MessengerParent.transform.position, MidPosition) > 0.1f)
        {
            MessengerParent.transform.position = Vector2.Lerp(MessengerParent.transform.position, MidPosition, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(2);

        while (Vector2.Distance(MessengerParent.transform.position, EndPosition) > 0.1f)
        {
            MessengerParent.transform.position = Vector2.Lerp(MessengerParent.transform.position, EndPosition, 10 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        //Destroy(MessengerParent);
        MessengerParent.gameObject.SetActive(false);
    }

    public void SetText(string Message)
    {
        GetComponent<TextMesh>().text = Message;
    }
}
