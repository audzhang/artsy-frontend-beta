using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
  public SnapshotCamera snapCam;

  public void Screenshot() {
    // take screenshot
    snapCam.CallTakeSnapshot();
  }
}
