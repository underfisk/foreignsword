using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Character
{
    private int _id;
    private string _name;
    private int lvl;
    private int exp;
    private int class_id;
    private Stats _stats;
    private Inventory inv;

    public int Level
    {
        get { return this.lvl; }
    }

    public int ID
    {
        get { return this._id; }
    }

    public string Name
    {
        get { return (!String.IsNullOrEmpty(this._name) ? this._name : "Name") ; }
    }

    public int ClassID
    {
        get { return this.class_id; }
    }

    public Inventory _Inventory
    {
        get { return this.inv; }
    }

    public Character(int nid, string name, int classid, int _lvl, Inventory ninv)
    {
        this._id = nid;
        this._name = name;
        this.class_id = classid;
        this.inv = ninv;
        this.lvl = _lvl;
    }

    
}
