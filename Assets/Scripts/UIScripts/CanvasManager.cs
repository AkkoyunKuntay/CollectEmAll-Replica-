using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : CustomSingleton<CanvasManager>
{
    [SerializeField] CanvasVisibilityController MainCanvas;
    [SerializeField] CanvasVisibilityController LevelPopup;
    [SerializeField] CanvasVisibilityController GameCanvas;

    [SerializeField] Button LevelsButton;
    [SerializeField] ParticleSystem ConfettiExpo;

    public event System.Action<int> LevelIndexSelectedEvent;

    public void OnLevelsButtonClicked()
    {
        LevelPopup.Show();
    }
    public void OnPlayButtonClicked(int levelIndex)
    {
        LevelIndexSelectedEvent?.Invoke(levelIndex);
        LevelPopup.Hide();
        MainCanvas.Hide();
        GameCanvas.Show();

    }

    public IEnumerator LevelEndRoutine(bool isSuccessfull)
    {
        if (isSuccessfull)
        {
            ConfettiExpo.Play(); 
        }
        yield return new WaitForSeconds(1f);
        GameCanvas.Hide();
        yield return new WaitForSeconds(0.25f);
        MainCanvas.Show();
        LevelPopup.Show();

        ConfettiExpo.Stop(true, ParticleSystemStopBehavior.StopEmitting);

    }
}
