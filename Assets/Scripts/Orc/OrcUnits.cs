using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrcUnits : Orc, IOrcUnits
{
    [SerializeField] GameObject orcUnitActionButtonPrefab;
    [SerializeField] Transform orcUnitActionButtonParent;
    [SerializeField]
    GameObject orcUnitPrefab, orcUnitArcherPrefab, orcUnitMeleePrefab, orcUnitWorkerPrefab;

    public void OrcWorker(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateOrcUnitActionButton("Peon");
    }

    public void OrcMelee(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateOrcUnitActionButton("Grunt");
    }

    public void OrcRange(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateOrcUnitActionButton("Berserker");
    }

    public void OrcHero(bool asButton, Transform[] spawnPoints)
    {
        if (asButton)
            GenerateOrcUnitActionButton("Blademaster");
    }

    public void GenerateOrcUnitActionButton(string orcUnitName)
    {
        GameObject go = Instantiate(orcUnitActionButtonPrefab, orcUnitActionButtonParent);
        go.GetComponentInChildren<TMP_Text>().text = orcUnitName;
        go.GetComponent<Button>().onClick.AddListener(delegate () { GenerateOrcUnit(orcUnitName, true); });
    }

    public void GenerateOrcUnit(string orcUnitName, bool byButton)
    {

        InitOrcUnitObjects(orcUnitName, byButton);
    }

    void InitOrcUnitObjects(string orcUnitName, bool byButton)
    {
        if (orcUnitName == "Berserker")
        {
            if (GameplayController.instance.RemoveGoldAndWood(3, 3))
            {
                if (byButton)
                {
                    GameObject go = Instantiate(orcUnitArcherPrefab);
                    go.GetComponentInChildren<TMP_Text>().text = orcUnitName;

                    go.GetComponent<OrcRangeUnit>().enabled = true;
                    go.GetComponent<OrcRangeUnit>().rangeObjects.SetActive(true);
                }
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 3 golds and 3 woods");
        }

        if (orcUnitName == "Blademaster")
        {
            if (GameplayController.instance.RemoveGoldAndWood(4, 4))
            {
                if (byButton)
                {
                    GameObject go = Instantiate(orcUnitPrefab);
                    go.GetComponentInChildren<TMP_Text>().text = orcUnitName;

                    go.GetComponent<OrcHeroUnit>().enabled = true;
                    go.GetComponent<OrcHeroUnit>().heroObjects.SetActive(true);
                }
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 4 golds and 4 woods");
        }

        if (orcUnitName == "Grunt")
        {
            if (byButton)
            {
                GameObject go = Instantiate(orcUnitMeleePrefab);
                go.GetComponentInChildren<TMP_Text>().text = orcUnitName;

                go.GetComponent<OrcMeleeUnit>().enabled = true;
                go.GetComponent<OrcMeleeUnit>().meleeObjects.SetActive(true);
            }
        }

        if (orcUnitName == "Peon")
        {
            if (byButton)
            {
                GameObject go = Instantiate(orcUnitWorkerPrefab);
                go.GetComponentInChildren<TMP_Text>().text = orcUnitName;
                go.GetComponent<OrcWorkerUnit>().enabled = true;
            }
        }
    }

    public override void InitOrcStuffs()
    {
        OrcHero(true, null);
        OrcMelee(true, null);
        OrcRange(true, null);
        OrcWorker(true, null);
    }

    public override void InitRace()
    {
        InitOrcStuffs();
    }
}
