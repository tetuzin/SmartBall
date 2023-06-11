using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Scenario.Model
{
    [Serializable]
    public class ScenarioModel
    {
        [SerializeField] public string ScenarioName { get; set; }
        [SerializeField] public List<DialogModel> DialogList { get; set; }
    }
}
