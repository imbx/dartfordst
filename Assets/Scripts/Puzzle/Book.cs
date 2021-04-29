using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Book : InteractBase {
    [SerializeField] private int BookID;
    private Vector2 InnerPosition;
    public Vector3 BookPosition; 
    public bool canPlace = true;
    [SerializeField] private UnityEvent<Book> Action;

    private Book otherBook;

    private void OnEnable() {
        BookPosition = transform.position;
    }

    private void OnTriggerStay(Collider other) {
        if(other.GetComponent<Book>()) {
            otherBook = other.GetComponent<Book>();
            canPlace = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        otherBook = null;
        canPlace = false;
    }

    public override void Execute() {
        Action.Invoke(this);
    }

    public void SetPos(Vector3 oBook) {
        transform.position = oBook;
        BookPosition = transform.position;
    }

    public void PlaceBook() {
        if(otherBook) {
            transform.position = otherBook.BookPosition;
            otherBook.SetPos(BookPosition);
            BookPosition = transform.position;
        }
    }
    
}