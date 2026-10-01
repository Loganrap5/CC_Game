using UnityEngine;
using System.Collections;
using System.Collections.Generic; 


public class GameManager : MonoBehaviour
{
    //For now and for testing, the game will just simply have you choose a certain item. 

    public Transform carSpawnPoint;

    public Transform windowLocation;

    public Transform carEndPoint; 

    public List<Item> masterItemList = new List<Item>();

    //public List<Car> cars = new List<Car>();

    public Car car; 



    //Debugging
    [ContextMenu("Start Game")]
    public void StartGame()
    {
        StartCoroutine(SpawnCarRoutine());
    }

    //Simple car loop
    public IEnumerator SpawnCarRoutine()
    {
        //First spawn car
        Car _car = Instantiate(car, carSpawnPoint.position, Quaternion.identity);

        //Move car towards window location
        _car.MoveCar(windowLocation);

        while(true)
        {
            if(_car.foodDelivered)
            {
                Debug.Log("Food delivered to car!");

                //Move car away from window after food is deliviered
                _car.MoveCar(carEndPoint);

                //Move car away from window - and WAIT until its fully away before starting next car. 
                yield return new WaitUntil(() => Vector3.Distance(_car.transform.position, carEndPoint.transform.position) < 0.1f);

                //Then destroy car
                Destroy(_car.gameObject);

                //Then end CoRoutine
                yield break;
            }

            yield return null; 
        }

    }


    

    

}
