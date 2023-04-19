 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    [SerializeField] private GameObject _collider1;
    [SerializeField] private GameObject _collider2;
    [SerializeField] private GameObject _collider3;

    private bool _isMain = false;
    private bool _isFront = false;
    private bool _isLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        updateCamera();
        changeCollider();
    }

    void changeCollider()
    {
        if (_isMain)
        {
            _collider1.SetActive(true);
            _collider2.SetActive(false);
            _collider3.SetActive(false);
        }
        else if (_isFront)
        {
            _collider1.SetActive(false);
            _collider2.SetActive(true);
            _collider3.SetActive(false);
        }
        else if (_isLeft)
        {
            _collider1.SetActive(false);
            _collider2.SetActive(false);
            _collider3.SetActive(true);
        }
    }
    void updateCamera()
    {
        _isLeft = _gameController._isLeft;
        _isMain = _gameController._isMain;
        _isFront = _gameController._isFront;
    }
}
