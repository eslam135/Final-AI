using UnityEngine;

public class HandleBookHeadDeath : MonoBehaviour
{
    private void HandleDeath()
    {
        gameObject.GetComponent<Animator>().Play("Death");
    }
    public void setFalse()
    {
        Debug.Log("Setting BookHead parent to false",transform.parent.gameObject);
        transform.parent.gameObject.SetActive(false);
    }
}
