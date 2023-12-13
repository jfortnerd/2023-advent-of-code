using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jfortnerd.Aod2k23.Tests
{
    abstract class Test
    {
        protected List<String> testInputs = new List<String>();

        public List<string> Inputs
        {
            get
            {
                return testInputs;
            }
        }

        public abstract void LoadTest();

        public void AddTestInput(String input)
        {
            testInputs.Add(input);
        }
    }
}
