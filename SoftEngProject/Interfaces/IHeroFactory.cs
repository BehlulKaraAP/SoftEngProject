using SoftEngProject.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Interfaces
{
    internal interface IHeroFactory
    {
        Hero CreateHero(IInputReader inputReader);
    }
}
