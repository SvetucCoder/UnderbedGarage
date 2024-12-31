using UnityEngine;

public class RoomChanger : MonoBehaviour
{
    public GameObject room1;
    public GameObject room2;
    public bool firstroom;
    public void RoomChanged(bool change)
    {
        firstroom = change;
    }
    public void ChangeRoom()
    {
        if (firstroom)
        {
            room1.SetActive(true);
            room2.SetActive(false);
        }
        else
        {
            room1.SetActive(false);
            room2.SetActive(true);
        }
    }
}
