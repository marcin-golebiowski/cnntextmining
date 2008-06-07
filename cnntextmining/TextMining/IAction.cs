using System;
using System.Collections.Generic;
using System.Text;

namespace TextMining
{
    public interface IAction
    {
        void Do(CNNPage page);
    }
}
