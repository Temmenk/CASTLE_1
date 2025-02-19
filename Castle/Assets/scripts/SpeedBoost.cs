using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public float BoostMultiplier = 2.0f;
    public float BoostSeconds = 5.0f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name== "Player")
        {
            Destroy(this.transform.parent.gameObject);
            Debug.Log("Must Go Faster");

            PlayerBehavior Player = collision.gameObject.GetComponent<PlayerBehavior>();
            Player.BoostSpeed(BoostMultiplier, BoostSeconds);
        }
    }

}
