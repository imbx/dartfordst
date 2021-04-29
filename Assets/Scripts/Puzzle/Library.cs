using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Library : PuzzleBase {

    public List<Book> Books;
    [SerializeField] private Book ChosenBook;
    private Vector3 offsetBook = Vector3.zero;
    public UnityEvent OnResetLightbox;
    [SerializeField] private PrimaryController playerData;
    private Vector3 lastMousePos = Vector3.zero;

    void Start(){
        // LibrarySelf = new int();
        // this.OnStart();
    }
    public override void Execute()
    {
        this.OnStart();
    }

    void Update() {
        if(ChosenBook) {
        }
        if(playerData.isEscapePressed) ChosenBook = null;
        //this.OnEnd();
    }


    public void GetBook(Book bk) {
        if(!ChosenBook) {
            ChosenBook = bk;
        } else {
            Debug.Log(bk.transform.name + " and " + ChosenBook.transform.name);
            Vector3 tempPos = ChosenBook.BookPosition;
            ChosenBook.SetPos(bk.BookPosition);
            bk.SetPos(tempPos);
            ChosenBook = null;
        }

    }
}