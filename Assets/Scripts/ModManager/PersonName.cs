using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaisEngine.ConfigManager;

namespace TaisEngine.ModManager
{
    internal class PersonName
    {
        internal static IEnumerable<string> EnumFamily()
        {
            foreach(var mod in Mod.listMod.Where(x=>x.content!= null))
            {
                if(!mod.content.localString.dictlan2PersonName.ContainsKey(Config.inst.lang))
                {
                    continue;
                }

                foreach(var name in mod.content.localString.dictlan2PersonName[Config.inst.lang].family)
                {
                    yield return name;
                }
            }
        }

        internal static IEnumerable<string> EnumGiven()
        {
            foreach (var mod in Mod.listMod.Where(x => x.content != null))
            {
                if (!mod.content.localString.dictlan2PersonName.ContainsKey(Config.inst.lang))
                {
                    continue;
                }

                foreach (var name in mod.content.localString.dictlan2PersonName[Config.inst.lang].given)
                {
                    yield return name;
                }
            }
        }

        internal static string RandomFull
        {
            get
            {
                return RandomFamily + RandomGiven;
            }
        }

        internal static string RandomFamily
        {
            get
            {
                return EnumFamily().OrderBy(x => Guid.NewGuid()).First();
            }
        }

        internal static string RandomGiven
        {
            get
            {
                return EnumGiven().OrderBy(x => Guid.NewGuid()).First();
            }
        }

        internal static PersonName Generate(string dir)
        {
            var familyNamePath = $"{dir}/personname/family.txt";
            var givenNamePath = $"{dir}/personname/given.txt";

            string[] family = { };
            if(File.Exists(familyNamePath))
            {
                family = File.ReadAllLines(familyNamePath);
            }

            string[] given = { };
            if (File.Exists(givenNamePath))
            {
                given = File.ReadAllLines(givenNamePath);
            }

            if(family.Count() == 0 && given.Count() == 0)
            {
                return null;
            }

            return new PersonName(family, given);
        }

        private PersonName(string[] family, string[] given)
        {
            this.family = family;
            this.given = given;
        }

        private string[] family;
        private string[] given;
    }
}
