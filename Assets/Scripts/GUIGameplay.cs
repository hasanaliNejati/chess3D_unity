using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Chess
{
    public class GUIGameplay : MonoBehaviour
    {
        [Header("game play")]
        [SerializeField] private PanelScript promotionPanel;
        [Header("victory menu")]
        [SerializeField] private PanelScript endMenu;
        [SerializeField] private TMP_Text endGameMassageText;

        public void EndGame(string massage)
        {
            endMenu.SetActive(true);
            endGameMassageText.text = massage;
            
        }

        public void ShowPawnPromotionMenu()
        {
            promotionPanel.SetActive(true);
        }
    }
}