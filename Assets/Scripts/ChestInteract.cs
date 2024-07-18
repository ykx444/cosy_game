using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : Interactable
{
    //[SerializeField] Animator anim;
    //[SerializeField] bool opened = false;
    //bool isPlayerNear = false;
    //public override void Interact()
    //{
    //    opened = !opened;//toggle
    //    anim.SetBool("isOpen", opened);
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        isPlayerNear = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        isPlayerNear = false;
    //        if (opened)
    //        {
    //            Interact();
    //        }
    //    }
    //}

    //private void Update()
    //{
    //    //#1: when user press E and was close to chest, chest opens
    //    //#2: user moves away from chest, chest cloes
    //    //#3: user press E again to close
    //    //TODO:#4: user close UI to close
    //    if (isPlayerNear && Input.GetMouseButtonDown(1))
    //    {
    //        Interact();
    //    }
    //}
}
