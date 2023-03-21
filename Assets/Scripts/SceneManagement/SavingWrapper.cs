using GameDevTV.Saving;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{

    public class SavingWrapper : MonoBehaviour
    {
        const string _defaultSaveFile = "save";

        //IEnumerator Start()
        //{
        //    print("heyo");
        //    yield return GetComponent<JsonSavingSystem>().LoadLastScene(_defaultSaveFile);
        //}

        public void Save()
        {
            GetComponent<JsonSavingSystem>().Save(_defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<JsonSavingSystem>().Load(_defaultSaveFile);
        }
    }

}