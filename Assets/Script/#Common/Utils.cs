using UnityEngine.SceneManagement;
public enum SceneNames 
{ Logo = 0,
  Login,
  Lobby,
  Game,
}
public static class Utils
{
    public static string GetActionScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadScene(string sceneName="")
    {
        if(sceneName == "")
        {
            SceneManager.LoadScene(GetActionScene());
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void LoadScene(SceneNames sceneName)
    {
        //열겨형으로 매개변수 받고 tostirng
        SceneManager.LoadScene(sceneName.ToString());
    }
}
