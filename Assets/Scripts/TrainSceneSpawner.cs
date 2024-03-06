using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainSceneSpawner : MonoBehaviour
{
    [SerializeField] private int _count = 10;
    [SerializeField] private float _space = 50f;
    [SerializeField] private GameObject _scene;

    [SerializeField] private int _successCount = 0;
    [SerializeField] private Dictionary<GameObject, int> _instances = new Dictionary<GameObject, int>();

    private void Awake() {
        float c = Mathf.Sqrt(_count);
        // c = _count / 2f;
        for (int i = 0; i < _count; i++) {
            float x = i % c;
            float z = (int)(i / c);
            GameObject instance = Instantiate(_scene, new Vector3(x, 0,  z) * _space, Quaternion.identity);
            instance.transform.parent = transform;
            instance.GetComponent<GameEnvController>().door.onDoorOpen.AddListener(() => {
                _instances[instance]++;
                _successCount++;
            });
            _instances.Add(instance, 0);
        }
    }
}
