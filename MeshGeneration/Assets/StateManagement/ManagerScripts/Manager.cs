using UnityEngine;
using System;

//a robust singleton class that is able to be inherited 
//from which makes use of templating to allow for better reuse of code.
//(singleton, only ever one of this active)


//is an abstract class, more features/functionality in child/derived classes
//only instances if classes that inherit from this

//also is a templated class that will take only a direct child (due to DerivedManager : Manager<DerivedManager>)
//as part of instanstiation
public abstract class Manager< DerivedManager > where DerivedManager : Manager< DerivedManager >
{

    protected abstract void Terminate();

    private static DerivedManager s_Instance;
    //get instance of derived manager
    public static DerivedManager Instance
    {
        get
        {
            return s_Instance;
        }
    }

    //Create instance of our Manager class
    public static DerivedManager Create()
    {
        try
        {
            if (null == s_Instance)
            {
                s_Instance = Activator.CreateInstance(typeof(DerivedManager), true) as DerivedManager;
            }
            else
            {
                string exceptionMessage = System.String.Format("Instance of {0} already exists", typeof(DerivedManager).ToString());
                throw new Exception(exceptionMessage);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return s_Instance;
    }

    //Destroy instance of our Manager class
    public static void Destroy()
    {
        s_Instance.Terminate();
    }

    //just for error checking reasons
    public override String ToString()
    {
        return "Base Manager Class";
    }

}
