using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorKey : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private Transform _player;

    private void Start()
    {
        _door.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_player.position, transform.position) < 3f)
        {
            if (Input.GetKeyDown(KeyCode.E))
                _door.SetActive(true);
        }
    }
}
