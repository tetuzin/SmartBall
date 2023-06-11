using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShunLib.Data.Common
{
    public class CommonModel
    {
        
    }

    [System.Serializable]
    public class TestModel
    {
        [SerializeField] private string _string;
        [SerializeField] private bool _bool;
        [SerializeField] private int _int;
        [SerializeField] private int[] _intArray;
        [SerializeField] private List<int> _intList;
        [SerializeField] private Dictionary<int, string> _dict;
        [SerializeField] private Action _action;
        public string String
        {
            get { return _string; }
            set { _string = value; }
        }
        public bool Bool
        {
            get { return _bool; }
            set { _bool = value; }
        }
        public int Int
        {
            get { return _int; }
            set { _int = value; }
        }
        public int[] IntArray
        {
            get { return _intArray; }
            set { _intArray = value; }
        }
        public List<int> IntList
        {
            get { return _intList; }
            set { _intList = value; }
        }
        public Dictionary<int, string> Dict
        {
            get { return _dict; }
            set { _dict = value; }
        }
        public Action Action
        {
            get { return _action; }
            set { _action = value; }
        }
    }
}

