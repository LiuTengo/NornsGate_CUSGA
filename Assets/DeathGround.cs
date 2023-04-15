using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathGround : MonoBehaviour
{
    public Transform _reloadTransform;
    public GameObject _player;
    public CanvasGroup _canvasGroup;

    private Vector3 _reloadposition;

    // Start is called before the first frame update
    void Start()
    {
        _reloadposition = _reloadTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("Player")) {

            _canvasGroup.alpha = Mathf.Lerp(_canvasGroup.alpha, 1, 0.3f);
            StartCoroutine(LoadThisScene());
        }
    }

    IEnumerator LoadThisScene() {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
