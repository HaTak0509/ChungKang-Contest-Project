using System.Collections.Generic;
using UnityEngine;

public class SettingManager : MonoBehaviour
{
    public static SettingManager Instance;

    private Stack<MenuType> menuStack = new Stack<MenuType>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void Push(MenuType menu)
    {
        menuStack.Push(menu);
    }

    public MenuType Pop()
    {
        if (menuStack.Count == 0)
            return MenuType.Menu;

        return menuStack.Pop();
    }

    public MenuType Peek()
    {
        if (menuStack.Count == 0)
            return MenuType.Menu;

        return menuStack.Peek();
    }

    public int Count => menuStack.Count;

    public void Clear()
    {
        menuStack.Clear();
    }
}
