#region

using System;
using LeagueSharp;
using LeagueSharp.Common;

#endregion

namespace ZestaR_s_Attack_Helper
{
    class AAreset
    {
        public static Spell Reset;  // reset list from SamuraiGilgamesh
        public static Boolean Special;
        public static void Champlist()
        {
            switch (ObjectManager.Player.ChampionName)
            {
                case "Jax": Reset = new Spell(SpellSlot.W);
                    break;
                case "Fiora": Reset = new Spell(SpellSlot.E);
                    break;
                case "Poppy": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Gangplank": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Talon": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Leona": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Blitzcrank": Reset = new Spell(SpellSlot.E);
                    break;
                case "Darius": Reset = new Spell(SpellSlot.W);
                    break;
                case "Garen": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Hecarim": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Kassadin": Reset = new Spell(SpellSlot.W);
                    break;
                case "MissFortune": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Mordekaiser": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Nasus": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Nautilus": Reset = new Spell(SpellSlot.W);
                    break;
                case "Renekton": Reset = new Spell(SpellSlot.W);
                    break;
                case "Rengar": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Sejuani": Reset = new Spell(SpellSlot.W);
                    break;
                case "Shyvana": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Sivir": Reset = new Spell(SpellSlot.W);
                    break;
                case "Trundle": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Teemo": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Vayne": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Vi": Reset = new Spell(SpellSlot.E);
                    break;
                case "Volibear": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Wukong": Reset = new Spell(SpellSlot.Q);
                    break;
                case "XinZhao": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Yorick": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Tristana": Reset = new Spell(SpellSlot.E);
                    break;
                case "MasterYi":
                    Reset = new Spell(SpellSlot.W);
                    Special = true;
                    break;
            }

        }


    }
}
