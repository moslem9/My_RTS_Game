using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHumanUnits
{
    void HumanWorker(bool asButton,Transform[] spawnPoints);
    void HumanMelee(bool asButton, Transform[] spawnPoints);
    void HumanRange(bool asButton, Transform[] spawnPoints);
    void HumanHero(bool asButton,Transform[] spawnPoints);
}
