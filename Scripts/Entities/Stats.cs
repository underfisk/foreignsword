public class Stats
{
    //TODO : REFACTOR !! to be edit yet this class is not done yet
    public int max_hp, mana, fury, energy, str, dex, agi, ap, def_min, def_max;
    public float speed;

    public Stats(int hp, int mana, int fury, int energy, int str, int dex, int agi, int ap, int def_min, int def_max)
    {
        this.max_hp = hp;
        this.mana = hp;
        this.fury = fury;
        this.energy = energy;
        this.str = str;
        this.dex = dex;
        this.agi = agi;
        this.ap = ap;
        this.def_min = def_min;
        this.def_max = def_max;
    }
}