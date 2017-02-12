using UnityEngine;
using KPlugin.Debug;
using KPlugin.Common;
using KPlugin.Editor;
using KPlugin.Extension;
using System.Linq;
using KPlugin.Constant;
using System.Collections.Generic;
using System;

public class Test : SingletonMonoBehaviour<Test>
{
    [SerializeField, Console("x")]
    private static int x;

    [Console("y")]
    private static float y
    {
        set
        {
            x = (int)value;
        }

        get
        {
            return x;
        }
    }

    [SerializeField]
    private int _x;

    [SerializeField]
    private bool boolField;

    [SerializeField]
    private int intField;

    [SerializeField]
    private float floatField;

    [SerializeField]
    private string stringField;

    [SerializeMethod]
    public void AddIntBy10()
    {
        intField += 10;
    }

    [Console("setauto")]
    private void SetInt()
    {
        this.intField++;

        Debug.Log("setauto");
    }

    [Console("setauto")]
    private void SetInt(int intField)
    {
        this.intField = intField;
    }

    [Console("setauto")]
    private void SetInt(int int1, int int2)
    {
        this.intField = int1 + int2;
    }

    [Console("setauto")]
    private void SetInt(float floatField)
    {
        this.floatField = floatField;
    }

    [Console("setauto")]
    private void SetIntAndFloat(int intField, float floatField)
    {
        this.intField = intField;
        this.floatField = floatField;
    }

    [Console("setauto")]
    private void SetString(string stringField)
    {
        this.stringField = stringField;
    }

    [Console("setauto")]
    private void SetBool(bool boolField)
    {
        this.boolField = boolField;
    }

    [Flags]
    public enum SomeEnum
    {
        A = 1, B = 8
    }

    [SerializeField]
    private SomeEnum enumField;

    void Start()
    {

    }

    [Console("setenum")]
    public void SetEnum(SomeEnum enumField)
    {
        this.enumField = enumField;

        Debug.Log((int)enumField);
    }

    private void Update()
    {
        _x = x;
    }
}
