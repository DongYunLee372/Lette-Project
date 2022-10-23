using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_trigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (other.gameObject.transform.position.z > this.gameObject.transform.position.z)
            {
                this.gameObject.GetComponent<MeshCollider>().isTrigger = false;
                CharacterCreate.Instance.obj_boss.GetComponent<Battle_Character>().real_AI.isPause = false;
            }
        }
    }
}
