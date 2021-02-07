using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioClip _coinPickUp;

    private void OnTriggerStay(Collider other)
    {
       if(other.tag=="Player")
        {
            Player player = other.GetComponent<Player>();

            if(player!=null && Input.GetKeyDown(KeyCode.E))
            {
                player.hasCoin = true;
                UIManager uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
                if(uiManager!=null)
                {
                    uiManager.PickUpCoin();
                }

                AudioSource.PlayClipAtPoint(_coinPickUp, transform.position, 1f);
                Destroy(this.gameObject);
            }
        }
    }
}
