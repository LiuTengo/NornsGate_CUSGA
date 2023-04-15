using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    [Header("Parts")]
    [SerializeField] private GameObject _partDisable;
    [SerializeField] private GameObject _partEnable;

    private void OnTriggerEnter(Collider other)
    {
        _partDisable.SetActive(false);
        _partEnable.SetActive(true);
    }
}
