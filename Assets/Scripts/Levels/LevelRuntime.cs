using UnityEngine;

/// <summary>Stage presentation and environment settings for the new levels only.</summary>
public class LevelRuntime : MonoBehaviour
{
    public bool underwater;
    public float waterExitX = 194f;
    public float surfaceStartX = 174f;
    private PlayerMovement player;
    private Camera stageCamera;
    private GUIStyle hud;
    private GUIStyle complete;

    private void Start()
    {
        GameObject mario = GameObject.FindGameObjectWithTag("Player");
        if (mario != null) player = mario.GetComponent<PlayerMovement>();
        stageCamera = Camera.main;
    }

    private void Update()
    {
        if (player != null) player.swimming = underwater && player.transform.position.x < waterExitX;
        if (player != null && stageCamera != null)
            stageCamera.backgroundColor = player.transform.position.x >= surfaceStartX ? new Color32(92, 148, 252, 255) :
                underwater ? new Color32(32, 56, 236, 255) : Color.black;
        if (GameManager.Instance != null && GameManager.Instance.awaitingNextLevel && Input.GetKeyDown(KeyCode.Return))
            GameManager.Instance.NewGame();
    }

    private void OnGUI()
    {
        GameManager game = GameManager.Instance;
        if (game == null) return;
        if (hud == null)
        {
            hud = new GUIStyle(GUI.skin.label) { fontSize = 17, fontStyle = FontStyle.Bold };
            hud.normal.textColor = Color.white;
            complete = new GUIStyle(hud) { alignment = TextAnchor.MiddleCenter, fontSize = 26, wordWrap = true };
        }
        GUI.Label(new Rect(18, 8, Screen.width - 36, 30),
            $"MARIO  x{game.lives}       COINS  {game.coins:00}       WORLD  {game.world}-{game.stage}", hud);
        if (game.awaitingNextLevel)
        {
            GUI.Box(new Rect(Screen.width * .15f, Screen.height * .3f, Screen.width * .7f, Screen.height * .4f), "");
            GUI.Label(new Rect(Screen.width * .18f, Screen.height * .32f, Screen.width * .64f, Screen.height * .36f),
                $"WORLD {game.world}-{game.stage} COMPLETE\n\nWORLD {game.pendingLevel} — COMING SOON\n\nENTER TO PLAY AGAIN", complete);
        }
        else GUI.Label(new Rect(18, Screen.height - 30, Screen.width - 36, 28),
            underwater && player != null && player.swimming ? "ARROWS / A,D: MOVE     SPACE: SWIM     D: ENTER EXIT PIPE" :
            "ARROWS / A,D: MOVE     SPACE: JUMP     S: ENTER PIPE", hud);
    }
}
