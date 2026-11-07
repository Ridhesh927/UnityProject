
using UnityEngine;
using TMPro;

public class GameHUD : MonoBehaviour
{
    public WaveManager waveManager;
    public TMP_Text waveText;
    public TMP_Text enemiesText;
    public TMP_Text killsText;

    void Update()
    {
        if (waveManager == null)
            return;

        if (waveText != null)
            waveText.text = "WAVE: " + waveManager.CurrentWave;

        if (enemiesText != null)
            enemiesText.text =
                "ENEMIES LEFT: " + waveManager.EnemiesAlive;

        if (killsText != null)
            killsText.text = "KILLS: " + waveManager.Kills;
    }
}
