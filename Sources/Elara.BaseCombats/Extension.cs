using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.BaseCombats
{

    public class BaseCombatsExtension : Extensions.IExtension
    {
        private Mage m_Mage;
        private Paladin m_Paladin;
        
        public Tuple<string, Action>[] Options => null;

        public bool OnEnable(Elara p_Elara)
        {
            m_Mage = new Mage(p_Elara);
            m_Paladin = new Paladin(p_Elara);

            p_Elara.AddCombatScript(m_Mage);
            p_Elara.AddCombatScript(m_Paladin);

            return true;
        }

        public bool OnDisable(Elara p_Elara)
        {
            p_Elara.RemoveCombatScript(m_Mage);
            p_Elara.RemoveCombatScript(m_Paladin);

            return true;
        }        
    }
}
