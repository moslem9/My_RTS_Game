using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrcUnits 
{
    void OrcWorker(bool asButton, Transform[] spawnPoints);
    void OrcMelee(bool asButton, Transform[] spawnPoints);
    void OrcRange(bool asButton, Transform[] spawnPoints);
    void OrcHero(bool asButton, Transform[] spawnPoints);
}
