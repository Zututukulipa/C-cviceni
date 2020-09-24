using System;

namespace Delegate
{


    public enum Faculty
    {
        FES = 0,
        FF = 1,
        FEI = 2,
        FCHT = 3

    }

    public class EnumCaller
    {
        public static Faculty getRandomFaculty()
        {
            Random r = new Random();
            var v = Enum.GetValues(typeof(Faculty));
            return (Faculty)v.GetValue(r.Next(3));
        }
    }
}

