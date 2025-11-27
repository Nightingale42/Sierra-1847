using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGun : MonoBehaviour
{

    public GameObject Gun;
    public Transform WeaponParent;

    // Start is called before the first frame update
    void Start()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.F))
        {
            Drop();
        }
    }
    void Drop()
    {
        WeaponParent.DetachChildren();

        Gun.transform.eulerAngles = new Vector3(Gun.transform.position.x, Gun.transform.position.z, Gun.transform.position.y);
        Gun.GetComponent<Rigidbody>().isKinematic = false;
        Gun.GetComponent<MeshCollider>().enabled = true;
    }
    
    void EquipWeapon()
    {
        Gun.GetComponent<Rigidbody>().isKinematic = true;

        Gun.transform.position = WeaponParent.transform.position; 
         Gun.transform.rotation = WeaponParent.transform.rotation; 

          Gun.GetComponent<MeshCollider>().enabled = false;

          Gun.transform.SetParent(WeaponParent);
    }
        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                if(Input.GetKey(KeyCode.E))
                {
                    EquipWeapon();
                }
            }
        }
            }
        