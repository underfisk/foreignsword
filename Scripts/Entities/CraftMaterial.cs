using System;

public class CraftMaterial
{
  enum m_type{
    common,
    normal,
    rare,
    legendary
  }
  
  private int id;
  private string name;
  private string stacks; //the X of this item 
  private m_type type;
  private string img_name; //for resources load
  private bool isDropable; //some of them are binded
  
  /*
    Diablo 3 crafting materials properties:
    - id
    - name
    - stack (quant of this item)
    - type (used for distinguised)
    - img_name (for resources porpuses)
    - isDropable (to knoww whenever we can drop this item or being bind)
  */
}