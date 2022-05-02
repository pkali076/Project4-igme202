using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [SerializeField]
    GameObject humanPrefab, zombiePrefab;

    [SerializeField]
    List<Human> humans = new List<Human>();

    [SerializeField]
    List<Zombie> zombies = new List<Zombie>();

    [SerializeField]
    Transform worldTransform;
    Vector3 worldSize = Vector3.zero;
   // Bounds worldBounds;


    //vehicle have reference to scene manager
    //scene manager is parent of giving information in world to each vehicle

    // Start is called before the first frame update
    void Start()
    {
        worldSize = worldTransform.localScale * 10f;

      //  worldBounds = new Bounds(worldTransform.position, worldTransform.localScale * 10f);
      // worldBounds = new Bounds(worldTransform.position, worldTransform.localScale * 10f);

        
        for(int i = 0; i < 6; i++)
        {
           
            zombies.Add(Instantiate((zombiePrefab).GetComponent<Zombie>(), new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f)), Quaternion.identity));
        }
        for(int i = 0; i < 4; i++)
        {
            
            
            humans.Add(Instantiate((humanPrefab).GetComponent<Human>(), new Vector3(Random.Range(-20f, 20f), 0, Random.Range(-20f, 20f)), Quaternion.identity));
        }

       // humans[0].worldBounds = worldBounds;
        humans[0].sceneManager = this;

        // zombies[0].worldBounds = worldBounds;
        zombies[0].sceneManager = this;

        //both zombies and humans have access to public variables from sceneManager
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(worldTransform.position, worldSize);
    }
    
}
