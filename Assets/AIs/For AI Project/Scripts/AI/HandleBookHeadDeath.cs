using UnityEngine;

public class HandleBookHeadDeath : MonoBehaviour
{
    public void setFalse()
    {
        Debug.Log("Setting BookHead parent to false",transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }
}
