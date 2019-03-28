using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ResourcesManager {
    private static Material land;
    private static Material sea;

    public static Material Land
    {
        get
        {
            if (land == null) {
                land = Resources.Load<Material>("land");
            }
            return land;
        }
    }

    public static Material Sea
    {
        get
        {
            if (sea == null) {
                sea = Resources.Load<Material>("sea");
            }
            return sea;
        }
    }
}
