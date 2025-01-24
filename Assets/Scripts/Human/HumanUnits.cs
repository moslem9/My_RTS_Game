using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanUnits : Human, IHumanUnits
{
    [SerializeField] GameObject humanUnitActionButtonPrefab;
    [SerializeField] Transform humanUnitActionButtonParent;
    [SerializeField]
    GameObject humanUnitPrefab, humanUnitArcherPrefab, humanUnitMeleePrefab, humanUnitWorkerPrefab;

    public void HumanWorker(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateHumanUnitActionButton("Peasant");
    }

    public void HumanMelee(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateHumanUnitActionButton("Footman");
    }

    public void HumanRange(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateHumanUnitActionButton("Archer");
    }

    public void HumanHero(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateHumanUnitActionButton("Paladin");
    }

    public void GenerateHumanUnitActionButton(string humanUnitName)
    {
        GameObject go = Instantiate(humanUnitActionButtonPrefab, humanUnitActionButtonParent);
        go.GetComponentInChildren<TMP_Text>().text = humanUnitName;
        go.GetComponent<Button>().onClick.AddListener(delegate () { GenerateHumanUnit(humanUnitName, true); });
    }

    public void GenerateHumanUnit(string humanUnitName, bool byButton)
    {
        InitHumanUnitObjects(humanUnitName, byButton);
    }

    void InitHumanUnitObjects(string humanUnitName, bool byButton)
    {
        if (humanUnitName == "Archer")
        {
            if (GameplayController.instance.RemoveGoldAndWood(3, 3))
            {
                if (byButton)
                {
                    GameObject go = Instantiate(humanUnitArcherPrefab);
                    go.GetComponentInChildren<TMP_Text>().text = humanUnitName;

                    go.GetComponent<HumanRangeUnit>().enabled = true;
                    go.GetComponent<HumanRangeUnit>().rangeObjects.SetActive(true);
                }
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 3 golds and 3 woods");
        }

        if (humanUnitName == "Paladin")
        {
            if (GameplayController.instance.RemoveGoldAndWood(4, 4))
            {
                if (byButton)
                {
                    GameObject go = Instantiate(humanUnitPrefab);
                    go.GetComponentInChildren<TMP_Text>().text = humanUnitName;

                    go.GetComponent<HumanHeroUnit>().enabled = true;
                    go.GetComponent<HumanHeroUnit>().heroObjects.SetActive(true);
                }
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 4 golds and 4 woods");
        }

        if (humanUnitName == "Footman")
        {
            if (byButton)
            {
                GameObject go = Instantiate(humanUnitMeleePrefab);
                go.GetComponentInChildren<TMP_Text>().text = humanUnitName;

                go.GetComponent<HumanMeleeUnit>().enabled = true;
                go.GetComponent<HumanMeleeUnit>().meleeObjects.SetActive(true);
            }
        }

        if (humanUnitName == "Peasant")
        {
            if (byButton)
            {
                GameObject go = Instantiate(humanUnitWorkerPrefab);
                go.GetComponentInChildren<TMP_Text>().text = humanUnitName;
                go.GetComponent<HumanWorkerUnit>().enabled = true;
            }
        }
    }

    public override void InitHumanStuffs()
    {
        HumanHero(true, null);
        HumanMelee(true, null);
        HumanRange(true, null);
        HumanWorker(true, null);
    }

    public override void InitRace()
    {
        InitHumanStuffs();
    }
}
