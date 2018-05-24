using System;
/*
* Auxiliary class to gather information regarding to classes
*/
public class CharacterClass
{
    /// <summary>
    /// Classes id 
    /// </summary>
    public enum Classes
    {
        barbarian,
        warrior
    }

    /// <summary>
    /// Returns the class name by his id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetClassName(int id)
    {
        switch (id)
        {
            case 1:
                return "Barbarian";
            case 2:
                return "Warrior";
        }
        return "";
    }

}