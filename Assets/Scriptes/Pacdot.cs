using UnityEngine;

public class Pacdot : MonoBehaviour
{
    public bool isSuperpacdot = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name=="Pacman")
        {
            if (isSuperpacdot)
            {
                GameManager.Instance.OnEatSuperPacdots();
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
            else
            {
                GameManager.Instance.OnEatPacdot(gameObject);
                Destroy(gameObject);
            }
            
        }
    }
}
