using System.Collections.Generic;
using UnityEngine;

public class Testing_Shop_00234568 : MonoBehaviour
{



}


public class RandomItem
{
    List<Testing_Food> FoodPool = new List<Testing_Food>();
    List<Testing_Drug> DrugPool = new List<Testing_Drug>();
    List<Testing_Weapon> WeaponPool = new List<Testing_Weapon>();


    public Testing_Food RandFood(Testing_Food TypeFood)
    {
        return FoodPool[FoodPool.Count - 1];
    }

}


public class Testing_Item
{

}

public class Testing_Food : Testing_Item
{ 
    public void initialized_Shop_Item()
    {

    }
}

public class Testing_Drug : Testing_Item
{

}

public class Testing_Weapon : Testing_Item
{

}