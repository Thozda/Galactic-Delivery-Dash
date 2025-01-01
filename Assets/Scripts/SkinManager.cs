using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static int SkinSelected;

    [SerializeField] TextMeshProUGUI RedText;
    [SerializeField] TextMeshProUGUI BlueText;
    [SerializeField] TextMeshProUGUI GreenText;
    [SerializeField] TextMeshProUGUI PurpleText;

    [SerializeField] GameObject BlueLocked;
    [SerializeField] GameObject GreenLocked;
    [SerializeField] GameObject PurpleLocked;

    private void Start()
    {
        SkinSelected = PlayerPrefs.GetInt("Skin", 0);
        RedText.text = "Unlocked";
        BlueText.text = "Unlocked";
        GreenText.text = "Unlocked";
        PurpleText.text = "Unlocked";
        switch (SkinManager.SkinSelected)
        {
            case 0:
                RedText.text = "Equipped";
                break;
            case 1:
                BlueText.text = "Equipped";
                break;
            case 2:
                GreenText.text = "Equipped";
                break;
            case 3:
                PurpleText.text = "Equipped";
                break;
        }

        if (PlayerPrefs.GetInt("BlueLocked", 0) == 1)
        {
            BlueLocked.SetActive(false);
        }
        if (PlayerPrefs.GetInt("GreenLocked", 0) == 1)
        {
            GreenLocked.SetActive(false);
        }
        if (PlayerPrefs.GetInt("PurpleLocked", 0) == 1)
        {
            PurpleLocked.SetActive(false);
        }
    }

    private void HandleEquip(int _selection, TextMeshProUGUI _text)
    {
        int _previousSelection = SkinSelected;
        SkinManager.SkinSelected = _selection;
        PlayerPrefs.SetInt("Skin", SkinSelected);
        PlayerPrefs.Save();
        switch (_previousSelection)
        {
            case 0:
                RedText.text = "Unlocked";
                break;
            case 1:
                BlueText.text = "Unlocked";
                break;
            case 2:
                GreenText.text = "Unlocked";
                break;
            case 3:
                PurpleText.text = "Unlocked";
                break;

        }
        _text.text = "Equipped";
    }
    private void HandleUnlock(int _cost, GameObject _button)
    {
        //check credits, minus credits and disable gameobject
        if (CurrencyManager.credits >= _cost)
        {
            CurrencyManager.credits -= _cost;
            PlayerPrefs.SetInt("credits", CurrencyManager.credits);
            PlayerPrefs.Save();

            _button.SetActive(false);

            if (_button == BlueLocked)
            {
                BlueEquip();
                PlayerPrefs.SetInt("BlueLocked", 1);
                PlayerPrefs.Save();
            }
            else if (_button == GreenLocked)
            {
                GreenEquip();
                PlayerPrefs.SetInt("GreenLocked", 1);
                PlayerPrefs.Save();
            }
            else if (_button == PurpleLocked)
            {
                PurpleEquip();
                PlayerPrefs.SetInt("PurpleLocked", 1);
                PlayerPrefs.Save();
            }
        }
    }

    public void RedEquip()
    {
        HandleEquip(0, RedText);
    }
    public void BlueEquip()
    {
        HandleEquip(1, BlueText);
    }
    public void GreenEquip()
    {
        HandleEquip(2, GreenText);
    }
    public void PurpleEquip()
    {
        HandleEquip(3, PurpleText);
    }

    public void BlueLockedFunction()
    {
        HandleUnlock(10000, BlueLocked);
    }
    public void GreenLockedFunction()
    {
        HandleUnlock(20000, GreenLocked);
    }
    public void PurpleLockedFunction()
    {
        HandleUnlock(50000, PurpleLocked);
    }
}
