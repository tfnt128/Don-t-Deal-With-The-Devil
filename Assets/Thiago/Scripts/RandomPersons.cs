using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPersons : MonoBehaviour
{
    [SerializeField] private GameObject[] persons;
    private int cont;

    public GameObject nextPerson()
    {
        if (cont < persons.Length)
        {
            GameObject person = persons[cont];
            cont++;
            return person;
        }
        else
        {
            return null;
        }
    }
}
