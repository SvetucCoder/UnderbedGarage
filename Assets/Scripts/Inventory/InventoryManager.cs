using System.Collections;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
	public Inventory playerInventory;

	[SerializeField]
	private RectTransform _rect;

	public Animator animator;
	public bool _isClosed = false;
	public bool _isOpened = false;

	void Awake()
	{
		animator = GetComponent<Animator>();
		playerInventory = gameObject.AddComponent<Inventory>();
		playerInventory.DisplayInventory();
		//Item key = new Item("Rabbit Key", "A key that looks like a rabbit's head. If you collect enough, something might happen.", 1, null);
		//playerInventory.AddItem(key);
		//#рыбаки
	}

	private void Update()
	{
		if(Input.GetButtonDown("InventoryButton"))
		{
			//animator.SetTrigger("OpenClose");
			_isOpened = !_isOpened;
			StartCoroutine(MoveUp());
            StopGame();
		}
	}

	private void StopGame()
	{
		if(_isOpened == true)
		{
			//Time.timeScale = 0f;
		}
		else
		{
            //Time.timeScale = 1f;
        }
	}

	private IEnumerator MoveUp()
	{
		while(!_isOpened)
		{
            _rect.offsetMax += Vector2.up * 0.01f;
            _rect.offsetMin += Vector2.up * 0.01f;
			Debug.Log(_rect.anchoredPosition);

			yield return new WaitForEndOfFrame();
        }
	}

    private void FixedUpdate()
    {
        if(_isOpened == true)
		{
			
		}
	}
}
