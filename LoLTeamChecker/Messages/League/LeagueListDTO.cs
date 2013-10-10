/*
copyright (C) 2011-2012 by high828@gmail.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluorineFx;
using LoLTeamChecker.Assets;
using LoLTeamChecker.Flash;

namespace LoLTeamChecker.Messages.League
{
	public class LeagueListDTO	: BaseObject
	{
        public LeagueListDTO(ASObject obj)
			: base(obj)
		{
			BaseObject.SetFields(this, obj);
		}

        [InternalName("queue")]
        public string Queue { get; set; }
        [InternalName("name")]
        public string Name { get; set; }
        [InternalName("tier")]
        public string Tier { get; set; }
        [InternalName("requestorsRank")]
        public string RequestorsRank { get; set; }
        [InternalName("entries")]
        public LeagueItemDTOList entries { get; set; }
        [InternalName("requestorsName")]
        public string RequestorsName { get; set; }

	}
}
