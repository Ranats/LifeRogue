using UnityEngine;

public static partial class GameObjectCommon{

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject Instantiate(Object original)
    {
        return GameObject.Instantiate(original) as GameObject;
    }

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <param name="position">位置</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject Instantiate(GameObject original, Vector3 position)
    {
        return GameObject.Instantiate(original, position, Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">角度</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation)
    {
        return GameObject.Instantiate(original, position, rotation) as GameObject;
    }

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject InstantiateChild(Object original, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(original) as GameObject;
        obj.transform.SetParent(parent);
        return obj;
    }

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <param name="position">位置</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject InstantiateChild(GameObject original, Vector3 position, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(original, position, Quaternion.identity) as GameObject;
        obj.transform.SetParent(parent);
        return obj;
    }

    /// <summary>
    /// ゲームオブジェクトを生成
    /// </summary>
    /// <param name="original">複製元オブジェクト</param>
    /// <param name="position">位置</param>
    /// <param name="rotation">角度</param>
    /// <returns>生成したゲームオブジェクト</returns>
    public static GameObject InstantiateChild(GameObject original, Vector3 position, Quaternion rotation, Transform parent)
    {
        GameObject obj = GameObject.Instantiate(original, position, rotation) as GameObject;
        obj.transform.SetParent(parent);
        return obj;
    }
}
