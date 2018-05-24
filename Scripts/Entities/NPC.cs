using System;
public class NPC
{
  private enum Job
  {
    shop,
    alchemist,
    blacksmith,
    legendary_tran, //kanai cube d3
    quest,
    sq //its quest and shopp at same time
  }
  private int id;
  private string name;
  private Job type;
  
  /*public NPC(int n_id, string n_name, Job n_type)
  {
    this.id = n_id;
    this.name = n_name;
    this.type = n_type;
  }*/
  
  public int GetID(){
    return this.id;
  }
  
}