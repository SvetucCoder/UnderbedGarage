using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public List<Image> IconUI;
    private List<Item> _haveitems = new List<Item>();
    public Animator _animator;
    public static int currentID;
    void Awake()
    {
        _haveitems = new List<Item>();
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].count > 0)
            {
                _haveitems.Add(items[i]);
            }
        }
       // DisplayInventory();
    }
    //Подонки.
    public void AddItem(int ID, int count)
    {
        items[ID].count += count;
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].count > 0 && !_haveitems.Contains(items[i]))
            {
                _haveitems.Add(items[i]);
            }
        }
        DisplayInventory();
    }
    public void RemoveItem(int ID, int count)
    {
        if(items[ID].count > 0)
        {
            items[ID].count -= count;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].count == 0 && _haveitems.Contains(items[i]))
                {
                    _haveitems.Remove(items[i]);
                    items[i].Object.SetActive(false);
                }
            }
            DisplayInventory();
        }
    }

    public void DisplayInventory() 
    {
        for(int i = 0; i < IconUI.Count; i++)
        {
            if (IconUI[i].gameObject.activeSelf)
            {
                IconUI[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < _haveitems.Count; i++)
        {
            if (!IconUI[i].gameObject.activeSelf)
            {
                IconUI[i].gameObject.SetActive(true);
                IconUI[i].sprite = _haveitems[i].Icon;
            }
        }
    }
    public RectTransform rectTransform;
    bool opened;
    int itemselected;
    bool drag;
    int dragableobj;
    public static void Swap<T>(List<T> list, int index1, int index2)
    {
        T temp = list[index1];
        list[index1] = list[index2];
        list[index2] = temp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(SurfCharacter.key.Inventory)) OpenInventory();
        if (Input.GetKeyDown(KeyCode.Alpha1)) ChangeItemView(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) ChangeItemView(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) ChangeItemView(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) ChangeItemView(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) ChangeItemView(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) ChangeItemView(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) ChangeItemView(6);
        if (Input.GetKeyDown(KeyCode.Alpha8)) ChangeItemView(7); 
        if (Input.GetKeyDown(KeyCode.Alpha9)) ChangeItemView(8);
        float scrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (scrollValue > 0f) 
        {
            if (itemselected < 8 && itemselected < _haveitems.Count) itemselected++;
            else itemselected = 0;
            ChangeItemView(itemselected);
        }
        if (scrollValue < 0f)
        {
            if (itemselected > 0) itemselected--;
            else itemselected = _haveitems.Count;
            ChangeItemView(itemselected);
        }

        // Проверяем, зажата ли левая кнопка мыши
        if (Input.GetMouseButton(0)) // 0 — это левая кнопка мыши
        {

            // Перемещаем панель в позицию курсора
            Vector2 mousePosition = Input.mousePosition;

            if (!drag) {
                for (int i = 0; i < _haveitems.Count; i++)
                {
                    if (RectTransformUtility.RectangleContainsScreenPoint(IconUI[i].rectTransform, mousePosition, Camera.main))
                    {
                        Vector2 localPosition = IconUI[i].rectTransform.parent.InverseTransformPoint(mousePosition);
                        IconUI[i].rectTransform.localPosition = localPosition;

                        drag = true;
                        dragableobj = i;
                        break;
                    }
                }
            }
            else
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(IconUI[dragableobj].rectTransform, mousePosition, Camera.main))
                {
                    Vector2 localPosition = IconUI[dragableobj].rectTransform.parent.InverseTransformPoint(mousePosition);
                    IconUI[dragableobj].rectTransform.localPosition = localPosition;
                }
            }
            
        }
        else if(!Input.GetMouseButton(0) && drag)
        {
            Vector2 mousePosition = Input.mousePosition;
            for (int i = 0; i < _haveitems.Count; i++)
            {
                if (RectTransformUtility.RectangleContainsScreenPoint(IconUI[i].rectTransform, mousePosition, Camera.main) && dragableobj != i)
                {
                    IconUI[dragableobj].rectTransform.localPosition = new Vector3(0, 0, 0);
                    for (int r = 0; r < _haveitems.Count; r++)
                    {
                        _haveitems[r].Object.SetActive(false);
                    }

                    Swap(_haveitems, i, dragableobj);


                    drag = false;

                    if(itemselected < _haveitems.Count) _haveitems[itemselected].Object.SetActive(true);
                    break;
                }
            }
            if (drag)
            {
                IconUI[dragableobj].rectTransform.localPosition = new Vector3(0, 0, 0);
                drag = false;
            }
            
            dragableobj = 0;
            DisplayInventory();
        }

    }
    private void ChangeItemView(int ID)
    {
        for (int i = 0; i < _haveitems.Count; i++)
        {
            if (_haveitems[i].Object.activeSelf)
            {
                _haveitems[i].Object.SetActive(false);
                Image parentImage = IconUI[i].transform.parent.GetComponent<Image>();
                parentImage.color = new Color(60f / 255f, 60f / 255f, 60f / 255f, 100f / 255f);
            }
        } 
        if(ID < _haveitems.Count)
        {
            _haveitems[ID].Object.SetActive(true);
            Image parentImage = IconUI[ID].transform.parent.GetComponent<Image>();
            parentImage.color = new Color(1f, 1f, 1f, 0.39f); 

            itemselected = ID;
            currentID = _haveitems[ID].ID;
        }
        else
        {
            currentID = -1;
        }

    }
    private void OpenInventory()
    {
        if (opened)
        {
            ResumeGame();
            _animator.Play("InventoryCloses", 0);
        }
        else
        {
            _animator.Play("InventoryOpens", 0);
            PauseGameLogic();
        }
        opened = !opened;
    }

    private void PauseGameLogic()
    {
        Time.timeScale = 0f; // Останавливает время

        // Включите курсор для взаимодействия с UI
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Возвращает нормальное время

        // Уберите курсор, если это необходимо для игры
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

//#рыбаки
[System.Serializable]
public class Item
{
    public string Name;
    public string Description;
    public int ID;
    public Sprite Icon;
    public GameObject Object;
    public int count;
}