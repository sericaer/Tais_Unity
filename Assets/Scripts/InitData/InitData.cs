﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaisEngine.ModManager;
using TaisEngine.Run;
using Tools;

namespace TaisEngine.Init
{
    public class InitData
    {
        internal static InitData inst;

        internal static void Generate()
        {
            inst = new InitData();
        }

        internal static void AssocVisitor()
        {
            Visitor.Add("init.taishou.background", () => inst.taishou.background, (obj) => inst.taishou.background = obj as string, typeof(string));
        }
                
        public Taishou taishou;

        public InitData()
        {
            taishou = new Taishou();
        }

        internal static InitData Random()
        {
            return new InitData()
            {
                taishou = Taishou.Random()
            };
        }
    }

    public class Taishou
    {

        public string background;

        public string name;

        public int age;

        public static (int min, int max) ageRange = (25, 55);

        internal static Taishou Random()
        {
            return new Taishou()
            {
                name = PersonName.RandomFull,
                background = "SHIZU",
                age = GRandom.getNum(ageRange.min, ageRange.max)
            };
        }
    }
}
