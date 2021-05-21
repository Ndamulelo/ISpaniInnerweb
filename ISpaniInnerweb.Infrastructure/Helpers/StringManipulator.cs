﻿using ISpaniInnerweb.Domain.Interfaces.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Infrastructure.Helpers
{
    public class StringManipulator : IStringManipulator
    {
        public IList<string> BreakDownText(string longtext)
        {
            IList<string> arrayOfTexts = longtext.Split(",");
            return arrayOfTexts;
        }
    }
}
