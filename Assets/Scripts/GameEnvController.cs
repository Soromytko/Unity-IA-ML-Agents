using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class GameEnvController : MonoBehaviour
{
    [SerializeField] private GameObject _debugResultObject;

    public int buttonsOnEpisode = 4;
    public int boxesOnEpisode = 3;

    private Agent agent;
    public GridedDistributor buttonsDistributor;
    public GridedDistributor boxDistributor;
    public GridedDistributor agentsDistributor;
    public Door door;
    public MeshCollider goal;

    void Start()
    {
        ResetScene();
    }

    void ResetScene()
    {
        var buttons = buttonsDistributor.Respawn(buttonsOnEpisode);
        boxDistributor.Respawn(boxesOnEpisode);
        var activators = new DoorActivator[buttons.Length];
        for (var i = 0; i < buttons.Length; i++)
            activators[i] = buttons[i].GetComponent<Button>();
        door.ResetActivators(activators);

        agent = agentsDistributor.Respawn(1)[0].GetComponent<Agent>();

        agent.GetComponent<AgentPusher>().WallCollided.AddListener(() => {
            ResetScene();
            agent.AddReward(-1f);
            agent.EndEpisode();
            // foreach (GameObject wall in GameObject.FindGameObjectsWithTag("wall"))
            //     wall.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        });
    }

    public void OnGoalTriggered()
    {
        agent.AddReward(10f);
        agent.EndEpisode();
        ResetScene();

        StartCoroutine(ShowingDebugResult(0.5f));

        
        // foreach (GameObject wall in GameObject.FindGameObjectsWithTag("wall"))
        //     wall.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
    }

    IEnumerator ShowingDebugResult(float time)
    {
        _debugResultObject.SetActive(true);
        yield return new WaitForSeconds(time);
        _debugResultObject.SetActive(false);
    }
}
