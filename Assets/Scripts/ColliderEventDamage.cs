using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderEventDamage : MonoBehaviour
{
    public float damage;


    public void DamageSetting(float m_damage)
    {
        damage = m_damage;
    }

    // 어떤 공격시 생성될 이펙트가 가지고 있을 스크립트
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 데미지 적용
    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("ColliderEventDamage : " + other.name);

        if (other.gameObject.tag == "Enemy")
        {

            //Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Cha"), LayerMask.NameToLayer("EnemyAttack"));
            //collision.rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Debug.Log("적군 공격");
        }
    }
}
