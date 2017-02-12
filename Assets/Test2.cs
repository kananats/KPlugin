using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using KPlugin.Editor;
using System;
using System.Linq;
using KPlugin.Extension;
using KPlugin.Common;
using System.Runtime.CompilerServices;

public class Test2 : MonoBehaviour
{
    public class Kuy<T>
    {
        T t;
        public int a;
        private int b;
        int[] x = new int[3];

        public Kuy(int a, int b)
        {
            t = default(T);
            this.a = a;
            this.b = b;
        }
        //override public string ToString()
        //{
        //    return "kuy";
        //}
    }
    void Start()
    {
        int[][] a = new int[3][];
        a[0] = new int[] { 1, 2, 3 };
        a[1] = new int[] { 4, 5 };
        List<int> b = new List<int> { 1, 2, 3, 4, 5 };
        Dictionary<string, int> d = new Dictionary<string, int>();
        d["kuy"] = 0;
        d["sus"] = 1;

        // todo
        //int[,] e = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } };
        int[,,] e = new int[,,] { { { 1, 2 }, { 3, 4 }, { 5, 6 } }, { { 7, 8 }, { 9, 10 }, { 11, 12 } }, { { 13, 14 }, { 15, 16 }, { 17, 18 } }, { { 19, 20 }, { 21, 22 }, { 23, 24 } } };

        Type t = typeof(int);

        Kuy<int> k = new Kuy<int>(1,2);

        //Debug.Log(k.ToSimplifiedString());

        string s = "kris";
    }

    [SerializeMethod]
    public void A1(int a)
    {

    }

    [SerializeMethod]
    public void A2(int b = 1)
    {
        Debug.Log(b);
    }

    [SerializeMethod]
    public void A7(int b = 1, int c = 2)
    {
        Debug.Log(b + c);
    }

    [SerializeMethod]
    public void A8(int b, int c = 2)
    {

    }

    [SerializeMethod]
    public T A3<T>(T t)
    {
        return t;
    }

    [SerializeMethod]
    public T A4<T>(T t = default(T))
    {
        return t;
    }

    [SerializeMethod]
    public void A5(ref int a)
    {

    }

    [SerializeMethod]
    public void A6(out int a)
    {
        a = 3;
    }

    /*
    static void Log(object message)
    {
        // frame 1, true for source info
        StackFrame frame = new StackFrame(1, true);
        var method = frame.GetMethod();
        var fileName = frame.GetFileName();
        var lineNumber = frame.GetFileLineNumber();

        // we'll just use a simple Console write for now    
        Console.WriteLine("{0}({1}):{2} - {3}", fileName, lineNumber, method.Name, message);
    }
    */
}
