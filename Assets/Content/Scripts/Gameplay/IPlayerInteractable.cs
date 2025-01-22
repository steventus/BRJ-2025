using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerInteractable
{
    //Press Input Button
    public void OnInputDown();

    public void OnScratch();

    //Release Input Button
    public void OnInputUp();

    //Not pressing any button
    public void OnMiss();
}
