using System;
using System.Collections.Generic;
using System.Text;
using TextMining.Crawling;

namespace TextMining.Crawling
{
    public interface IAction
    {
        void Do(CNNPage page);
    }
}