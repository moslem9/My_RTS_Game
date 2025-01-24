using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HumanBuildings : Human, IHumanBuildings
{
    [SerializeField] GameObject humanBuildingActionButtonPrefab;
    [SerializeField] Transform humanBuildingActionButtonParent;
    [SerializeField] GameObject humanBuildingAltarPrefab, humanBuildingBarrackPrefab, humanBuildingTownPrefab;

    public void HumanTown_Hall()
    {
        GenerateHumanBuildingActionButton("Human Town_Hall");
    }

    public void HumanBarracks()
    {
        GenerateHumanBuildingActionButton("Human Barracks");
    }

    public void HumanAlter_of_Kings()
    {
        GenerateHumanBuildingActionButton("Human Alter_of_Kings");
    }

    public void GenerateHumanBuildingActionButton(string humanBuildingName)
    {
        GameObject go = Instantiate(humanBuildingActionButtonPrefab, humanBuildingActionButtonParent);
        go.GetComponentInChildren<TMP_Text>().text = humanBuildingName;
        go.GetComponent<Button>().onClick.AddListener(delegate { GenerateHumanBuilding(humanBuildingName); });
    }

    public void GenerateHumanBuilding(string humanBuildingName)
    {
        CheckIfWhichOneIsNeededToGenerate(humanBuildingName);
    }

    void CheckIfWhichOneIsNeededToGenerate(string buildingName)
    {

        if (buildingName.Equals("Human Alter_of_Kings"))
        {

            if (GameplayController.instance.RemoveGoldAndWood(20, 20))
            {
                GameObject go = Instantiate(humanBuildingAltarPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Altar";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 20 golds and 20 woods");
        }


        if (buildingName.Equals("Human Barracks"))
        {

            if (GameplayController.instance.RemoveGoldAndWood(15, 15))
            {
                GameObject go = Instantiate(humanBuildingBarrackPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Barrack";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 15 golds and 15 woods");
        }


        if (buildingName.Equals("Human Town_Hall"))
        {

            if (GameplayController.instance.RemoveGoldAndWood(5, 5))
            {
                GameObject go = Instantiate(humanBuildingTownPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Town";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 5 golds and 5 woods");
        }
    }

    public override void InitHumanStuffs()
    {
        HumanAlter_of_Kings();
        HumanBarracks();
        HumanTown_Hall();
    }

    public override void InitRace()
    {
        InitHumanStuffs();
    }
}
