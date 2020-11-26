using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickSelect : ControllerInput
{
    static public int playersPlaying = 0;

    public UnityEngine.UI.Text text;
    public Material[] choices;
    public SkinnedMeshRenderer mouse;

    private int choice = 0;
    private bool joined = false;
    private float choiceTimer = 0f;
    private float timer = 0f;
    private bool finished = false;
    private int bet = 0;
    public UnityEngine.UI.Image fade;

    public override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        if (GetBackButton() && !finished)
        {
            finished = true;

            ControllerInput.available = 0;

            int level = 0;
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);

            return;
        }

        if (GetJumpButton() && ControllerExists() && timer < Time.time)
        {
            text.text = joined ? "Press A to join" : controllerInput.Replace("Controller", "Player");
            joined = !joined;

            playersPlaying += joined ? 1 : -1;
            Debug.Log(playersPlaying);

            timer = Time.time + 0.5f;

            mouse.enabled = joined;

            PlayerManager.characterInfo[] old = PlayerManager.characters;

            if (!joined)
            {
                int newVar = PlayerManager.characters.Length - 1;

                PlayerManager.characters = new PlayerManager.characterInfo[newVar];

                int bonus = 0;

                if (PlayerManager.characters.Length > 0)
                {
                    for (int i = 0; i < old.Length; i++)
                    {
                        if (i != bet)
                        {
                            PlayerManager.characters[bonus] = old[i];
                            bonus++;
                        }
                    }
                }

                bet = 0;
            }
            else if (bet == 0 && joined)
            {
                bet = PlayerManager.characters.Length + 1;

                PlayerManager.characters = new PlayerManager.characterInfo[bet];

                for (int i = 0; i < old.Length; i++)
                {
                    PlayerManager.characters[i] = old[i];
                }

                PlayerManager.characters[bet - 1] = new PlayerManager.characterInfo();
                PlayerManager.characters[bet - 1].controller = GetSelectedController();
                PlayerManager.characters[bet - 1].body = choices[choice];
            }
        }

        if (joined && playersPlaying >= 2 && Clicking())
        {
            fade.color = Color.Lerp(fade.color, new Color(fade.color.r, fade.color.g, fade.color.b, 1), Time.deltaTime);

            if (fade.color.a >= 0.9 && !finished)
            {
                finished = true;
                ControllerInput.available = 0;

                int level = 2;
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(level);
            }
        }
        else if (joined && !Clicking()) fade.color = Color.Lerp(fade.color, new Color(fade.color.r, fade.color.g, fade.color.b, 0), Time.deltaTime);

        if (choiceTimer < Time.time)
        {
            if (GetHorizontalAxis() > 0.5)
            {
                choiceTimer = Time.time + 0.15f;
                choice++;

                if (choice >= choices.Length)
                    choice = 0;

                mouse.material = choices[choice];
                PlayerManager.characters[bet - 1].body = choices[choice];
            }
            else if (GetHorizontalAxis() < -0.5)
            {
                choiceTimer = Time.time + 0.15f;
                choice--;

                if (choice <= 0)
                    choice = choices.Length - 1;

                mouse.material = choices[choice];
                PlayerManager.characters[bet - 1].body = choices[choice];
            }
        }
    }
}
