using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrcBuildings : Orc, IOrcBuildings
{
    [SerializeField] GameObject orcBuildingActionButtonPrefab;
    [SerializeField] Transform orcBuildingActionButtonParent;
    [SerializeField]
    GameObject orcBuildingAltarPrefab, orcBuildingBarrackPrefab
        , orcBuildingTownPrefab;

    public void OrcTown_Hall()
    {
        GenerateOrcBuildingActionButton("Orc Town_Hall");
    }

    public void OrcBarracks()
    {
        GenerateOrcBuildingActionButton("Orc Barracks");
    }

    public void OrcAlter_of_Storms()
    {
        GenerateOrcBuildingActionButton("Orc Alter_of_Storms");
    }

    public void GenerateOrcBuildingActionButton(string orcBuildingName)
    {
        GameObject go = Instantiate(orcBuildingActionButtonPrefab, orcBuildingActionButtonParent);
        go.GetComponentInChildren<TMP_Text>().text = orcBuildingName;
        go.GetComponent<Button>().onClick.AddListener(delegate { GenerateOrcBuilding(orcBuildingName); });
    }

    public void GenerateOrcBuilding(string orcBuildingName)
    {
        CheckIfWhichOneIsNeededToGenerate(orcBuildingName);
    }

    void CheckIfWhichOneIsNeededToGenerate(string buildingName)
    {

        if (buildingName.Equals("Orc Alter_of_Kings"))
        {

            if (GameplayController.instance.RemoveGoldAndWood(20, 20))
            {
                GameObject go = Instantiate(orcBuildingAltarPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Altar";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 20 golds and 20 woods");
        }


        if (buildingName.Equals("Orc Barracks"))
        {

            if (GameplayController.instance.RemoveGoldAndWood(15, 15))
            {
                GameObject go = Instantiate(orcBuildingBarrackPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Barrack";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 15 golds and 15 woods");
        }


        if (buildingName.Equals("Orc Town_Hall"))
        {
            if (GameplayController.instance.RemoveGoldAndWood(5, 5))
            {
                GameObject go = Instantiate(orcBuildingTownPrefab);
                go.GetComponentInChildren<TMP_Text>().text = "Town";
            }
            else
                GameplayController.instance.ShowingInfoText("Not enough gold or wood\nRequires more than 5 golds and 5 woods");
        }
    }

    public override void InitOrcStuffs()
    {
        OrcAlter_of_Storms();
        OrcBarracks();
        OrcTown_Hall();
    }

    public override void InitRace()
    {
        InitOrcStuffs();
    }
}
