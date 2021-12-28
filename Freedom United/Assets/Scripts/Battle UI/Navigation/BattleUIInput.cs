using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleUIInput : MonoBehaviour
{
    [SerializeField] private BattleUINavigation battleUINavigation;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            battleUINavigation.Down();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            battleUINavigation.Up();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            battleUINavigation.Left();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            battleUINavigation.Right();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            battleUINavigation.Forward();
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            battleUINavigation.Backwards();
        }
    }
}
