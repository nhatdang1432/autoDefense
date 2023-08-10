using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.InventoryEngine;
using MoreMountains.TopDownEngine;

public class Testboss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
      MMBossMission.Trigger(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        //MMBossMission.Trigger(this.transform);

    }
}