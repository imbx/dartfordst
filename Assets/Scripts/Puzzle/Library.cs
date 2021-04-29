using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Library : PuzzleBase {

    public List<Book> Books;
    [SerializeField] private Book ChosenBook;
    private Vector3 offsetBook = Vector3.zero;
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
        if(playerData.isEscapePressed) ChosenBook = null;
        if(CheckList()) {
            this.OnEnd();
        }
    }


    public void GetBook(Book bk) {
        if(!ChosenBook) {
            ChosenBook = bk;
        } else {
            Debug.Log(bk.transform.name + " and " + ChosenBook.transform.name);
            Vector3 tempPos = ChosenBook.BookPosition;
            int bookId = ChosenBook.BookID;
            ChosenBook.SetPos(bk.BookPosition, bk.BookID);
            bk.SetPos(tempPos, bookId);
            ChosenBook = null;
        }

    }
    
    bool CheckList() {
        for(int i = 0; i < Books.Count; i++) {
            if(Books[i].BookID != i + 1) return false;
        }
        return true;
    }
}