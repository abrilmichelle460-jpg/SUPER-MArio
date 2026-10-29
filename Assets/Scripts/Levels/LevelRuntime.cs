
using UnityEngine;

/// <summary>
/// Presentación y configuración ambiental de los niveles.
/// </summary>
public class LevelRuntime : MonoBehaviour
{
    public bool underwater;
    public float waterExitX = 194f;
    public float surfaceStartX = 174f;

    private PlayerMovement player;
    private Camera stageCamera;
    private GUIStyle hud;

    private void Start()
    {
        GameObject mario = GameObject.FindGameObjectWithTag("Player");

        if (mario != null)
        {
            player = mario.GetComponent<PlayerMovement>();
        }

        stageCamera = Camera.main;
    }

    private void Update()
    {
        // Activar o desactivar el movimiento acuático.
        if (player != null)
        {
            player.swimming =
                underwater &&
                player.transform.position.x < waterExitX;
        }

        // Cambiar el fondo según la zona del nivel.
        if (player != null && stageCamera != null)
        {
            if (player.transform.position.x >= surfaceStartX)
            {
                stageCamera.backgroundColor =
                    new Color32(92, 148, 252, 255);
            }
            else if (underwater)
            {
                stageCamera.backgroundColor =
                    new Color32(32, 56, 236, 255);
            }
            else
            {
                stageCamera.backgroundColor = Color.black;
            }
        }
    }

    private void OnGUI()
    {
        GameManager game = GameManager.Instance;

        if (game == null)
        {
            return;
        }

        if (hud == null)
        {
            hud = new GUIStyle(GUI.skin.label)
            {
                fontSize = 17,
                fontStyle = FontStyle.Bold
            };

            hud.normal.textColor = Color.white;
        }

        // Mostrar información del jugador y del nivel.
        GUI.Label(
            new Rect(18, 8, Screen.width - 36, 30),
            $"MARIO  x{game.lives}       " +
            $"COINS  {game.coins:00}       " +
            $"WORLD  {game.world}-{game.stage}",
            hud
        );

        // Mostrar los controles según el entorno.
        string controls;

        if (underwater && player != null && player.swimming)
        {
            controls = "ARROWS / A,D: MOVE     SPACE: SWIM";
        }
        else
        {
            controls = "ARROWS / A,D: MOVE     SPACE: JUMP     S: ENTER PIPE";
        }

        GUI.Label(
            new Rect(
                18,
                Screen.height - 30,
                Screen.width - 36,
                28
            ),
            controls,
            hud
        );
    }
}
