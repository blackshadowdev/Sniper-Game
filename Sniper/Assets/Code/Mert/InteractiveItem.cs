using UnityEngine;
using System;

public class InteractiveItem : MonoBehaviour {

    public event Action OnShoot;            //Called when the item is shot

	public void ShootMenuItem() {
        if (OnShoot != null)
            OnShoot();
    }
}
