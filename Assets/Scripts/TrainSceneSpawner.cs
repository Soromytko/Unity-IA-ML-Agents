using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSceneSpawner : MonoBehaviour
{
    [SerializeField] private int _count = 10;
    [SerializeField] private float _space = 50f;
    [SerializeField] private GameObject _scene;

    private void Awake() {
        float c = Mathf.Sqrt(_count);
        // c = _count / 2f;
        for (int i = 0; i < _count; i++) {
            float x = i % c;
            float z = (int)(i / c);
            Instantiate(_scene, new Vector3(x, 0,  z) * _space, Quaternion.identity);
        }
    }
}
